using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NUnit.Framework;

namespace NUnitReporter.ActionReport
{
    /// <summary>
    /// Cretes instances of <see cref="IActionReport"/>.
    /// </summary>
    public class DefaultActionReportFactory : IActionReportFactory
    {
        private readonly IDictionary<String, IActionReport> _reports;

        public DefaultActionReportFactory()
        {
            _reports = new ConcurrentDictionary<String, IActionReport>();
        }

        /// <summary>
        /// Get action report for current test. 
        /// It will return same instance until <see cref="TestContext.CurrentContext"/> has not changed.
        /// </summary>
        /// <returns>
        /// Report associated with current <see cref="TestContext"/> or null when <see cref="TestContext.CurrentContext"/> is null
        /// </returns>
        public IActionReport CurrentTestReport
        {
            get
            {
                if (TestContext.CurrentContext == null || TestContext.CurrentContext.Test == null)
                {
                    return null;
                }

                var currentTestId = TestContext.CurrentContext.Test.ID;

                if (!_reports.ContainsKey(currentTestId))
                {
                    _reports.Add(currentTestId, new DefaultActionReport());
                }

                return _reports[currentTestId];
            }
        }
    }
}
