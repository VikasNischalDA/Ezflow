using LeadManagementSystem.Contracts.Request;
using LeadManagementSystem.Contracts.Response;
using LeadManagementSystem.Model.Models;
using LeadManagementSystem.Providers.HttpProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public class SmsService
{
    private readonly ExternalSMSProvider externalSmsProvider;
    private readonly ILogger<SmsService> logger;
    private readonly string smsEndpoint;

    public SmsService(ExternalSMSProvider _externalSmsProvider, ILogger<SmsService> _logger,IConfiguration configuration)
    {
        externalSmsProvider = _externalSmsProvider;
        logger = _logger;
        smsEndpoint = configuration["Service:SMSUrl"];
    }

    public async Task<string> SendSmsAsync(LeadRequest leadRequest, string messageTypeCode, LeadSourceModel leadSource)
    {
        try
        {
            var client = externalSmsProvider.GetClient();
            logger.LogInformation("SMS Call: HTTP client for SMS service created.");

            var smsRequest = externalSmsProvider.CreateSmsRequest(leadRequest, messageTypeCode, leadSource);
            logger.LogInformation("SMS Call: Created SMS request for SMS service {@smsRequest}.", smsRequest);

            var smsResponse = await externalSmsProvider.PostAsync<SMSRequest, Guid>(client, smsRequest, smsEndpoint);
            logger.LogInformation("SMS Call: Response received from SMS endpoint: {@SmsResponse}", smsResponse);

            return smsResponse.ToString();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "SMS Call: Failed to get response from SMS endpoint.");
            throw new Exception("Failed to get the response from SMS endpoint", ex);
        }
    }
}
