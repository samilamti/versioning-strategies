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
    }
}
