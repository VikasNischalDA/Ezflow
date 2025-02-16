using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LeadManagementSystem.Model.Models
{
    [Table("LESResponse")]
    public class LesResponseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Decision { get; set; }
        public string BrandName { get; set; }
        public string CustomerType { get; set; }
        public string MessageId { get; set; }

        public int LeadId { get; set; } 
        public LeadModel Lead { get; set; }
    }
}
