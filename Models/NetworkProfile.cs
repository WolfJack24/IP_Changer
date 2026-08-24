using IP_Changer.Enums;

namespace IP_Changer.Models
{
    public class NetworkProfile
    {
        public string Name { get; set; } = string.Empty;

        public NetworkAdapter? Adapter { get; set; }

        public NetworkMode Mode { get; set; }

        public List<string>? IpAddress { get; set; }
        public List<string>? SubnetMask { get; set; }
        public string Gateway { get; set; } = string.Empty;

        public List<string> DnsServers { get; set; } = [];
    }
}
