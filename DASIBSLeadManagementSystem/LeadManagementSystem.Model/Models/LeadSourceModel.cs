using System.ComponentModel.DataAnnotations.Schema;

namespace LeadManagementSystem.Model.Models
{
    [Table("LeadSource")]
    public class LeadSourceModel
    {
        public int Id { get; set; }
        public string LeadSourceClass { get; set; }
        public int LeadSourceId { get; set; }
        public bool DigitalLeadSource { get; set; }
        public bool Active { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string ChangedBy { get; set; }
        public string BusinessUnit { get; set; }
        public string LeadSource { get; set; }
        public string CustomerFriendlyName { get; set; }
        public string UMID { get; set; }
        public string Brand { get; set; }
        public int BrandId { get; set; }
        public BrandModel BrandModel { get; set; }
        public string TelephoneNumber { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string SubBrandName { get; set; }

        // Navigation property to access related LeadResponses
        public ICollection<LeadResponseModel> LeadResponses { get; set; }

        // Navigation property to LeadStatusHistory
        public ICollection<LeadStatusModel> LeadStatusHistories { get; set; }

    }
}
