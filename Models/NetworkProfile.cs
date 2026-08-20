using IP_Changer.Enums;

namespace IP_Changer.Models
{
    public class NetworkProfile
    {
        public string Name { get; set; } = string.Empty;

        public string Adapter { get; set; } = string.Empty;

        public NetworkMode Mode { get; set; }

        public string? IpAddress { get; set; }
        public string? SubnetMask { get; set; }
        public string? Gateway { get; set; }

        public List<string> DnsServers { get; set; } = [];
    }
}
