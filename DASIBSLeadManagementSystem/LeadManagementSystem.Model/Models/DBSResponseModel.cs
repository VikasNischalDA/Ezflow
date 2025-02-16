using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace LeadManagementSystem.Model.Models
{
    [Table("DBSResponse")]
    public class DBSResponseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string IDNumber { get; set; }
        public long BrandID { get; set; }
        public string RiskCellOrProfile { get; set; }
        public bool ApplicationFound { get; set; }
    }
}
