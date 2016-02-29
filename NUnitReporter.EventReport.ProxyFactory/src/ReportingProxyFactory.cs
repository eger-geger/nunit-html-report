using System;
using Castle.DynamicProxy;

namespace NUnitReporter.EventReport.ProxyFactory
{
    /// <summary>
    ///     Creates proxies which are recording events to <see cref="IEventReport"/>
    /// </summary>
    public class ReportingProxyFactory
    {
        private readonly ProxyGenerator _generator;

        private readonly IEventReportFactory _reportFactory;

        /// <summary>
        ///     Initializes new instance of proxy factory with given report factory
        /// </summary>
        /// <param name="reportFactory">Factory to be used by proxies for creating reports</param>
        public ReportingProxyFactory(IEventReportFactory reportFactory)
        {
            if (reportFactory == null)
            {
                throw new ArgumentNullException(nameof(reportFactory));
            }

            _generator = new ProxyGenerator();
            _reportFactory = reportFactory;
        }

        /// <summary>
        ///     Creates new proxy instance using provided class and class instance
        /// </summary>
        /// <typeparam name="TTarget">Class of the proxy instance</typeparam>
        /// <param name="constructorArguments">Constructor arguments used by target class constructor</param>
        /// <returns>Proxy instance which records every method call to event report</returns>
        public TTarget Create<TTarget>(params Object[] constructorArguments) where TTarget : class
        {
            return (TTarget) Create(typeof(TTarget), constructorArguments);
        }

        /// <summary>
        ///     Creates new proxy instance using provided class and class instance
        /// </summary>
        /// <param name="classToProxy">Class of the proxy instance</param>
        /// <param name="constructorArguments">Constructor arguments used by target class constructor</param>
        /// <returns>Proxy instance which records every method call to event report</returns>
        public object Create(Type classToProxy, params Object[] constructorArguments)
        {
            return _generator.CreateClassProxy(classToProxy, constructorArguments, new ReportingInerceptor(_reportFactory));
        }
    }
}