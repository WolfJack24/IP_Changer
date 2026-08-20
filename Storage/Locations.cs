namespace IP_Changer.Storage
{
    public class Locations
    {
        private static readonly string _localAppdata = Environment.GetEnvironmentVariable("LOCALAPPDATA") ?? throw new Exception("Where is Local App Data?!");
        public static readonly string storageLocation = Path.Combine(_localAppdata, "IP_Changer");
        public static readonly string profileLocation = Path.Combine(storageLocation, "Profiles.json");
        public static readonly string adapterLocation = Path.Combine(storageLocation, "Adapters.json");
    }
}
