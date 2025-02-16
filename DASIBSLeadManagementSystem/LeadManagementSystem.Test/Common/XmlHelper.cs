
using System.Xml.Serialization;

namespace LeadManagementSystem.Test.Common
{
    public static class XmlHelper
    {
        public static T DeserializeXml<T>(string xmlContent)
        {
            var serializer = new XmlSerializer(typeof(T));
            using var reader = new StringReader(xmlContent);
            return (T)serializer.Deserialize(reader);
        }
    }
}
