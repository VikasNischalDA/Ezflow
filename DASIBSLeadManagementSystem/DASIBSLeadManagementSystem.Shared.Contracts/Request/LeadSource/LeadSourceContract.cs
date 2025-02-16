using LeadManagementSystem.Shared.Contracts.Response;

namespace LeadManagementSystem.Shared.Contracts.Request
{
    public class LeadSourceContract
    {
        public int? Id { get; set; }
        public string LeadSourceClass { get; set; }
        public int LeadSourceId { get; set; }
        public bool DigitalLeadSource { get; set; }
        public bool Active { get; set; }
        public DateTime DateTime { get; set; }
        public string ChangedBy { get; set; }
        public string BusinessUnit { get; set; }
        public string LeadSource { get; set; }
        public string CustomerFriendlyName { get; set; }
        public string UMID { get; set; }        
        public int BrandId { get; set; }
        public string TelephoneNumber { get; set; }               
    }
}
