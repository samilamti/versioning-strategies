using System;
using System.IO;
using MessageContracts.SystemInformation;

namespace Producer
{
    public class SystemInformationService : IProvideSystemInformation
    {
        public Response GetSystemInformation(Request informationRequest)
        {
            var response = new Response();
            if (informationRequest.IncludeDateTime)
            {
                response.DateTime = System.DateTime.Now.ToString(informationRequest.DateTimeFormat);
            }
            if (informationRequest.IncludeDriveInformation)
            {
                response.FreeSpace = BytesToMegaBytes(new DriveInfo("c").AvailableFreeSpace);
            }
            if (informationRequest.IncludeUserName)
            {
                response.UserName = Environment.UserName;
            }
            return response;
        }

        private int BytesToMegaBytes(long bytes) => (int)(bytes/1024/1024);
    }
}