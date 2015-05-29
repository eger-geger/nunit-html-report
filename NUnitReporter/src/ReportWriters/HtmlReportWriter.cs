using System;
using System.IO;
using System.Reflection;
using System.Xml;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace NUnitReporter.ReportWriters
{
    public class HtmlReportWriter : AbstractReportWriter
    {
        private const String TemplateResourceName = "templates.default.html";

        public HtmlReportWriter(string fileName) : base(fileName)
        {
        }

        private Stream ReportTemplateStream
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();

                return assembly.GetManifestResourceStream(
                    String.Format("{0}.{1}", assembly.GetName().Name, TemplateResourceName)
                    );
            }
        }

        protected override void Write(XmlDocument document, TextWriter writer)
        {
            using (Stream stream = ReportTemplateStream)
            {
                if (stream == null)
                {
                    throw new InvalidOperationException("Failed to load report template");
                }

                var template = new HtmlDocument();

                template.Load(stream);

                InsertTestResult(template, JsonConvert.SerializeXmlNode(document, Formatting.Indented));

                template.Save(writer);
            }
        }

        private static void InsertTestResult(HtmlDocument template, String testResultJson)
        {
            var body = template.DocumentNode.SelectSingleNode("//body");

            if (body == null)
            {
                throw new InvalidOperationException("Document did not contain body");
            }

            var script = template.CreateElement("script");
            script.SetAttributeValue("type", "text/javascript");
            script.AppendChild(template.CreateTextNode(
                String.Format("var NUnitTestResult = {0};", testResultJson)
            ));

            body.PrependChild(script);
        }
    }
}