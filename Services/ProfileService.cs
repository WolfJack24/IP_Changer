using IP_Changer.Storage;

namespace IP_Changer.Services
{
    public class ProfileService(ProfileStore store, NetworkService network)
    {
        private readonly ProfileStore _store = store;
        private readonly NetworkService _network = network;
    }
}
