using Microsoft.AspNetCore.Mvc;
using System.Xml.Serialization;

namespace LeadManagementSystem.Shared.Infrastructure
{
    [XmlRoot("ActionResult")]
    public class ActionResult
    {
        public ActionResult() { }

        public ActionResult(ActionResultCode enumActionResult)
        {
            Status = enumActionResult;
            Errors = new List<ValidationError>();
        }

        public ActionResult(ActionResultCode enumActionResult, IList<ValidationError> errors)
        {
            Status = enumActionResult;
            Errors = (List<ValidationError>?)(errors ?? new List<ValidationError>());
        }

        [XmlElement("Status")]
        public ActionResultCode Status { get; set; }

        [XmlArray("Errors")]
        [XmlArrayItem("ValidationError")]
        public List<ValidationError> Errors { get; set; } // Use List<ValidationError> instead of IEnumerable
    }

    [XmlRoot("ActionResult")]
    public class ActionResult<T> : ActionResult
    {
        public ActionResult() : base() { }

        public ActionResult(ActionResultCode enumActionResult, IList<ValidationError> errors) : base(enumActionResult, errors) { }

        public ActionResult(ActionResultCode enumActionResult, IList<ValidationError> errors, T entity) : base(enumActionResult, errors)
        {
            Entity = entity;
        }

        public ActionResult(ActionResultCode enumActionResult, T entity) : base(enumActionResult)
        {
            Entity = entity;
        }

        [XmlElement("Entity")]
        public T Entity { get; set; }
    }
}
