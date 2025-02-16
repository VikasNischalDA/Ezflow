using System.Xml.Serialization;

namespace LeadManagementSystem.Contracts.Request
{
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class CustomerStatusRequest
    {
        public CustomerStatusRequest()
        {
            Namespaces = new XmlSerializerNamespaces();
            Namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            Namespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");
            Namespaces.Add("soap", "http://schemas.xmlsoap.org/soap/envelope/");
        }

        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Namespaces { get; set; }

        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public CustomerStatusBody CustomerStatusBody { get; set; }
    }

    public class CustomerStatusBody
    {
        [XmlElement(ElementName = "GetCustomerStatus", Namespace = "urn:DA-InternetLink-Service")]
        public GetCustomerStatusRequest GetCustomerStatusRequest { get; set; }
    }

    public class GetCustomerStatusRequest
    {
        [XmlElement(ElementName = "customerStatusInput")]
        public CustomerStatusInput CustomerStatusInput { get; set; }
    }

    public class CustomerStatusInput
    {
        [XmlElement(ElementName = "IdNumber")]
        public string IdNumber { get; set; }

        [XmlArray(ElementName = "BrandIds")]
        [XmlArrayItem(ElementName = "int")]
        public List<int> BrandIds { get; set; }
    }

}
