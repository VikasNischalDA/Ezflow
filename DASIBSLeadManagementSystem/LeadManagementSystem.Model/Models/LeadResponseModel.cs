using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeadManagementSystem.Model.Models
{
    [Table("LeadResponse")]
    public class LeadResponseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? ErrorMessage { get; set; }
        public int LeadId { get; set; }
        public string UMID { get; set; }
        public string? BrandApplicationId { get; set; }
        public string? ApplicationId { get; set; }
        public string? BrandId { get; set; }
        public string Decision { get; set; }
        public string Status { get; set;}
        public string? LesResponse { get; set; }
        public string? DbsResponse { get; set; }
        public string? TurboResponse { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation property for LeadSourceModel 
        public LeadSourceModel LeadSources { get; set; }

    }
}
