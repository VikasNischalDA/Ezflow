using System.Xml;
using System.Xml.Serialization;

namespace LeadManagementSystem.Contracts.Response
{
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class CustomerStatusResponse
    {

        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Namespaces = new XmlSerializerNamespaces(new[]
        {
            new XmlQualifiedName("xsd", "http://www.w3.org/2001/XMLSchema"),
            new XmlQualifiedName("soap", "http://schemas.xmlsoap.org/soap/envelope/"),
            new XmlQualifiedName("xsi", "http://www.w3.org/2001/XMLSchema-instance")
        });

        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public CustomerStatusResponseBody Body { get; set; }

    }

    public class CustomerStatusResponseBody
    {
        [XmlElement(ElementName = "GetCustomerStatusResponse", Namespace = "urn:DA-InternetLink-Service")]
        public GetCustomerStatusResponse GetCustomerStatusResponse { get; set; }
    }

    public class GetCustomerStatusResponse
    {
        [XmlElement(ElementName = "GetCustomerStatusResult")]
        public GetCustomerStatusResult GetCustomerStatusResult { get; set; }
    }

    public class GetCustomerStatusResult
    {
        [XmlElement(ElementName = "Result")]
        public string Result { get; set; }

        [XmlElement(ElementName = "MessageCode")]
        public string MessageCode { get; set; }

        [XmlElement(ElementName = "Message")]
        public string Message { get; set; }

        [XmlElement(ElementName = "IdNumber")]
        public string IdNumber { get; set; }

        [XmlElement(ElementName = "CustomerStatus")]
        public CustomerStatus CustomerStatus { get; set; }
    }

    public class CustomerStatus
    {
        [XmlElement(ElementName = "CustomerBrandStatus")]
        public CustomerBrandStatus CustomerBrandStatus { get; set; }
    }

    public class CustomerBrandStatus
    {
        [XmlElement(ElementName = "BrandId")]
        public int BrandId { get; set; }

        [XmlElement(ElementName = "IsRepeat")]
        public bool IsRepeat { get; set; }

        [XmlElement(ElementName = "HasApplication")]
        public bool HasApplication { get; set; }

        [XmlElement(ElementName = "ReApplyRestrictionDate", IsNullable = true)]
        public DateTime? ReApplyRestrictionDate { get; set; }

        [XmlElement(ElementName = "ApplicationStatus")]
        public string ApplicationStatus { get; set; }

        [XmlElement(ElementName = "ApplicationSubStatus")]
        public string ApplicationSubStatus { get; set; }

        [XmlElement(ElementName = "IsRepeatExpired")]
        public bool IsRepeatExpired { get; set; }
    }
}
