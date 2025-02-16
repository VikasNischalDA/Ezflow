using System.Xml;
using System.Xml.Serialization;

namespace LeadManagementSystem.Contracts.Request
{

    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
    public class DbsServiceCall
    {
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Namespaces = new XmlSerializerNamespaces(new[]
        {
            new XmlQualifiedName("xsd", "http://www.w3.org/2001/XMLSchema"),
            new XmlQualifiedName("soap", "http://schemas.xmlsoap.org/soap/envelope/"),
            new XmlQualifiedName("xsi", "http://www.w3.org/2001/XMLSchema-instance"),
            new XmlQualifiedName("tns", "http://datapp12.directaxiz.co.za:9083/ws/")
        });


        [XmlElement(ElementName = "Header", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public SoapHeader Header { get; set; }

        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public SoapBody Body { get; set; }
    }

    public class SoapHeader
    {
        [XmlElement(ElementName = "authentication", Namespace = "http://datapp12.directaxiz.co.za:9083/ws/")]
        public Authentication Authentication { get; set; }
    }
    public class Authentication
    {
        [XmlElement(ElementName = "username", Namespace = "")]
        public string Username { get; set; }

        [XmlElement(ElementName = "password", Namespace = "")]
        public string Password { get; set; }
    }

    public class SoapBody
    {
        [XmlElement(ElementName = "RequestRiskGrade", Namespace = "http://datapp12.directaxiz.co.za:9083/ws/")]
        public DbsRequest RequestRiskGrade { get; set; }
    }
}
