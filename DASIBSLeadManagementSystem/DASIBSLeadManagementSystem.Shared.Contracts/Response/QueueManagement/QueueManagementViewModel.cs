using LeadManagementSystem.Shared.Contracts.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSystem.Shared.Contracts.Response
{
    public class QueueManagementViewModel : PaginationResponse
    {
        public List<LeadRequest> AllLeads { get; set; }
        List<object> AllTasks { get; set; }
        List<object> AllGlobalQueues { get; set; }
    }
}
