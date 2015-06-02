using System;
using System.Collections.Generic;
using CommandLine;
using NUnitReporter.Attachments;
using NUnitReporter.NUnitReports;
using NUnitReporter.ReportWriters;

namespace NUnitReporter
{
    public class Program
    {
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

                foreach (AbstractReportWriter writer in new List<AbstractReportWriter>
                {
                    options.WriteJson ? new JsonReportWriter() : null,
                    options.WriteXml ? new XmlReportWriter() : null,
                    options.WriteHtml ? new HtmlReportWriter() : null
                })
                {
                    if (writer != null)
                    {
                        writer.Write(nunitReport, options.OutputFolderPath);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(options.GetUsage());
            }
        }
    }
}