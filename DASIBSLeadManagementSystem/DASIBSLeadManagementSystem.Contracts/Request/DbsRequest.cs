using System.Xml.Serialization;

namespace LeadManagementSystem.Contracts.Request
{
    public class DbsRequest
    {
        [XmlElement(ElementName = "IDNumber")]
        public string IdNumber { get; set; }


        [XmlElement(ElementName = "brandID")]
        public int BrandId { get; set; }
    }
}
