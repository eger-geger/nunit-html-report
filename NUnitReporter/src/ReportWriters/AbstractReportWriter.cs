using System;
using System.IO;
using System.Xml;
using NUnitReporter.NUnitReports;

namespace NUnitReporter.ReportWriters
{
    public abstract class AbstractReportWriter
    {
        private readonly String _fileName;

        protected AbstractReportWriter(String fileName)
        {
            _fileName = fileName;
        }

        public void Write(INUnitTestResult testResult, String outputFolderPath)
        {
            if (testResult == null)
            {
                throw new ArgumentNullException("testResult");
            }

            if (String.IsNullOrEmpty(outputFolderPath))
            {
                throw new ArgumentNullException("outputFolderPath");
            }

            if (!Directory.Exists(outputFolderPath))
            {
                Directory.CreateDirectory(outputFolderPath);
            }

            using (var writer = new StreamWriter(Path.Combine(outputFolderPath, _fileName)))
            {
                Write(testResult.XmlDocument, writer);    
            }
        }

        protected abstract void Write(XmlDocument document, TextWriter writer);

    }
}
