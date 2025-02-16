using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeadManagementSystem.Model.Models
{
    [Table("GetCustomerStatusResponse")]
    public class GetCustomerStatusResponseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Result { get; set; }

        public string MessageCode { get; set; }

        public string Message { get; set; }

        public int BrandId { get; set; }

        public bool IsRepeat { get; set; }

        public bool HasApplication { get; set; }

        public string? ApplicationStatus { get; set; }

        public string? ApplicationSubStatus { get; set; }

        public bool IsRepeatExpired { get; set; }

        public int LeadId { get; set; }

        public LeadModel LeadModel { get; set; } 

    }
}
