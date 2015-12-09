using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using MessageContracts.SystemInformation;
using NServiceBus;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            var baseAddress = new Uri("http://localhost:8080/");
            var busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("non-breaking-changes.Producer");
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.EnableInstallers();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.Conventions().DefiningEventsAs(t =>
                t.Namespace.Contains("Contracts") &&
                t.IsInterface &&
                t.Name.StartsWith("I") && t.Name.EndsWith("ed"));

            using (var host = new ServiceHost(typeof(SystemInformationService), baseAddress))
            using (var bus = Bus.Create(busConfiguration).Start())
            {
                var smb = new ServiceMetadataBehavior
                {
                    HttpGetEnabled = true,
                    MetadataExporter = { PolicyVersion = PolicyVersion.Policy15 }
                };
                host.Description.Behaviors.Add(smb);
                host.Open();

                Console.WriteLine("The service is ready at {0}", baseAddress);
                Console.WriteLine("Enter 'msg' to send a domain event.");
                Console.WriteLine("Enter 'quit' to stop the service.");


                var command = "";
                while ((command = Console.ReadLine()) != "quit")
                {
                    if (command == "msg")
                    {
                        var serviceInstance = new SystemInformationService();
                        var systemInformation = serviceInstance.GetSystemInformation(new Request {IncludeDriveInformation = true});
                        bus.Publish<IDriveFreeSpaceChanged>(info => info.FreeSpace = systemInformation.FreeSpace.GetValueOrDefault(0));
                        continue;
                    }
                    Console.WriteLine("Unknown command; try 'quit' or 'msg'");
                }

                host.Close();
            }
        }
    }
}
