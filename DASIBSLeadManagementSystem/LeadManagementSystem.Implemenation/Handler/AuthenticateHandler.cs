using LeadManagementSystem.Contracts.Request;
using LeadManagementSystem.Contracts.Response;
using MediatR;
using LeadManagementSystem.Shared.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.DirectoryServices.Protocols;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LeadManagementSystem.Common.Helpers;


namespace LeadManagementSystem.Logic.Handler
{
    public class AuthenticateHandler : IRequestHandler<LoginRequest, ActionResult<LoginResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RegisterLeadHandler> _logger;
        private string _ldapHost = string.Empty;
        private string _ldapPort = string.Empty;
        private string _searchBase = string.Empty;

        public AuthenticateHandler(IConfiguration configuration,
                                   ILogger<RegisterLeadHandler> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<ActionResult<LoginResponse>> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var response = await LDAPConnect(request);            
            if (response.Success)
            {
                return new Shared.Infrastructure.ActionResult<LoginResponse>(ActionResultCode.Success,response);
            }
            else
            {
                return new Shared.Infrastructure.ActionResult<LoginResponse>(
                    ActionResultCode.Error,
                    new List<ValidationError>
                    {
                        new ValidationError
                        {
                            FieldName = "Error",
                            ErrorMessage = response.ErrorMessage!
                        }
                    }
                );
            }
        }

        private async Task<LoginResponse> LDAPConnect(LoginRequest request)
        {
            string errorMessage = string.Empty;
            string _searchFilter = string.Empty;
            var loginResponse = new LoginResponse();

            var LDAPSettings = _configuration.GetSection("LDAPSettings");
            if (LDAPSettings != null)
            {
                _ldapHost = LDAPSettings["Host"];
                _ldapPort = LDAPSettings["Port"];
                _searchBase = LDAPSettings["SearchBase"];
            }

            try
            {
                string decryptedPassword = CryptographyHelper.Decrypt(request.Password);
                var ldapIdentifier = new LdapDirectoryIdentifier(_ldapHost, Convert.ToInt32(_ldapPort));
                using (var ldapConnection = new LdapConnection(ldapIdentifier))
                {
                    ldapConnection.AuthType = AuthType.Basic;
                    ldapConnection.SessionOptions.ProtocolVersion = 3;
                    ldapConnection.Timeout = TimeSpan.FromSeconds(60); //Set a timeout
                    ldapConnection.Credential = new NetworkCredential(request.Username, decryptedPassword);
                    ldapConnection.Bind();//Attempt to bind with the given credentials

                    _searchFilter = $"(samAccountName={request.Username})";
                    var searchRequest = new SearchRequest(
                        _searchBase,
                        _searchFilter,
                        SearchScope.Subtree,
                        "cn", "sAMAccountName", "userPrincipalName", "description" // Specify the attributes to retrieve
                    );

                    var searchResponse = (SearchResponse)ldapConnection.SendRequest(searchRequest);
                    if (searchResponse.Entries.Count > 0)
                    {
                        var entry = searchResponse.Entries[0];

                        string commonName = entry.Attributes["cn"]?.GetValues(typeof(string))?.FirstOrDefault()?.ToString();
                        string samAccountName = entry.Attributes["sAMAccountName"]?.GetValues(typeof(string))?.FirstOrDefault()?.ToString();
                        string userPrincipalName = entry.Attributes["userPrincipalName"]?.GetValues(typeof(string))?.FirstOrDefault()?.ToString();
                        string description = entry.Attributes["description"]?.GetValues(typeof(string))?.FirstOrDefault()?.ToString();

                        var token = GenerateJwtToken(request.Username);
                        loginResponse.Token = token;
                        loginResponse.Success = true;
                        loginResponse.UserPrincipalName = userPrincipalName;
                        loginResponse.Username = samAccountName;
                        loginResponse.Description = description;
                    }
                    else
                    {
                        loginResponse.Success = false;
                        loginResponse.ErrorMessage = "User not found in LDAP.";
                    }
                }
            }
            catch(LdapException ex)
            {
                loginResponse.ErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                loginResponse.Success = false;
                loginResponse.ErrorMessage = $"Authentication failed: {ex.Message}";
            }
            return loginResponse;
        }

        private string GenerateJwtToken(string userName)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
