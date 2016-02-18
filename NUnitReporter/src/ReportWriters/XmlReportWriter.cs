using System.Xml;

namespace NUnitReporter.ReportWriters
{
    public class XmlReportWriter : AbstractReportWriter
    {
        public XmlReportWriter(string outputFilePath) : base(outputFilePath)
        {
        }

        protected override void Write(XmlDocument document)
        {
            document.Save(OutputFilePath);
        }
    }
}