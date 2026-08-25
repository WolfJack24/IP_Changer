using IP_Changer.Enums;
using IP_Changer.Models;
using IP_Changer.Storage;
using Spectre.Console;

namespace IP_Changer.Services
{
    public class ProfileService(ProfileStore store, NetworkService network)
    {
        private readonly ProfileStore _store = store;
        private readonly NetworkService _network = network;

        public List<NetworkProfile> LoadProfiles()
        {
            (_store.profiles, bool ok) = _store.Load();

            return _store.profiles;
        }

        public bool SaveProfile(NetworkProfile profile)
        {
            return _store.Save(profile);
        }
        }

        public NetworkProfile CreateStaticProfile(
                string name,
                NetworkAdapter adapter,
                NetworkMode mode,
                int numOfIps
            )
        {
            var ips = new List<string>();
            var subnets = new List<string>();

            for (int i = 0; i < numOfIps; i++)
            {
                ips.Add(AnsiConsole.Ask<string>($"IP {i + 1}:"));
                subnets.Add(AnsiConsole.Ask<string>($"Subnet {i + 1}:"));
            }

            var gateway = AnsiConsole.Ask<string>("Gateway: ");

            var dns = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("DNS Configuration")
                        .AddChoices(
                            "Use gateway as DNS",
                            "Use custom DNS",
                            "No DNS"
                        )
                );

            (string, string) GetDns()
            {
                switch (dns)
                {
                    case "Use gateway as DNS":
                        {
                            return (gateway, "");
                        }
                    case "Use custom DNS":
                        {
                            return (AnsiConsole.Ask<string>("Primary DNS:"),
                                    AnsiConsole.Ask<string>("Secondary DNS:"));
                        }
                    case "No DNS":
                        {
                            return ("", "");
                        }
                    default: return ("", "");
                }
            }
            var (primaryDns, secondaryDns) = GetDns();

            return new NetworkProfile
            {
                Name = name,
                Adapter = adapter,
                Mode = mode,
                IpAddress = ips,
                SubnetMask = subnets,
                Gateway = gateway,
                DnsServers = [primaryDns, secondaryDns]
            };
        }

        public NetworkProfile CreateDHCPProfile(
                string name,
                NetworkAdapter adapter,
                NetworkMode mode
            )
        {
            return new NetworkProfile
            {
                Name = name,
                Adapter = adapter,
                Mode = mode,
                IpAddress = [string.Empty],
                SubnetMask = [string.Empty],
                Gateway = string.Empty,
                DnsServers = [string.Empty]
            };
        }
    }
}
