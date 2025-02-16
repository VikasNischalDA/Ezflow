namespace LeadManagementSystem.Spoof
{
    public class LESSpoofResponse
    {
        public required string Decision { get; set; }
        public required string BrandName { get; set; }
        public required List<string> DecisionReasons { get; set; }
        public required string CustomerType { get; set; }
        public required DoNotPromote DoNotPromote { get; set; }
        public List<string>? DecisionCodes { get; set; }
        public string? MessageId { get; set; }

    }

    public class DoNotPromote
    {
        public bool AllowEmail { get; set; }
        public bool AllowMail { get; set; }
        public bool AllowSms { get; set; }
        public bool AllowTelephone { get; set; }
    }
}
