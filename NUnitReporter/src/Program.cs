using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using NUnitReporter.Attachments;
using NUnitReporter.EventReport;
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
                Validate.FileExist(options.TestResultXmlPath, "Test Result Xml Path");

                var nunitReport = new NUnitTestResult(options.TestResultXmlPath);

                if (!String.IsNullOrEmpty(options.AttachmentFolderPath))
                {
                    nunitReport.AddAttachments(new ImageAttachmentProvider(
                        options.AttachmentFolderPath,
                        options.OutputFolderPath));

                    nunitReport.AddEventLog(new DiskStorage(options.AttachmentFolderPath));
                }

                foreach (AbstractReportWriter writer in GetSelectedWriters(options))
                {
                    try
                    {
                        writer.Write(nunitReport);
                        Console.WriteLine("Report was written to {0}", writer.OutputFilePath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Failed to write report to {0}", writer.OutputFilePath);
                        Console.Error.WriteLine(ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.WriteLine(options.GetUsage());
            }
        }

        private static IEnumerable<AbstractReportWriter> GetSelectedWriters(CommandLineOptions options)
        {
            if (options.WriteJson)
            {
                yield return new JsonReportWriter(Path.Combine(options.OutputFolderPath, "TestResult.json"));
            }

            if (options.WriteHtml)
            {
                yield return new HtmlReportWriter(Path.Combine(options.OutputFolderPath, "TestResult.html"));
            }

            if (options.WriteXml)
            {
                yield return new XmlReportWriter(Path.Combine(options.OutputFolderPath, "TestResult.xml"));
            }
        }
    }
}