using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using MessageContracts.SystemInformation;
using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

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

            using (var host = new ServiceHost(typeof (SystemInformationService), baseAddress))
            using (var bus = Bus.Create(busConfiguration).Start())
            {
                var smb = new ServiceMetadataBehavior
                {
                    HttpGetEnabled = true,
                    MetadataExporter = {PolicyVersion = PolicyVersion.Policy15}
                };
                host.Description.Behaviors.Add(smb);
                host.Open();
                Console.WriteLine("The service is ready at {0}", baseAddress);

                ShowHelp();
                Prompt();

                var command = "";
                while ((command = Console.ReadLine()) != "quit")
                {
                    if (command == "msg")
                    {
                        var serviceInstance = new SystemInformationService();
                        var request = new Request
                        {
                            IncludeInformationForDrives = new[] {"C", "D", "E", "F", "G"}
                        };
                        var systemInformation = serviceInstance.GetSystemInformation(request);
                        bus.Publish<IDriveFreeSpaceChanged>(info =>
                        {
                            info.Drives = systemInformation.DriveInformation;
                        });
                        Prompt();
                        continue;
                    }
                    if (command == "cls")
                    {
                        Console.Clear();
                        ShowHelp();
                        Prompt();
                    }
                    Console.WriteLine("Unknown command; try 'quit', 'cls' or 'msg'");
                    Prompt();
                }

                host.Close();
            }
        }

        private static void Prompt()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Producer> ");
        }

        private static void ShowHelp()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-- Producer ----------------------------");
            Console.WriteLine("Enter 'msg' to send a domain event.");
            Console.WriteLine("Enter 'cls' to clear the screen.");
            Console.WriteLine("Enter 'quit' to stop the service.");
            Console.WriteLine("----------------------------------------");
        }
    }
}
