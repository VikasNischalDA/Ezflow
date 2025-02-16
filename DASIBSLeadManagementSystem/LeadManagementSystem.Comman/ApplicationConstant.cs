
namespace LeadManagementSystem.Comman
{
    public static class ApplicationConstant
    {
        public static string Source { get; set; } = "EzFlow";

        public const string DecisionDecline = "Decline";
        public const string DecisionApprove = "Approve";
        public const string DecisionMayBe = "Maybe";

        public const string ConfirmDeclined = "Cancelled: Message To be confirmed";
        public const string ConsentDeclined = "Cancelled: Consent Declined";

        public const string TwoMonths = "Thank you for applying for a personal loan. Unfortunately your application was unsuccessful as you do not meet our minimum requirements. You are welcome to re-apply in 2 months.";

        public const string AccountName = "DA_Leads";
        public const string SystemCode = "Ezflw";
        public const string Language = "English";
    }
}
