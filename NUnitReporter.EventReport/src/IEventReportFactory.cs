namespace NUnitReporter.EventReport
{
    /// <summary>
    ///     Creates instances of <see cref="IEventReport"/>
    /// </summary>
    public interface IEventReportFactory
    {

        /// <summary>
        ///     Get action report for current test.
        /// </summary>
        /// <returns>
        ///     Action report or null when report cannot be created.
        /// </returns>
        IEventReport CurrentTestReport { get; }
    }
}