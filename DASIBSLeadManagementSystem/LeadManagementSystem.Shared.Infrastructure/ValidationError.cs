using System.Xml.Serialization;

namespace LeadManagementSystem.Shared.Infrastructure
{

    [XmlRoot("ValidationError")]
    public class ValidationError
    {
        [XmlElement("FieldName")]
        public string FieldName { get; set; }

        [XmlElement("ErrorMessage")]
        public string ErrorMessage { get; set; }
    }
}
