using System.Net.NetworkInformation;

namespace IP_Changer.Models
{
    public class NetworkAdapter
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;

        public bool IsPhysical { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsConnected { get; set; }

        public NetworkInterfaceType Type { get; set; }
    }
}
