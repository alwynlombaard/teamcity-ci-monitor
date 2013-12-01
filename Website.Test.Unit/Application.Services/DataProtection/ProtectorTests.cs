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

        [TestCase(-1, 0)]
        [TestCase(0, 0)]
        [TestCase(1, 0)]
        [TestCase(64, 64)]
        [TestCase(65, 64)]
        public void GenerateKeyCreatesKeyWithCorrectLength(int length, int expected)
        {
            var protector = new Protector();
            var key = protector.GenerateKey(length);
            Assert.That(key.Length, Is.EqualTo(expected));
            
        }
    }
}
