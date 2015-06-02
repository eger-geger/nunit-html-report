using System;
using System.IO;
using System.Xml;
using NUnitReporter.NUnitReports;

namespace NUnitReporter.ReportWriters
{
    public abstract class AbstractReportWriter
    {
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

            Directory.CreateDirectory(outputFolderPath);

            Write(testResult.XmlDocument, outputFolderPath);
        }

        protected abstract void Write(XmlDocument document, String outputFolderPath);
    }
}