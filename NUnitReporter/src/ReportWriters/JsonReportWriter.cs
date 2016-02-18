using System.IO;
using System.Xml;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace NUnitReporter.ReportWriters
{
    public class JsonReportWriter : AbstractReportWriter
    {
        public JsonReportWriter(string outputFilePath) : base(outputFilePath)
        {
        }

        protected override void Write(XmlDocument document)
        {
            File.WriteAllText(OutputFilePath, JsonConvert.SerializeXmlNode(document, Formatting.Indented));
        }
    }
}