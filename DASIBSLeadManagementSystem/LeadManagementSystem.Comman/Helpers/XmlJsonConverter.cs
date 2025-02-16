using LeadManagementSystem.Contracts.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LeadManagementSystem.Comman.Helpers
{
    public static class XmlJsonConverter
    {
        public static string ConvertJsonToXml(string json)
        {
            var jsonObj = JsonConvert.DeserializeObject<JObject>(json);
            XDocument xmlDocument = new(new XElement("Root"));
            ConvertJsonToXml(jsonObj, xmlDocument.Root);
            return xmlDocument.ToString();
        }
        public static ContentResult SerializeToXmlContentResult<T>(T actionResult, ControllerBase controller)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using var stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, actionResult);
            return controller.Content(stringWriter.ToString(), "application/xml");
        }

        public static ContentResult SerializeToXmlContentResult(LeadResponseEnvelope leadResponseEnvelope, ControllerBase controller)
        {
            var xmlSerializer = new XmlSerializer(typeof(LeadResponseEnvelope));
            using var stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, leadResponseEnvelope);
            return controller.Content(stringWriter.ToString(), "application/xml");
        }
        public static string SerializeToXml<T>(T request)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            var stringWriterEncoding = new StringWriterWithEncoding(Encoding.UTF8);
            var xmlWriterSettings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = false, Encoding = Encoding.UTF8 };

            using (var xmlWriter = XmlWriter.Create(stringWriterEncoding, xmlWriterSettings))
            {
                xmlSerializer.Serialize(xmlWriter, request);
            }

            return stringWriterEncoding.ToString();
        }

        private static void ConvertJsonToXml(JToken json, XElement parentElement)
        {
            if (json is JObject obj)
            {
                foreach (var property in obj.Properties())
                {
                    var element = new XElement(property.Name);
                    parentElement.Add(element);
                    ConvertJsonToXml(property.Value, element);
                }
            }
            else if (json is JArray array)
            {
                foreach (var item in array)
                {
                    var element = new XElement("Item");
                    parentElement.Add(element);
                    ConvertJsonToXml(item, element);
                }
            }
            else
            {
                parentElement.Value = json.ToString();
            }
        }

      

        public class StringWriterWithEncoding : StringWriter
        {
            public StringWriterWithEncoding(Encoding encoding) : base()
            {
                this.Encoding = encoding;
            }

            public override Encoding Encoding { get; }
        }
    }
}
