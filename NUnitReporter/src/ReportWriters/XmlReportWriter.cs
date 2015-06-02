using System;
using System.IO;
using System.Xml;

namespace NUnitReporter.ReportWriters
{
    public class XmlReportWriter : AbstractReportWriter
    {
        private const String TestResultXml = "TestResult.xml";

        protected override void Write(XmlDocument document, String outputFolderPath)
        {
            document.Save(Path.Combine(outputFolderPath, TestResultXml));
        }
    }
}