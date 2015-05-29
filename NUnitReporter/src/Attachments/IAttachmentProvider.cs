using System;
using System.Collections.Generic;

namespace NUnitReporter.Attachments
{
    public interface IAttachmentProvider
    {
        String AttachmentElementName { get; }

        String AttachmentListElementName { get; }

        IEnumerable<String> GetTestCaseAttachments(String testCaseId);

        Boolean HasAttachments { get; }
    }
}