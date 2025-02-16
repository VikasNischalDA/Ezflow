using System.Xml.Serialization;

namespace LeadManagementSystem.Contracts.Response
{
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class DBSResponse
    {
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public SoapBody DbsResponseBody { get; set; }

    }

    public class SoapBody
    {
        [XmlElement(ElementName = "Fault", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public SoapFault Fault { get; set; }


        [XmlElement(ElementName = "RequestRiskGradeResponse", Namespace = "http://www.example.com/")]
        public RequestRiskGradeResponse RequestRiskGradeResponse { get; set; }

    }

    [XmlRoot(ElementName = "RequestRiskGradeResponse", Namespace = "http://www.example.com/")]
    public class RequestRiskGradeResponse
    {
        [XmlElement(ElementName = "ReturnRiskGrade", Namespace = "")]
        public ReturnRiskGrade ReturnRiskGrade { get; set; }
    }

    public class SoapFault
    {
        [XmlElement(ElementName = "faultcode", Namespace = "")]
        public string FaultCode { get; set; }

        [XmlElement(ElementName = "faultstring", Namespace = "")]
        public string FaultString { get; set; }
    }

    public class ReturnRiskGrade
    {
        [XmlElement(ElementName = "IDNumber", Namespace = "")]
        public string IDNumber { get; set; }

        [XmlElement(ElementName = "BrandID", Namespace = "")]
        public long BrandID { get; set; }

        [XmlElement(ElementName = "RiskCellOrProfile", Namespace = "")]
        public string RiskCellOrProfile { get; set; }

        [XmlElement(ElementName = "ApplicationFound", Namespace = "")]
        public bool ApplicationFound { get; set; }
    }
}
