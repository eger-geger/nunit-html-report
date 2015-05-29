using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NUnitReporter.Attachments;

namespace NUnitReporter.NUnitReports
{
    public class NUnitTestResult : INUnitTestResult
    {
        private const String TestCaseElementName = "test-case";
        private const String TestCaseIdAttribute = "id";

        private readonly XmlDocument _testResultXml;

        public NUnitTestResult(String testResultXml)
        {
            Validate.FilePath(testResultXml, "testResultXml is null or empty");

            _testResultXml = new XmlDocument();
            _testResultXml.Load(testResultXml);
        }

        public NUnitTestResult(XmlDocument testResultXml)
        {
            if (testResultXml == null)
            {
                throw new ArgumentNullException("testResultXml");
            }

            _testResultXml = testResultXml;
        }

        public void Append(IAttachmentProvider attachmentProvider)
        {
            if (attachmentProvider == null)
            {
                throw new ArgumentNullException("attachmentProvider");
            }

            if (!attachmentProvider.HasAttachments)
            {
                return;
            }

            foreach (XmlNode testCaseNode in _testResultXml.GetElementsByTagName(TestCaseElementName))
            {
                if (testCaseNode.Attributes == null)
                {
                    throw new InvalidOperationException("TestCase XML node does not have attributes");
                }

                XmlAttribute idAttribute = testCaseNode.Attributes[TestCaseIdAttribute];

                if (idAttribute == null)
                {
                    throw new InvalidOperationException("TestCase does not have ID attribute");
                }

                List<string> attachments = attachmentProvider
                    .GetTestCaseAttachments(idAttribute.Value)
                    .ToList();

                if (!attachments.Any())
                {
                    continue;
                }

                XmlElement attachmentListElement = _testResultXml.CreateElement(
                    attachmentProvider.AttachmentListElementName);

                foreach (string attachment in attachments)
                {
                    XmlElement filePathElement = _testResultXml.CreateElement(
                        attachmentProvider.AttachmentElementName);

                    filePathElement.InnerText = attachment;

                    attachmentListElement.AppendChild(filePathElement);
                }

                testCaseNode.AppendChild(attachmentListElement);
            }
        }

        public XmlDocument XmlDocument
        {
            get { return _testResultXml.CloneNode(true) as XmlDocument; }
        }
    }
}