using System;
using Castle.DynamicProxy;

namespace NUnitReporter.EventReport.ProxyFactory
{
    internal class ReportingInerceptor : IInterceptor
    {
        private readonly IEventReportFactory _reportFactory;

        public ReportingInerceptor(IEventReportFactory reportFactory)
        {
            _reportFactory = reportFactory;
        }

        public void Intercept(IInvocation invocation)
        {
            var report = _reportFactory.CurrentTestReport;

            EventReportAttribute attribute = GetEventReportAttribute(invocation);

            if (report != null && IsAligibleForReporting(invocation, attribute))
            {
                ReportAndProceed(invocation, report);
            }
            else
            {
                invocation.Proceed();
            }
        }

        private static void ReportAndProceed(IInvocation invocation, IEventReport report)
        {
            String actionGuid = report.RecordActivityStarted(FormatInvocationName(invocation), invocation.Arguments);

            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                report.RecordError(ex);
                throw;
            }
            finally
            {
                if (!String.IsNullOrEmpty(actionGuid))
                {
                    report.RecordActivityFinished(actionGuid);
                }
            }
        }

        private static EventReportAttribute GetEventReportAttribute(IInvocation invocation)
        {
            var eventReportAttributes = invocation.Method.GetCustomAttributes(typeof(EventReportAttribute), true);

            if (eventReportAttributes.Length == 0)
            {
                return null;
            }

            return (EventReportAttribute) eventReportAttributes[0];
        }

        private static String FormatInvocationName(IInvocation invocation)
        {
            if (IsPropertySetter(invocation))
            {
                return String.Format("{0}::{1}", invocation.TargetType.Name, invocation.Method.Name.Replace("set_", "Set"));
            }

            if (IsPropertyGetter(invocation))
            {
                return String.Format("{0}::{1}", invocation.TargetType.Name, invocation.Method.Name.Replace("get_", "Get"));
            }

            return String.Format("{0}::{1}", invocation.TargetType.Name, invocation.Method.Name);
        }

        private static Boolean IsAligibleForReporting(IInvocation invocation, EventReportAttribute attribute)
        {
            if (attribute != null && attribute.Ignore)
            {
                return false;
            }

            if (invocation.Method.IsSpecialName)
            {
                return IsPropertySetter(invocation);
            }

            if (invocation.Method.GetBaseDefinition().DeclaringType == typeof (Object))
            {
                return false;
            }

            return true;
        }

        private static Boolean IsPropertyGetter(IInvocation invocation)
        {
            if (!invocation.Method.IsSpecialName)
            {
                return false;
            }

            return invocation.Method.Name.StartsWith("get_");
        }

        private static Boolean IsPropertySetter(IInvocation invocation)
        {
            if (!invocation.Method.IsSpecialName)
            {
                return false;
            }

            return invocation.Method.Name.StartsWith("set_");
        }
    }
}