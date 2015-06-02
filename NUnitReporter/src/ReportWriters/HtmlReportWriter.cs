using System;
using System.Collections.Generic;
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
        private const String TestResultHtml = "TestResult.html";

        private const String TemplateResoucePath = "templates.default";

        private const String TemplateIndexFileName = "index.html";

        private static readonly IDictionary<string, string> TemplateResources = new Dictionary<string, string>
        {
            {
                "bootstrap.dist.css.bootstrap-theme.min.css",
                @"bootstrap\dist\css\bootstrap-theme.min.css"
            },
            {
                "bootstrap.dist.fonts.glyphicons-halflings-regular.eot",
                @"bootstrap\dist\fonts\glyphicons-halflings-regular.eot"
            },
            {
                "bootstrap.dist.fonts.glyphicons-halflings-regular.svg",
                @"bootstrap\dist\fonts\glyphicons-halflings-regular.svg"
            },
            {
                "bootstrap.dist.fonts.glyphicons-halflings-regular.ttf",
                @"bootstrap\dist\fonts\glyphicons-halflings-regular.ttf"
            },
            {
                "bootstrap.dist.fonts.glyphicons-halflings-regular.woff",
                @"bootstrap\dist\fonts\glyphicons-halflings-regular.woff"
            },
            {
                "bootstrap.dist.fonts.glyphicons-halflings-regular.woff2",
                @"bootstrap\dist\fonts\glyphicons-halflings-regular.woff2"
            },
            {
                "bootstrap.dist.css.bootstrap.min.css",
                @"bootstrap\dist\css\bootstrap.min.css"
            }
        };

        private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();

        private static String GetTemplateResourceName(String relativePath)
        {
            return String.Format("{0}.{1}.{2}", Assembly.GetName().Name, TemplateResoucePath, relativePath);
        }

        protected override void Write(XmlDocument document, String outputFolderPath)
        {
            CopyMainFileTo(outputFolderPath, JsonConvert.SerializeXmlNode(document, Formatting.Indented));

            foreach (var pair in TemplateResources)
            {
                CopyResourceTo(GetTemplateResourceName(pair.Key), Path.Combine(outputFolderPath, pair.Value));
            }
        }

        private void CopyResourceTo(String resourceName, String filePath)
        {
            string targetFolderPath = Path.GetDirectoryName(filePath);

            if (String.IsNullOrEmpty(targetFolderPath))
            {
                throw new ArgumentException(String.Format("Wrong path: {0}", filePath));
            }

            Directory.CreateDirectory(targetFolderPath);

            using (Stream source = Assembly.GetManifestResourceStream(resourceName))
            {
                if (source == null)
                {
                    throw new ArgumentException(String.Format("Wrong resource name: {0}", resourceName));
                }

                using (var target = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    source.CopyTo(target);
                }
            }
        }

        private static void CopyMainFileTo(String outputFolderPath, String testResultJson)
        {
            using (Stream stream = Assembly.GetManifestResourceStream(GetTemplateResourceName(TemplateIndexFileName)))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException("Failed to load report template");
                }

                var document = new HtmlDocument();

                document.Load(stream);

                HtmlNode body = document.DocumentNode.SelectSingleNode("//body");

                if (body == null)
                {
                    throw new InvalidOperationException("Document did not contain body");
                }

                HtmlNode script = document.CreateElement("script");
                script.SetAttributeValue("type", "text/javascript");
                script.AppendChild(document.CreateTextNode(String.Format("var NUnitTestResult = {0};", testResultJson)));
                body.PrependChild(script);

                document.Save(Path.Combine(outputFolderPath, TestResultHtml));
            }
        }
    }
}