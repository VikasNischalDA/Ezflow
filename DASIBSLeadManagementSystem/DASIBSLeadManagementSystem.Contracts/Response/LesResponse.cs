
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSystem.Contracts.Response
{
    /// <summary>
    /// 
    /// </summary>
    public class LesResponse
    {
      
        public string Decision { get; set; }
        public string BrandName { get; set; }
        public List<string> DecisionReasons { get; set; }
        public string CustomerType { get; set; }
        public DoNotPromote DoNotPromote { get; set; }
        public List<string> DecisionCodes { get; set; }
        public string MessageId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DoNotPromote
    {
        public bool AllowEmail { get; set; }
        public bool AllowMail { get; set; }
        public bool AllowSms { get; set; }
        public bool AllowTelephone { get; set; }
    }

}
