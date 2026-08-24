using IP_Changer.Models;
using Spectre.Console;
using System.Text.Json;

namespace IP_Changer.Storage
{
    public class ProfileStore
    {
        public List<NetworkProfile> profiles = [];
        private readonly JsonSerializerOptions options = new() { WriteIndented = true };

        public (List<NetworkProfile>, bool) Load()
        {
            try
            {
                string jsonString = File.ReadAllText(Locations.profileLocation);
                var profilesList = JsonSerializer.Deserialize<List<NetworkProfile>>(jsonString);

                if (profilesList is not null) return (profilesList, true);
                else return ([], false);
            }
            catch (JsonException ex)
            {
                // AnsiConsole.WriteLine($"Exception: {ex.Message}");
                _ = ex;
                return ([], false);
            }
        }

        public void Save(NetworkProfile profile)
        {
            if (profiles.Any(p => p.Name.Equals(profile.Name, StringComparison.OrdinalIgnoreCase)))
            {
                AnsiConsole.MarkupLine($"[red]You already have a profile with the name of [blue]{profile.Name}[/][/]");
                return;
            }

            profiles.Add(profile);
            File.WriteAllText(Locations.profileLocation, JsonSerializer.Serialize(profiles, options));
        }
    }
}
