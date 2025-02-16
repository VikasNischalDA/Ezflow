namespace LeadManagementSystem.Spoof
{
    public class LES_Spoofing
    {
        public async Task<LESSpoofResponse> LESLeadCheck(LESSpoofInput LESspoofInput)
        {

            string decision;
            List<string> decisionReasons = [];
            string customerType = "New";
            if (LESspoofInput.IdNumber == "9102050303085")
            {
                decision = "Decline";
                decisionReasons.Add("LES: Active Application Status Decline Result");
            }
            else if (LESspoofInput.IdNumber == "3102050303085")
            {
                decision = "Accept";
                decisionReasons.Add("LES: Accepted Application");
            }
            else
            {
                decision = "MayBe";
                decisionReasons.Add("LES: May be We Can Accept Application");
            }

            return new LESSpoofResponse
            {
                Decision = decision,
                BrandName = LESspoofInput.BrandName,
                DecisionReasons = decisionReasons,
                CustomerType = customerType,
                DoNotPromote = new DoNotPromote
                {
                    AllowEmail = true,
                    AllowMail = true,
                    AllowSms = true,
                    AllowTelephone = true
                },
                DecisionCodes = new List<string>(),
                MessageId = LESspoofInput.MessageId
            };
        }
    }
}
