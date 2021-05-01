using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SmartHome.Tests
{
    [TestClass()]
    public class LoaderTests
    {
        [TestMethod()]
        public void LoadSubs_EverythingIsOkay()
        {
            var loader = new Loader();

            var temp = loader.loadSubscribers();
            Assert.IsInstanceOfType(temp, typeof(Subscribers));
        }

        [TestMethod()]
        [ExpectedException(typeof(System.IO.FileNotFoundException))]
        public void LoadSubscirbers_BadFileName()
        {
            var loader = new Loader();

            loader.loadSubscribersBadFileName();
        }
    }
}