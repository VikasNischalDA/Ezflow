using System.ComponentModel.DataAnnotations.Schema;

namespace LeadManagementSystem.Model.Models
{
    [Table("LeadStatusHistory")]
    public class LeadStatusModel { 
        public int Id { get; set; }
        public string? StatusIdFrom { get; set; }
        public string? SubStatusIdFrom { get; set; }
        public int LeadId { get; set; }
        public string UMID { get; set; }
        public string? StatusIdTo { get; set; }
        public string? SubStatusIdTo { get; set; }
        public string? System { get; set; }
        public string? DalasUserName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
        public LeadModel LeadModel { get; set; }

        // Navigation property to LeadSource
        public LeadSourceModel LeadSource { get; set; }

    }
}
