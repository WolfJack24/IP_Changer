using IP_Changer.Storage;

namespace IP_Changer.Services
{
    public class ProfileService
    {
        private readonly ProfileStore _store;
        private readonly NetworkService _network;

        public ProfileService(
            ProfileStore store,
            NetworkService network)
        {
            _store = store;
            _network = network;
        }
    }
}
