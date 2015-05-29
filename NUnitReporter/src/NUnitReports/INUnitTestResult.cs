using System.Xml;
using NUnitReporter.Attachments;

namespace NUnitReporter.NUnitReports
{
    public interface INUnitTestResult
    {
        XmlDocument XmlDocument { get; }
        void Append(IAttachmentProvider attachmentProvider);
    }
}