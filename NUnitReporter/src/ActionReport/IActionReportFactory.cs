namespace NUnitReporter.ActionReport
{
    /// <summary>
    /// Cretes instances of <see cref="IActionReport"/>
    /// </summary>
    public interface IActionReportFactory
    {

        /// <summary>
        /// Get action report for current test
        /// </summary>
        /// <returns>Action report or null when report cannot be created</returns>
        IActionReport CurrentTestReport { get; }
    }
}