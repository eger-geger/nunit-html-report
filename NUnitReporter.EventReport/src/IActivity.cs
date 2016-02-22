using System;
using System.Collections.Generic;

namespace NUnitReporter.EventReport
{
    public interface IActivity
    {
        Guid Guid { get; }

        IActivity Parent { get; }

        IEnumerable<IActivity> Nested { get; }

        void AddNested(IActivity activity);

        void FinalizeActivity();
    }
}