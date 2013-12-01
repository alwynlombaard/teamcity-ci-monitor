using NUnit.Framework;
using website.Application.Services.DataProtection;

namespace Website.Test.Unit.Application.Services.DataProtection
{
    [TestFixture]
    public class ProtectorTests
    {
        [SetUp]
        public void SetUp()
        {
            
        }

        [Test]
        public void CanGenerateKey()
        {
            var protector = new Protector();
            var key = protector.GenerateKey(1);
            Assert.NotNull(key);
        }
    }
}
