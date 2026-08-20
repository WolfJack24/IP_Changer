using IP_Changer.Models;

namespace IP_Changer.Services
{
    public class NetworkService
    {
        public NetworkConfiguration GetConfiguration(
            NetworkAdapter adapter)
        {
            // Implementation for getting network configuration
            return new NetworkConfiguration();
        }

        public void SetDhcp(
            NetworkAdapter adapter)
        {
            // Implementation for setting DHCP
        }

        public void SetStatic(
            NetworkAdapter adapter,
            NetworkProfile profile)
        {
            // Implementation for setting static IP
        }
    }
}
