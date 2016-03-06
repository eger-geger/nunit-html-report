using System;
using System.Collections.Generic;

namespace NUnitReporter.EventReport
{
    /// <summary>
    ///     Recorder activity.
    /// </summary>
    public interface IActivity
    {
        /// <summary>
        ///     Activity unique identifier.
        /// </summary>
        Guid Guid { get; }

        /// <summary>
        ///     Parent activity in scope of which current activity was recorded.
        /// </summary>
        IActivity Parent { get; }

        /// <summary>
        ///     Nested activities recorder in scope of current activity.
        /// </summary>
        IEnumerable<IActivity> Nested { get; }

        /// <summary>
        ///     Adds nested activity.
        /// </summary>
        /// <param name="activity">Nested activity.</param>
        void AddNested(IActivity activity);

        /// <summary>
        ///     Marks activity as finished.
        /// </summary>
        void FinalizeActivity();
    }
}