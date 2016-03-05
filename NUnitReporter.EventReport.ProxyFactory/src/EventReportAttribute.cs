using System;

namespace NUnitReporter.EventReport.ProxyFactory
{
    /// <summary>
    ///     When placed on method or property it changes the way events are being created.
    ///     Used in conjunction with <see cref="ReportingProxyFactory"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public class EventReportAttribute : Attribute
    {

        /// <summary>
        ///     Flag indicating that event should not be reported when method is being executed
        /// </summary>
        public Boolean Ignore { get; set; }

    }
}
