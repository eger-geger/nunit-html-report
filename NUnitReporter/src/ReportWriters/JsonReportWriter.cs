using System.IO;
using System.Xml;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace NUnitReporter.ReportWriters
{
    public class JsonReportWriter : AbstractReportWriter
    {
        public JsonReportWriter(string fileName) : base(fileName)
        {
        }

        protected override void Write(XmlDocument document, TextWriter writer)
        {
            writer.Write(JsonConvert.SerializeXmlNode(document, Formatting.Indented));
        }
    }
}