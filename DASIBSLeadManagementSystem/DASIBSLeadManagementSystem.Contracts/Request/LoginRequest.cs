using LeadManagementSystem.Contracts.Response;
using LeadManagementSystem.Shared.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSystem.Contracts.Request
{    
    public class LoginRequest : Shared.Contracts.Request.LoginRequest, IRequest<ActionResult<LoginResponse>>
    {

    }
}
