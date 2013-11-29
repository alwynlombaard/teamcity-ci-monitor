using System.Runtime.InteropServices;
using AutoMoq;
using NUnit.Framework;

namespace Website.Test.Unit
{
    public class Service
    {
        private readonly IDependancy _dependancy;

        public Service(IDependancy dependancy)
        {
            _dependancy = dependancy;
        }

        public string RetrieveData()
        {
            return _dependancy.Retrieve();
        }
    }

    public interface IDependancy
    {
        string Retrieve();
    }

    [TestFixture]
    public class ServiceTests
    {
        private AutoMoqer _mocker;

        [SetUp]
        public void SetUp()
        {
            _mocker = new AutoMoqer();
        }

        [Test]
        public void RetrieveData_Retrieves_Hello()
        {
            _mocker.GetMock<IDependancy>()
                .Setup(d => d.Retrieve())
                .Returns("hello");

            var service = _mocker.Resolve<Service>();

            var result = service.RetrieveData();

            Assert.That(result, Is.EqualTo("hello"));
        }
    }
}
