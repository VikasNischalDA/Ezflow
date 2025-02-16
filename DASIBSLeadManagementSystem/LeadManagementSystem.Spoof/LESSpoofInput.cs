namespace LeadManagementSystem.Spoof
{
    public class LESSpoofInput
    {
        public LESSpoofInput(string? messageId, string idNumber, string brandName, string source, string umid, string leadSource, string? customerId)
        {
            MessageId = messageId;
            IdNumber = idNumber;
            BrandName = brandName;
            Source = source;
            UMID = umid;
            LeadSource = leadSource;
            CustomerId = customerId;
        }

        public string? MessageId { get; set; }
        public string IdNumber { get; set; }
        public string BrandName { get; set; }
        public string Source { get; set; }
        public string UMID { get; set; }
        public string LeadSource { get; set; }
        public string? CustomerId { get; set; }
    }
}