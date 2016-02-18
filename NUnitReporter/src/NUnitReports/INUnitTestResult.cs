using System.Xml;
using NUnitReporter.Attachments;
using NUnitReporter.EventReport;

namespace NUnitReporter.NUnitReports
{
    public interface INUnitTestResult
    {
        XmlDocument XmlDocument { get; }

        void AddAttachments(IAttachmentProvider attachmentProvider);

        void AddEventLog(IEventStorage eventStorage);
    }
}