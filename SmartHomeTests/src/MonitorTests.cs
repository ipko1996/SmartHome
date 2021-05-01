using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace SmartHome.Tests
{
    [TestClass()]
    public class MonitorTests
    {
        [TestMethod()]
        public async Task GetSession_ExistingHomeIdAsync()
        {
            var monitor = new Monitor();

            var session = await monitor.getSession("KD34AF24DS");

            Assert.IsNotNull(session.sessionId);
        }

        [TestMethod()]
        public async Task GetSession_NonExistingHomeIdAsync()
        {
            var monitor = new Monitor();

            var session = await monitor.getSession("asdf");

            Assert.IsNull(session.sessionId);
        }
    }
}