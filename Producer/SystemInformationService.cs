using System;
using System.Collections.Generic;
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
            if (informationRequest.IncludeInformationForDrives != null)
            {
                var driveInformation = new List<DriveInformation>(informationRequest.IncludeInformationForDrives.Length);
                foreach (var driveLetter in informationRequest.IncludeInformationForDrives)
                {
                    try
                    {
                        var bytes = new DriveInfo(driveLetter).AvailableFreeSpace;
                        driveInformation.Add(new DriveInformation
                        {
                            Drive = driveLetter,
                            FreeSpaceBytes = bytes,
                            FreeSpaceFormatted = $"{BytesToMegaBytes(bytes)} MB"
                        });
                    }
                    catch (Exception ex)
                    {
                        driveInformation.Add(new DriveInformation
                        {
                            Drive = driveLetter,
                            FreeSpaceBytes = -1,
                            FreeSpaceFormatted = $"Error retrieving drive information ({ex.Message})"
                        });
                    }
                }
                response.DriveInformation = driveInformation.ToArray();
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