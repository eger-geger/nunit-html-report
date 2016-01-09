using System;

namespace NUnitReporter.ActionReport
{
    /// <summary>
    ///     Provides methods for adding additional information into report in way compatible with report generator
    /// </summary>
    public interface IActionReport
    {
        /// <summary>
        ///     Notify report about simple action without nested actions.
        /// </summary>
        /// <param name="description">Action description</param>
        /// <param name="args">Action details (input arguments)</param>
        void ActionTaken(string description, params object[] args);

        /// <summary>
        ///     Notify report that complex action have started. It may contain nested action.
        ///     Action considered nested if it occurred after current action started and before it was finished.
        /// </summary>
        /// <param name="description">Action description</param>
        /// <param name="args">Action details (input arguments)</param>
        /// <returns>Action identifier which should be used with <see cref="ActionFinished" /></returns>
        string ActionStarted(string description, params object[] args);

        /// <summary>
        ///     Notify report that complex action have finished which means that actions occurred later will not be considered it's
        ///     children.
        /// </summary>
        /// <param name="descriptor">Returned by <see cref="ActionStarted" /></param>
        void ActionFinished(string descriptor);

        /// <summary>
        ///     Notify report that screen-shot was taken
        /// </summary>
        /// <param name="filePath">Absolute screen-shot file path</param>
        void ImageTaken(string filePath);

        /// <summary>
        ///     Notify report about error has been thrown
        /// </summary>
        /// <param name="exception">Error that was thrown</param>
        void ErrorThrown(Exception exception);
    }
}