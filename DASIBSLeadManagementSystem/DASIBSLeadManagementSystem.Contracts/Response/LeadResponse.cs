using LeadManagementSystem.Shared.Contracts;
using System.Xml.Serialization;

namespace LeadManagementSystem.Contracts.Response
{
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class LeadResponseEnvelope
    {
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Xmlns { get; set; }

        [XmlElement(ElementName = "Body")]
        public LeadResponseBody Body { get; set; }

        public LeadResponseEnvelope()
        {
            Xmlns = new XmlSerializerNamespaces();
            Xmlns.Add("soap", "http://schemas.xmlsoap.org/soap/envelope/");
            Xmlns.Add("tns", "http://dapdev03.direcaxis.co.za");
            Xmlns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            Xmlns.Add("xsd", "http://www.w3.org/2001/XMLSchema");
        }
    }

    public class LeadResponseBody
    {
        [XmlElement(ElementName = "CreateLead_FromRequest_HDResponse", Namespace = "http://dapdev03.direcaxis.co.za")]
        public CreateLeadFromRequestHDResponse CreateLeadFromRequestHDResponse { get; set; }
    }

    public class CreateLeadFromRequestHDResponse
    {
        [XmlElement(ElementName = "WebServiceMessage", Namespace ="")]
        public LeadResponse WebServiceMessage { get; set; }
    }


    public class LeadResponse
    {
        [XmlIgnore]
        public int Id { get; set; }

        [XmlElement(ElementName = "Success", Namespace = "")]
        public bool Success { get; set; }

        [XmlElement(ElementName = "Status", Namespace = "")]
        public string? Status { get; set; }

        [XmlElement(ElementName = "Lead", Namespace = "")]
        public string LeadId { get; set; }
        public string? BrandApplicationId { get; set; }
        public string? ApplicationId { get; set; }
        public string BrandId { get; set; }

        [XmlElement(ElementName = "ErrorMessage", Namespace = "")]
        public string? ErrorMessage { get; set; }

        [XmlElement(ElementName = "Decision", Namespace = "")]
        public string Decision { get; set; }

        [XmlIgnore]
        public string? LesResponse { get; set; }

        [XmlIgnore]
        public string? DbsResponse { get; set; }

        [XmlIgnore]
        public string? TurboResponse { get; set; }

        [XmlIgnore]
        public string UMID { get; set; }

        [XmlIgnore]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    }
}
