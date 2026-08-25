using System.ComponentModel;
using System.Text.Json;
using IP_Changer.Services;
using Spectre.Console;
using Spectre.Console.Json;
using Spectre.Console.Cli;

namespace IP_Changer.Commands
{
    public class ListProfilesCommand : Command<ListProfilesCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandOption("-n|--name")]
            [Description("List a specific profile")]
            public string Name { get; init; } = string.Empty;
        }

        protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            if (context.Data is ProfileService profileService)
            {
                var profiles = profileService.LoadProfiles();

                if (settings.Name == string.Empty)
                {
                    var table = new Table().RoundedBorder();
                    table.Title("Profiles");

                    table.AddColumn("Profile Name");
                    table.AddColumn("Adapter Name");
                    table.AddColumn("Mode");

                    for (int i = 0; i < profiles.Count; i++)
                    {
                        table.AddRow(
                            Markup.Escape(profiles[i].Name ?? string.Empty),
                            Markup.Escape(profiles[i].Adapter?.Name ?? string.Empty),
                            Markup.Escape(profiles[i].Mode.ToString() ?? string.Empty));
                    }

                    AnsiConsole.Write(table);
                }
                else
                {
                    var profile = profiles.Find(p => p.Name == settings.Name);
                    var profileJson = JsonSerializer.Serialize(profile);
                    var jsonText = new JsonText(profileJson);

                    AnsiConsole.Write(jsonText);
                }
            }
            return 0;
        }
    }
}
