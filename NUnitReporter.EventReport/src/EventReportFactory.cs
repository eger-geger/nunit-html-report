using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NUnit.Framework;

namespace NUnitReporter.EventReport
{
    /// <summary>
    ///     Creates instances of <see cref="IEventReport" />.
    /// </summary>
    public class EventReportFactory : IEventReportFactory
    {
        private readonly IDictionary<string, IEventReport> _reports;

        public EventReportFactory()
        {
            _reports = new ConcurrentDictionary<string, IEventReport>();
        }

        private static string CurrentTestId
        {
            get
            {
                return TestContext.CurrentContext == null || TestContext.CurrentContext.Test == null
                    ? string.Empty
                    : TestContext.CurrentContext.Test.ID;
            }
        }

        /// <summary>
        ///     Get action report for current test.
        ///     It will return same instance until <see cref="TestContext.CurrentContext" /> has not changed.
        /// </summary>
        /// <returns>
        ///     Report associated with current <see cref="TestContext" /> or null when <see cref="TestContext.CurrentContext" /> is
        ///     null
        /// </returns>
        public IEventReport CurrentTestReport
        {
            get
            {
                var currentTestId = CurrentTestId;

                if (String.IsNullOrEmpty(CurrentTestId))
                {
                    return null;
                }

                if (!_reports.ContainsKey(currentTestId))
                {
                    _reports.Add(currentTestId, new DefaultEventReport());
                }

                return _reports[currentTestId];
            }
        }
    }
}