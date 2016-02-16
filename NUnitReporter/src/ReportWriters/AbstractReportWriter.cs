using System;
using System.IO;
using System.Xml;
using NUnitReporter.NUnitReports;

namespace NUnitReporter.ReportWriters
{
    public abstract class AbstractReportWriter
    {
        private readonly String _outputFilePath;

        private readonly String _outputFolderPath;

        protected AbstractReportWriter(string outputFilePath)
        {
            _outputFilePath = Path.GetFullPath(outputFilePath);
            _outputFolderPath = Path.GetDirectoryName(_outputFilePath);
        }

        public void Write(INUnitTestResult testResult)
        {
            if (testResult == null)
            {
                throw new ArgumentNullException("testResult");
            }

            Directory.CreateDirectory(OutputFolderPath);

            Write(testResult.XmlDocument);
        }

        public String OutputFilePath
        {
            get { return _outputFilePath; }
        }

        public String OutputFolderPath
        {
            get { return _outputFolderPath; }
        }

        protected abstract void Write(XmlDocument document);
    }
}