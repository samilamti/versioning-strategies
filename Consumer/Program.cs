using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using MessageContracts.SystemInformation;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var request = new Request
            {
                DateTimeFormat = CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern,
                IncludeDateTime = true,
                IncludeDriveInformation = true,
                IncludeUserName = true
            };
            var response = GetSystemInformation(request);
            Console.WriteLine($"System information through service:");
            Console.WriteLine($"DateTime:           {response.DateTime}");
            Console.WriteLine($"System Free Space:  {response.FreeSpace} MB");
            Console.WriteLine($"Logged in User:     {response.UserName}");
        }

        private static Response GetSystemInformation(Request request)
        {
            using (
                var factory = new ChannelFactory<IProvideSystemInformation>(new BasicHttpBinding(),
                    new EndpointAddress("http://localhost:8080/")))
            {
                return factory.CreateChannel().GetSystemInformation(request);
            }
        }
    }
}
