using System.ComponentModel.DataAnnotations.Schema;

namespace LeadManagementSystem.Model.Models
{
    [Table("BrandMaster")]
    public class BrandModel
    {
        public int Id { get; set; }

        public int BrandId { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string SubBrand { get; set; }

        public bool Active { get; set; }

        public ICollection<LeadSourceModel> LeadSourceModel { get; set; }
    }
}
