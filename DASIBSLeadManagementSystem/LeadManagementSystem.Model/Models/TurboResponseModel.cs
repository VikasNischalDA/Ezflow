using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeadManagementSystem.Model.Models
{
    [Table("TurboContractResponse")]
    public class TurboResponseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int InternetTurboApplicationId { get; set; }

        public string Message { get; set; }

        public long LeadId { get; set; }
        
        public string ResultCode { get; set; }
    }
}
