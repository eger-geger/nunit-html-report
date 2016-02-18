using System;
using Castle.DynamicProxy;

namespace NUnitReporter.EventReport
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
        /// <param name="target">Object to proxy</param>
        /// <returns>Proxy instance which records every method call to event report</returns>
        public TTarget CreateClassProxy<TTarget>(TTarget target) where TTarget : class
        {
            return _generator.CreateClassProxyWithTarget(target, new ReportingInerceptor(_reportFactory));
        }

        /// <summary>
        ///     Creates new proxy instance using provided class and class instance
        /// </summary>
        /// <param name="class">Class of the proxy instance</param>
        /// <param name="target">Object to proxy</param>
        /// <returns>Proxy instance which records every method call to event report</returns>
        public object CreateClassProxy(Type @class, object target)
        {
            return _generator.CreateClassProxyWithTarget(@class, target, new ReportingInerceptor(_reportFactory));
        }

        /// <summary>
        ///     Creates new proxy instance using provided interface and implementation
        /// </summary>
        /// <typeparam name="TTarget">Interface which proxy instance will implement</typeparam>
        /// <param name="target">Object to proxy</param>
        /// <returns>Proxy instance which records every method call to event report</returns>
        public TTarget CreateInterfaceProxy<TTarget>(TTarget target) where TTarget : class
        {
            return _generator.CreateInterfaceProxyWithTarget(target, new ReportingInerceptor(_reportFactory));
        }

        /// <summary>
        ///     Creates new proxy instance using provided interface and implementation
        /// </summary>
        /// <param name="interface">Interface which proxy instance will implement</param>
        /// <param name="target">Object to proxy</param>
        /// <returns>Proxy instance which records every method call to event report</returns>
        public object CreateInterfaceProxy(Type @interface, object target)
        {
            return _generator.CreateInterfaceProxyWithTarget(@interface, target, new ReportingInerceptor(_reportFactory));
        }
    }
}