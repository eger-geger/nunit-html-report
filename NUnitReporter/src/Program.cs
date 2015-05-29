using System;
using System.IO;
using CommandLine;
using NUnitReporter.Attachments;
using NUnitReporter.NUnitReports;
using NUnitReporter.ReportWriters;

namespace NUnitReporter
{
    public class Program
    {
        private const String TestResultJson = "TestResult.json";
        private const String TestResultXml = "TestResult.xml";
        private const String TestResultHtml = "TestResult.html";

        public static void Main(String[] args)
        {
            var options = new CommandLineOptions();

            if (!Parser.Default.ParseArguments(args, options))
            {
                return;
            }

            try
            {
                Validate.FilePath(options.TestResultXmlPath, "Test Result Xml Path");

                var nunitReport = new NUnitTestResult(options.TestResultXmlPath);

                if (!String.IsNullOrEmpty(options.AttachmentFolderPath))
                {
                    nunitReport.Append(new ImageAttachmentProvider(
                        options.AttachmentFolderPath,
                        options.OutputFolderPath));
                }

                if (options.WriteJson)
                {
                    new JsonReportWriter(TestResultJson).Write(nunitReport, options.OutputFolderPath);
                }

                if (options.WriteXml)
                {
                    new XmlReportWriter(TestResultXml).Write(nunitReport, options.OutputFolderPath);
                }

                if (options.WriteHtml)
                {
                    new HtmlReportWriter(TestResultHtml).Write(nunitReport, options.OutputFolderPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.WriteLine(options.GetUsage());
            }
        }
    }
}