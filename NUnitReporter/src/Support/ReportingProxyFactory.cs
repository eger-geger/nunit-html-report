using System;
using Castle.DynamicProxy;
using NUnitReporter.ActionReport;

namespace NUnitReporter.Support
{
    public class ReportingProxyFactory
    {
        private readonly ProxyGenerator _generator;
        private readonly IActionReportFactory _reportFactory;

        public ReportingProxyFactory(IActionReportFactory reportFactory)
        {
            if (reportFactory == null)
            {
                throw new ArgumentNullException(nameof(reportFactory));
            }

            _generator = new ProxyGenerator();
            _reportFactory = reportFactory;
        }

        public TTarget CreateProxy<TTarget>(TTarget target) where TTarget : class
        {
            return _generator.CreateClassProxyWithTarget(target, new ReportingInerceptor(_reportFactory));
        }
    }
}