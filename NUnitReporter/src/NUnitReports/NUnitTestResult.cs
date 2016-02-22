using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Newtonsoft.Json;
using NUnitReporter.Attachments;
using NUnitReporter.EventReport;

namespace NUnitReporter.NUnitReports
{
    public class NUnitTestResult : INUnitTestResult
    {
        private const String TestCaseElementName = "test-case";
        private const String TestCaseIdAttribute = "id";

        private readonly XmlDocument _testResultXml;

        private readonly JsonSerializer _jsonSerializer;

        public NUnitTestResult(String testResultXml) : this(ValidateAndLoadTestResultXml(testResultXml))
        {
        }

        public NUnitTestResult(XmlDocument testResultXml)
        {
            if (testResultXml == null)
            {
                throw new ArgumentNullException("testResultXml");
            }

            _testResultXml = testResultXml;

            _jsonSerializer = JsonSerializer.CreateDefault(new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            });
        }

        public void AddAttachments(IAttachmentProvider attachmentProvider)
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
                List<String> attachments = attachmentProvider
                    .GetTestCaseAttachments(GetTestCaseId(testCaseNode))
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

        public void AddEventLog(IEventStorage eventStorage)
        {
            if (eventStorage == null)
            {
                throw new ArgumentNullException("eventStorage");
            }

            foreach (XmlNode testCaseNode in _testResultXml.GetElementsByTagName(TestCaseElementName))
            {
                var testCaseId = GetTestCaseId(testCaseNode);

                if (!eventStorage.Exist(testCaseId))
                {
                    continue;
                }

                XmlElement eventLogElement = _testResultXml.CreateElement("events");

                using (var stringWriter = new StringWriter())
                {
                    _jsonSerializer.Serialize(stringWriter, eventStorage.Get(testCaseId));

                    eventLogElement.AppendChild(
                        _testResultXml.CreateCDataSection(stringWriter.ToString())
                    );
                }

                testCaseNode.AppendChild(eventLogElement);
            }
        }

        public XmlDocument XmlDocument
        {
            get { return _testResultXml.CloneNode(true) as XmlDocument; }
        }

        private static String GetTestCaseId(XmlNode testCaseNode)
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

            return idAttribute.Value;
        }

        private static XmlDocument ValidateAndLoadTestResultXml(String testResultXml)
        {
            Validate.FilePath(testResultXml, "testResultXml is null or empty");

            var document = new XmlDocument();

            document.Load(testResultXml);

            return document;
        }
    }
}