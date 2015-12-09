using System;
using System.Diagnostics;
using System.Globalization;
using System.ServiceModel;
using MessageContracts.SystemInformation;
using NServiceBus;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("non-breaking-changes.Consumer");
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.EnableInstallers();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.Conventions().DefiningEventsAs(t =>
                t.Namespace.Contains("Contracts") &&
                t.IsInterface &&
                t.Name.StartsWith("I") && t.Name.EndsWith("ed"));
            using (var bus = Bus.Create(busConfiguration).Start())
            {
                Console.WriteLine("*** Consumer running");
                Console.WriteLine("Enter 'svc' to call service");
                Console.WriteLine("Enter 'cls' to clear the screen");
                Console.WriteLine("Enter 'quit' to exit");
                var command = "";
                while ((command = Console.ReadLine()) != "quit")
                {
                    if (command == "svc")
                    {
                        CallService();
                        continue;
                    }
                    if (command == "cls")
                    {
                        Console.Clear();
                        continue;
                    }
                    Console.WriteLine("Unknown command; try 'svc', 'cls' or 'quit'");
                }
            }
        }

        private static void CallService()
        {
            const string uriString = "http://localhost:8080/";
            var watch = new Stopwatch();

            Console.WriteLine("Calling service on {0}... ", uriString);
            watch.Start();
            var request = new Request
            {
                DateTimeFormat = CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern,
                IncludeDateTime = true,
                IncludeDriveInformation = true,
                IncludeUserName = true
            };
            var response = GetSystemInformation(request, uriString);
            watch.Stop();
            Console.WriteLine($"System information through service:");
            Console.WriteLine($"DateTime:           {response.DateTime}");
            Console.WriteLine($"System Free Space:  {response.FreeSpace} MB");
            Console.WriteLine($"Logged in User:     {response.UserName}");
            Console.WriteLine($"- Service roundtrip: {watch.Elapsed.TotalMilliseconds}ms");
        }

        private static Response GetSystemInformation(Request request, string uriString)
        {
            using (
                var factory = new ChannelFactory<IProvideSystemInformation>(new BasicHttpBinding(),
                    new EndpointAddress(uriString)))
            {
                return factory.CreateChannel().GetSystemInformation(request);
            }
        }
    }

    public class OnDriveFreeSpaceChanged : IHandleMessages<IDriveFreeSpaceChanged>
    {
        public void Handle(IDriveFreeSpaceChanged message)
        {
            Console.WriteLine($"Received event {nameof(IDriveFreeSpaceChanged)}! Free space: {message.FreeSpace} MB");
        }
    }
}
