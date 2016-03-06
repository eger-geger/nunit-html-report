using System;
using CommandLine;
using CommandLine.Text;

namespace NUnitReporter
{
    public class CommandLineOptions {

        [ValueOption(0)]
        public String TestResultXmlPath { get; set; }

        [Option('a', "attachments", HelpText = "Path to attachments folder")]
        public String AttachmentFolderPath { get; set; }

        [Option('o', "output", DefaultValue = "./", HelpText = "Path to output folder")]
        public String OutputFolderPath { get; set; }

        [Option("json", HelpText = "Write TestResult.json to output folder")]
        public Boolean WriteJson { get; set; }

        [Option("xml", HelpText = "Write TestResult.xml to output folder")]
        public Boolean WriteXml { get; set; }

        [Option("html", HelpText = "Write TestResult.html to output folder")]
        public Boolean WriteHtml { get; set; }

        [HelpOption]
        public String GetUsage()
        {
            return HelpText.AutoBuild(this);
        }

    }
}
