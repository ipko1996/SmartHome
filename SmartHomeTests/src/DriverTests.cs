using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace SmartHome.Tests
{
    [TestClass()]
    public class DriverTests
    {
        [TestMethod()]
        public async Task Get100ResponseFromServer()
        {
            var driver = new Driver();

            var testCommand = "{'homeId':'xxx','boilerCommand':'dX3422','airConditionerCommand':'cX5423'}";

            var res = await driver.getResponseFromServerAsync("EEI23EE12D", testCommand);
            Assert.AreEqual("100", res);
        }

        [TestMethod()]
        public async Task Get101ResponseFromServer()
        {
            var driver = new Driver();

            var testCommand = "{'homeId':'EEI23EE12D','boilerCommand':'dX3422','airConditionerCommand':'cX5423'"; // missing }

            var res = await driver.getResponseFromServerAsync("EEI23EE12D", testCommand);
            Assert.AreEqual("101", res);
        }

        [TestMethod()]
        //earilier before a bugfix server sent status code 500, Internal Server Error
        public async Task Get103ResponseFromServer()
        {
            var driver = new Driver();

            var testCommand = "{'homeId':'xxx','boilerCommand':'dX3422','airConditionerCommand':'cX5423'}";

            var res = await driver.getResponseFromServerAsync("xxx", testCommand);
            Assert.AreEqual("103", res);
        }
    }
}