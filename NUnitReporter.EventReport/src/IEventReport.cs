using System;

namespace NUnitReporter.EventReport
{
    /// <summary>
    ///     Provides methods for adding additional information into report in a way compatible with report generator
    /// </summary>
    public interface IEventReport
    {
        /// <summary>
        ///     Record atomic activity which will not considered as "parent" for following activities
        /// </summary>
        /// <param name="description">Activity description</param>
        /// <param name="args">Activity details (input arguments)</param>
        void RecordEvent(string description, params object[] args);

        /// <summary>
        ///     Record atomic activity which will not considered as "parent" for following activities
        /// </summary>
        /// <param name="event">Activity to be recorded</param>
        void RecordEvent(IActivity @event);

        /// <summary>
        ///     Record starting point of the complex activity. 
        ///     All activities reported after current method call will be considered it's "children" 
        ///     until activity endpoint will be recorder with <see cref="RecordActivityFinished"/>
        /// </summary>
        /// <param name="description">Activity description</param>
        /// <param name="args">Activity details (input arguments)</param>
        /// <returns>Activity identifier which should be used with <see cref="RecordActivityFinished"/></returns>
        string RecordActivityStarted(string description, params object[] args);

        /// <summary>
        ///     Record the point when activity identified with descriptor has finished. 
        ///     It will have no effect when wrong activity identifier was given.
        /// </summary>
        /// <param name="descriptor">Activity identifier created by <see cref="RecordActivityStarted"/></param>
        void RecordActivityFinished(string descriptor);

        /// <summary>
        ///     Record error event
        /// </summary>
        /// <param name="exception">Error to record</param>
        void RecordError(Exception exception);
    }
}