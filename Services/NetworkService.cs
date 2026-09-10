using System.Management;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using IP_Changer.Enums;
using IP_Changer.Models;

// *INFO: This is only a windows platform for now (Maybe add Linux and Mac support later (Not garanteed))
#pragma warning disable CA1416 // Validate platform compatibility

namespace IP_Changer.Services
{
    public class NetworkService
    {
        public NetworkConfiguration GetConfiguration(NetworkAdapter adapter)
        {
            var networkInterface = NetworkInterface
                .GetAllNetworkInterfaces()
                .FirstOrDefault(ni => ni.Id == adapter.Id) ?? throw new InvalidOperationException(
                    $"Adapter '{adapter.Name}' could not be found."
                );

            var properties = networkInterface.GetIPProperties();

            var configuration = new NetworkConfiguration
            {
                Adapter = adapter,
            };

            foreach (var address in properties.UnicastAddresses)
            {
                if (address.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    configuration.IpAddress?.Add(
                        address.Address.ToString()
                    );

                    configuration.SubnetMask?.Add(
                        address.IPv4Mask.ToString()
                    );
                }
            }

            foreach (var gateway in properties.GatewayAddresses)
            {
                if (gateway.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    configuration.Gateway =
                        gateway.Address.ToString();

                    break;
                }
            }

            foreach (var dns in properties.DnsAddresses)
            {
                if (dns.AddressFamily == AddressFamily.InterNetwork)
                {
                    configuration.DnsServers.Add(
                        dns.ToString()
                    );
                }
            }

            using var searcher = new ManagementObjectSearcher(
                "SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = TRUE"
            );
            using var results = searcher.Get();

            foreach (ManagementObject config in results)
            {
                if (config["SettingID"]?.ToString() == adapter.Id)
                {
                    configuration.Mode = config["DHCPEnabled"] is bool enabled && enabled
                        ? NetworkMode.DHCP
                        : NetworkMode.Static;
                }
            }

            return configuration;
        }

        public void SetDhcp(NetworkAdapter adapter)
        {
            // Implementation for setting DHCP
        }

        public void SetStatic(NetworkAdapter adapter, NetworkProfile profile)
        {
            // Implementation for setting static IP
        }
    }
}
