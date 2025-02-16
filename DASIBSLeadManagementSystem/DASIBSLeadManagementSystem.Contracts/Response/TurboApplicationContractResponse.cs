using System.Xml.Serialization;

namespace LeadManagementSystem.Contracts.Response
{
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class TurboApplicationContractResponse
    {
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public TurboResponseSoapBody Body { get; set; }
    }
    public class TurboResponseSoapBody
    {
        [XmlElement(ElementName = "TurboApplicationContractResponse", Namespace = "urn:DA-InternetLink-Service")]
        public TurboApplicationContractResponseBody TurboApplicationContractResponse { get; set; }
    }
    public class TurboApplicationContractResponseBody
    {
        [XmlElement(ElementName = "TurboApplicationContractResult", Namespace = "urn:DA-InternetLink-Schema")]
        public TurboApplicationContractResult TurboApplicationContractResult { get; set; }
    }
    public class TurboApplicationContractResult
    {
        [XmlElement(ElementName = "InternetTurboApplicationId", Namespace = "urn:DA-InternetLink-Schema")]
        public int InternetTurboApplicationId { get; set; }
        
        [XmlElement(ElementName = "Message", Namespace = "urn:DA-InternetLink-Schema")]
        public string Message { get; set; }
        
        [XmlElement(ElementName = "SystemDecision", Namespace = "urn:DA-InternetLink-Schema", IsNullable = true)]
        public string? SystemDecision { get; set; }
        
        [XmlElement(ElementName = "LeadId", Namespace = "urn:DA-InternetLink-Schema")]
        public long LeadId { get; set; }
        
        [XmlElement(ElementName = "ApplicationId", Namespace = "urn:DA-InternetLink-Schema", IsNullable = true)]
        public string? ApplicationId { get; set; }
        
        [XmlElement(ElementName = "BrandApplicationId", Namespace = "urn:DA-InternetLink-Schema", IsNullable = true)]
        public int? BrandApplicationId { get; set; }
        
        [XmlElement(ElementName = "ResultCode", Namespace = "urn:DA-InternetLink-Schema")]
        public string ResultCode { get; set; }
        
        [XmlElement(ElementName = "RiskLevel", Namespace = "urn:DA-InternetLink-Schema", IsNullable = true)]
        public int? RiskLevel { get; set; }
    }
}