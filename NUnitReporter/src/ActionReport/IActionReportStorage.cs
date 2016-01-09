namespace NUnitReporter.ActionReport
{
    /// <summary>
    ///     Abstraction over serializing and retrieving action reports
    /// </summary>
    public interface IActionReportStorage
    {
        /// <summary>
        ///     Save action report for later usage
        /// </summary>
        /// <param name="id">NUnit test unique identifier</param>
        /// <param name="report">Instance of action report</param>
        void Save(string id, IActionReport report);

        /// <summary>
        ///     Get previously stored action report
        /// </summary>
        /// <param name="id">NUnit test unique identifier</param>
        /// <returns>Instance of action report associated with provided identifier</returns>
        IActionReport Get(string id);

        /// <summary>
        ///     Check if report associated with provided identifier exist
        /// </summary>
        /// <param name="id">NUnit test unique identifier</param>
        /// <returns>
        ///     <value>TRUE</value>
        ///     if test report exist, otherwise -
        ///     <value>FALSE</value>
        /// </returns>
        bool Exist(string id);
    }
}