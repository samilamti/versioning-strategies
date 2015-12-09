using System;
using System.Globalization;
using MessageContracts.SystemInformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Producer;

namespace Tests
{
    [TestClass]
    public class SystemInformationServiceTests
    {
        [TestMethod, TestCategory("Integration")]
        public void IncludeEverything()
        {
            var request = new Request { IncludeDateTime = true, IncludeDriveInformation = true, IncludeUserName = true, DateTimeFormat = DateTimeFormatInfo.InvariantInfo.ShortDatePattern };
            var service = new SystemInformationService();
            var response = service.GetSystemInformation(request);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.DateTime);
            Assert.IsTrue(response.FreeSpace > 0);
            Assert.IsNotNull(response.UserName);
        }

        [TestMethod, TestCategory("Integration")]
        public void V11DriveInformation()
        {
            var request = new Request { IncludeInformationForDrives = new[] {"C", "D"}};
            var service = new SystemInformationService();
            var response = service.GetSystemInformation(request);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.DriveInformation);
            Assert.AreEqual(2, response.DriveInformation.Length);
            Assert.IsNotNull(response.DriveInformation[0]);
            Assert.AreEqual("C", response.DriveInformation[0].Drive);
            Assert.IsNotNull(response.DriveInformation[1]);
            Assert.AreEqual("D", response.DriveInformation[1].Drive);
            StringAssert.StartsWith(response.DriveInformation[1].FreeSpaceFormatted, "Error");
        }
    }
}
