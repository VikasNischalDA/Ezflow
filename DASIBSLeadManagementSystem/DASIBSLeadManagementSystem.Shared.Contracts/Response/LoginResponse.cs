using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSystem.Shared.Contracts.Response
{
    public class LoginResponse
    {
        public bool Success { get; set; } = false;
        public string? ErrorMessage { get; set; } = string.Empty;
        public string? Token { get; set; } = string.Empty;
        public string Username { get; set; }
        public string UserPrincipalName { get; set; }
        public string Description { get; set; }
    }
}
