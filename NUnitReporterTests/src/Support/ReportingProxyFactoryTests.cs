using System;
using Moq;
using NUnit.Framework;
using NUnitReporter.EventReport;
using NUnitReporter.EventReport.ProxyFactory;

namespace NUnitReporterTests.Support
{
    public class ReportingProxyFactoryTests
    {
        private Mock<IEventReport> _eventReportMock;
        private ReportingProxyFactory _proxyFactory;
        private Mock<IEventReportFactory> _reportFactoryMock;

        [SetUp]
        public void SetUpMocks()
        {
            _reportFactoryMock = new Mock<IEventReportFactory>();
            _eventReportMock = new Mock<IEventReport>();
            _proxyFactory = new ReportingProxyFactory(_reportFactoryMock.Object);

            _reportFactoryMock
                .Setup(f => f.CurrentTestReport)
                .Returns(_eventReportMock.Object);

            _eventReportMock
                .Setup(r => r.RecordActivityStarted("Call", "123456"))
                .Returns("abcdefg")
                .Verifiable();

            _eventReportMock
                .Setup(r => r.RecordActivityStarted("Call", "098765"))
                .Returns("hklmnop")
                .Verifiable();

            _eventReportMock
                .Setup(r => r.RecordActivityStarted("Charge"))
                .Returns("qprstxy")
                .Verifiable();

            _eventReportMock
                .Setup(r => r.RecordActivityFinished("qprstxy"))
                .Verifiable();

            _eventReportMock
                .Setup(r => r.RecordActivityFinished("hklmnop"))
                .Verifiable();

            _eventReportMock
                .Setup(r => r.RecordActivityFinished("abcdefg"))
                .Verifiable();
        }

        [Test]
        public void ShouldCreateReportingProxyFromInterfaceAndImplementation()
        {
            var phoneMock = new Mock<IPhone>();

            var phoneProxy = _proxyFactory.CreateInterfaceProxy(phoneMock.Object);

            phoneProxy.Call("123456");
            phoneProxy.Call("098765");
            phoneProxy.Charge();

            phoneMock.Verify(p => p.Call("123456"));
            phoneMock.Verify(p => p.Call("098765"));
            phoneMock.Verify(p => p.Charge());

            _eventReportMock.Verify();
        }

        [Test]
        public void ShouldCreateReportingProxyFromClassAndImplemenation()
        {
            var phoneMock = new Mock<Phone>();

            var phoneProxy = _proxyFactory.CreateClassProxy(phoneMock.Object);

            phoneProxy.Call("123456");
            phoneProxy.Call("098765");
            phoneProxy.Charge();

            phoneMock.Verify(p => p.Call("123456"));
            phoneMock.Verify(p => p.Call("098765"));
            phoneMock.Verify(p => p.Charge());

            _eventReportMock.Verify();
        }

        [Test]
        public void ProxyShouldRecordAnError()
        {
            var phoneMock = new Mock<IPhone>();

            var exception = new Exception("Low battery level");

            phoneMock.Setup(p => p.Call("123456")).Throws(exception);

            var phoneProxy = _proxyFactory.CreateInterfaceProxy(phoneMock.Object);

            Assert.That(()=> phoneProxy.Call("123456"), Throws.Exception);
            phoneProxy.Call("098765");
            phoneProxy.Charge();

            phoneMock.Verify(p => p.Call("123456"));
            phoneMock.Verify(p => p.Call("098765"));
            phoneMock.Verify(p => p.Charge());

            _eventReportMock.Verify();

            _eventReportMock.Verify(r => r.RecordError(exception));
        }
    }

    public interface IPhone
    {
        void Call(string phone);
        void Charge();
    }

    public class Phone : IPhone
    {
        public virtual void Call(string phone)
        {
        }

        public virtual void Charge()
        {
        }
    }
}