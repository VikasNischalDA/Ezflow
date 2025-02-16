namespace LeadManagementSystem.Contracts.Request
{
    public class SMSRequest
    {
        public string MessageTypeCode { get; set; }
        public string AccountName { get; set; }
        public string PhoneNumber { get; set; }
        public string SourceSystemCode { get; set; }
        public string? SourceSystemReference { get; set; }
        public string? IdNumber { get; set; }
        public int? MplNumber { get; set; }
        public string? Language { get; set; }
        public PersonalisationData PersonalisationData { get; set; }
        public ContextData ContextData { get; set; }
    }
    public class PersonalisationData
    {
        public string? LeadSourceNumber { get; set; }
    }

    public class ContextData
    {
        public int? ProductID { get; set; }
    }
}
