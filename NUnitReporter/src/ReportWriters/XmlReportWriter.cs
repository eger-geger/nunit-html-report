using System.IO;
using System.Xml;

namespace NUnitReporter.ReportWriters
{
    public class XmlReportWriter : AbstractReportWriter
    {
        public XmlReportWriter(string outputFolderPath) : base(outputFolderPath)
        {
        }

        protected override void Write(XmlDocument document, TextWriter writer)
        {
            document.Save(writer);
        }
    }
}
