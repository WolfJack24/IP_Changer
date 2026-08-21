namespace IP_Changer.Storage
{
    public class OnLoad
    {
        public static void CheckLocations()
        {
            if (!Path.Exists(Locations.storageLocation))
                Directory.CreateDirectory(Locations.storageLocation);

            if (!Path.Exists(Locations.logsLocation))
                Directory.CreateDirectory(Locations.logsLocation);

            if (!File.Exists(Locations.profileLocation))
                File.Create(Locations.profileLocation);

            if (!File.Exists(Locations.adapterLocation))
                File.Create(Locations.adapterLocation);
        }
    }
}
