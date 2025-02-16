using System.Xml;
using System.Xml.Serialization;

namespace LeadManagementSystem.Contracts.Request
{
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
    public class TurboServiceCall
    {
        [XmlElement(ElementName = "Body")]
        public TurboSoapBody Body { get; set; }
    }

    public class TurboSoapBody
    {
        [XmlElement(ElementName = "TurboApplicationContract", Namespace = "urn:DA-InternetLink-Service")]
        public TurboApplicationContractRequest TurboApplicationContract { get; set; }
    }

}