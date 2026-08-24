using System.Management;
using System.Net.NetworkInformation;
using IP_Changer.Models;

// *INFO: This is only a windows platform for now (Maybe add Linux and Mac support later (Not garanteed))
#pragma warning disable CA1416 // Validate platform compatibility

namespace IP_Changer.Services
{
    public class AdapterService
    {
        public static List<NetworkAdapter> GetAdapters()
        {
            var adapters = new List<NetworkAdapter>();

            using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter");

            using var results = searcher.Get();

            foreach (var adapter in results)
            {
                var connectionName = adapter["NetConnectionID"]?.ToString();

                if (string.IsNullOrEmpty(connectionName))
                    continue;

                bool isPhysical = adapter["PhysicalAdapter"] is bool physical && physical;
                bool isEnabled = adapter["NetEnabled"] is bool enabled && enabled;

                var networkInterface = NetworkInterface.GetAllNetworkInterfaces()
                    .FirstOrDefault(a => a.Name == connectionName);

                if (networkInterface == null)
                    continue;

                if (!isPhysical)
                    continue;

                adapters.Add(new NetworkAdapter
                {
                    Name = connectionName,
                    Description = adapter["Name"]?.ToString() ?? string.Empty,
                    Id = networkInterface.Id,
                    IsPhysical = isPhysical,
                    IsEnabled = isEnabled,
                    IsConnected = networkInterface.OperationalStatus == OperationalStatus.Up,
                    Type = networkInterface.NetworkInterfaceType
                });
            }

            return adapters;
        }

        public static NetworkAdapter? GetAdapter(string name)
                => GetAdapters().Find(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        public static NetworkAdapter? GetActiveAdapter()
        {
            // Implementation for getting the active adapter
            return null;
        }
    }
}
