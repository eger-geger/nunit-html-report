using System;
using Castle.DynamicProxy;
using NUnitReporter.ActionReport;

namespace NUnitReporter.Support
{
    public class ReportingInerceptor : IInterceptor
    {
        private readonly IActionReportFactory _reportFactory;

        public ReportingInerceptor(IActionReportFactory reportFactory)
        {
            _reportFactory = reportFactory;
        }

        public void Intercept(IInvocation invocation)
        {
            var report = _reportFactory.CurrentTestReport;

            if (report == null)
            {
                invocation.Proceed();
            }
            else
            {
                ProceedAndReport(invocation, report);
            }
        }

        private static void ProceedAndReport(IInvocation invocation, IActionReport report)
        {
            String actionGuid = report.ActionStarted(invocation.Method.Name, invocation.Arguments);

            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                report.ErrorThrown(ex);
                throw;
            }
            finally
            {
                if (!String.IsNullOrEmpty(actionGuid))
                {
                    report.ActionFinished(actionGuid);
                }
            }
        }
    }
}