using System;
using System.IO;
using System.Xml;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace NUnitReporter.ReportWriters
{
    public class JsonReportWriter : AbstractReportWriter
    {
        private const String TestResultJson = "TestResult.json";

        protected override void Write(XmlDocument document, String outputFolderPath)
        {
            File.WriteAllText(
                Path.Combine(outputFolderPath, TestResultJson),
                JsonConvert.SerializeXmlNode(document, Formatting.Indented));
        }
    }
}