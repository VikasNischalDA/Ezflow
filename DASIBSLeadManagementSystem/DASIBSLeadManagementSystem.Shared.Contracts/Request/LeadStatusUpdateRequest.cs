using System.Diagnostics.CodeAnalysis;

namespace LeadManagementSystem.Shared.Contracts.Request
{
    public class LeadStatusUpdateRequest
    {
        public int LeadId { get; set; }
        public int StatusIdFrom { get; set; }
        public int StatusIdTo { get; set; }
        [MaybeNull]
        public string SubStatusIdFrom { get; set; }
        [MaybeNull]
        public string SubStatusIdTo { get; set; }
        public string DALASUserName { get; set; }
        public string BrandID { get; set; }
        public string BrandAppID { get; set; }

    } 


}
