using System.Runtime.Serialization;

namespace LeadManagementSystem.Shared.Contracts
{
    public class LeadResponse
    {
        [IgnoreDataMember]
        public int Id { get; set; }

        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public int Lead { get; set; }
        public required string Decision { get; set; }
        public required string Status { get; set; }
    }
}
