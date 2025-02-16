using System.Xml.Serialization;

namespace LeadManagementSystem.Contracts.Request
{
    [XmlRoot(ElementName = "TurboApplicationContractRequest")]
    public class TurboApplicationContractRequest
    {
        [XmlElement(ElementName = "turboInput", Namespace = "urn:DA-InternetLink-Schema")]
        public TurboInput TurboInput { get; set; }
    }

    public class TurboInput
    {
        [XmlElement(ElementName = "ProductId")]
        public int ProductId { get; set; }

        [XmlElement(ElementName = "PromotionalCode")]
        public string PromotionalCode { get; set; }

        [XmlElement(ElementName = "ApplicationType")]
        public string ApplicationType { get; set; }

        [XmlElement(ElementName = "ApplicationSource")]
        public string ApplicationSource { get; set; }

        [XmlElement(ElementName = "OMIUniqueId")]
        public string OMIUniqueId { get; set; }

        [XmlElement(ElementName = "OMISessionID")]
        public string OMISessionID { get; set; }

        [XmlElement(ElementName = "OMISubmissionID")]
        public string OMISubmissionID { get; set; }

        [XmlElement(ElementName = "LeadId")]
        public string LeadId { get; set; }

        [XmlElement(ElementName = "LeadEvaluationMessageId")]
        public string LeadEvaluationMessageId { get; set; }

        [XmlElement(ElementName = "ApplicationReferrerLookupID")]
        public string ApplicationReferrerLookupID { get; set; }

        [XmlElement(ElementName = "PersonalInfo")]
        public PersonalInfo PersonalInfo { get; set; }
    }

    public class PersonalInfo
    {
        [XmlElement(ElementName = "Person")]
        public Person Person { get; set; }

        [XmlElement(ElementName = "HomeTelephoneNumber")]
        public TelephoneNumber? HomeTelephoneNumber { get; set; }

        [XmlElement(ElementName = "WorkTelephoneNumber")]
        public TelephoneNumber? WorkTelephoneNumber { get; set; }

        [XmlElement(ElementName = "CellNumber")]
        public TelephoneNumber CellNumber { get; set; }

        [XmlElement(ElementName = "EmailAddress")]
        public string? EmailAddress { get; set; }

        [XmlElement(ElementName = "PermissionToPromote")]
        public string PermissionToPromote { get; set; }

        [XmlElement(ElementName = "GrossIncome")]
        public double GrossIncome { get; set; }

        [XmlElement(ElementName = "PreagreementStatement")]
        public string PreagreementStatement { get; set; }

        [XmlElement(ElementName = "AllowsCreditCheck")]
        public string AllowsCreditCheck { get; set; }

        [XmlElement(ElementName = "IsInFinancialTrouble")]
        public string IsInFinancialTrouble { get; set; }

        [XmlElement(ElementName = "OtherLoans")]
        public string OtherLoans { get; set; }
    }

    public class Person
    {
        [XmlElement(ElementName = "PersonName")]
        public string PersonName { get; set; }

        [XmlElement(ElementName = "PersonSurname")]
        public string PersonSurname { get; set; }

        [XmlElement(ElementName = "PersonIDNumber")]
        public string PersonIDNumber { get; set; }
    }

    public class TelephoneNumber
    {
        [XmlElement(ElementName = "Code")]
        public string Code { get; set; }

        [XmlElement(ElementName = "Number")]
        public string Number { get; set; }
    }
}
