using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeadManagementSystem.Model.Models
{
    [Table("LeadRequest")]
    public class LeadModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string IDNumber { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string CellPhone { get; set; }

        public string AlternateNumber { get; set; }

        public string Email { get; set; }

        public string SupplierEmail { get; set; }

        public string UMID { get; set; }

        public string SupplierSource { get; set; }

        public string SMSresponse { get; set; }

        public string Option { get; set; }

        public string WorkNumber { get; set; }

        public string HomeNumber { get; set; }

        public string PermissionToPromote { get; set; }

        public bool AllowsCreditCheck { get; set; }

        public double GrossIncome { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ICollection<LeadStatusModel> LeadStatusHistory { get; set; }
      

        public ICollection<GetCustomerStatusResponseModel> GetCustomerStatusResponses { get; set; }
    }
}
