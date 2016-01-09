using System;
using System.Collections.Generic;

namespace NUnitReporter.ActionReport
{
    internal interface IAction
    {
        Guid Guid { get; }
        string Description { get; }
        IEnumerable<Object> Arguments { get; }
        IAction Parent { get; }
        IEnumerable<IAction> Nested { get; }
        void AddNested(IAction action);
    }
}