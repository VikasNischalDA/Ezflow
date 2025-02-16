using System.Xml.Serialization;

namespace LeadManagementSystem.Contracts.Response
{
 
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class LeadStatusUpdateEnvelope
    {
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Xmlns { get; set; }

        [XmlElement(ElementName = "Body")]
        public LeadStatusUpdateBody Body { get; set; }

        public LeadStatusUpdateEnvelope()
        {
            Xmlns = new XmlSerializerNamespaces();
            Xmlns.Add("soap", "http://schemas.xmlsoap.org/soap/envelope/");
            Xmlns.Add("tns", "http://dapdev03.direcaxis.co.za");
            Xmlns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            Xmlns.Add("xsd", "http://www.w3.org/2001/XMLSchema");
        }
    }

    public class LeadStatusUpdateBody
    {
  
        [XmlElement(ElementName = "LeadStatusUpdateResponse", Namespace = "http://dapdev03.direcaxis.co.za")]
        public LeadStatusUpdateResponse LeadStatusUpdateResponse { get; set; }
    }

    public class LeadStatusUpdateResponse
    {
        [XmlElement(ElementName = "WebServiceMessage", Namespace = "")]
        public WebServiceMessage WebServiceMessage { get; set; }
    }

    public class WebServiceMessage
    {
        [XmlElement(ElementName = "Success", Namespace = "")]
        public bool Success { get; set; }

        [XmlElement(ElementName = "ErrorMessage", Namespace = "")]
        public string ErrorMessage { get; set; }
    }
}