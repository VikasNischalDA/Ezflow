namespace LeadManagementSystem.Shared.Contracts.Response
{
    public class LeadRecordResponse
    {
        public string Class { get; set; }
        public string LeadSource { get; set; }
        public int LeadReceived { get; set; }
        public int LeadsNotProcessed { get; set; }
        public int AppCapture { get; set; }
        public int CreditApps { get; set; }
        public int ValidationApps { get; set; }
        public int FraudApps { get; set; }
        public int ContractApplications { get; set; }
    }
}
