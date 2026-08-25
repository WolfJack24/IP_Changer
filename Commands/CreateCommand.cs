using IP_Changer.Enums;
using IP_Changer.Services;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace IP_Changer.Commands
{
    public class CreateCommand : Command<CreateCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<ProfileName>")]
            [Description("The name of the profile to create.")]
            public string ProfileName { get; init; } = string.Empty;
        }

        protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            if (context.Data is ProfileService profileService)
            {
                AnsiConsole.MarkupLine($"Creating a profile with the name: [blue]{settings.ProfileName}[/]\n");

                var adapters = AdapterService.GetAdapters();
                var choices = adapters.Select(a => Markup.Escape(a.Name)).ToList();

                var adapterName = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]What adapter would you like to configure?[/]")
                        .AddChoices(choices)
                );

                var adapter = adapters.Find(a => a.Name == adapterName) ?? throw new Exception("Adapter can not be null");

                AnsiConsole.MarkupLine($"Modifying: [blue]{adapterName}[/]");

                var networkMode = AnsiConsole.Prompt(
                    new SelectionPrompt<NetworkMode>()
                        .Title("[green]What mode are you configuring?[/]")
                        .AddChoices(NetworkMode.DHCP, NetworkMode.Static)
                );

                AnsiConsole.MarkupLine($"Network Mode: [blue]{networkMode}[/]\n");

                string Ip = string.Empty;
                var Ips = new Dictionary<int, string>();
                string Subnet = string.Empty;
                var Subnets = new Dictionary<int, string>();
                string Gateway = string.Empty;
                string PriDns = string.Empty;
                string SecDns = string.Empty;

                switch (networkMode)
                {
                    case NetworkMode.DHCP:
                        {
                            var profile = profileService.CreateDHCPProfile(
                                    settings.ProfileName,
                                    adapter,
                                    networkMode);

                            AnsiConsole.Status()
                                .Start("Saving Profile...", ctx =>
                                {
                                    var saved = profileService.SaveProfile(profile);
                                    if (saved) AnsiConsole.MarkupLine("[green]Profile saved![/]");
                                    else AnsiConsole.MarkupLine("[red]Profile failed to save![/]");
                                });
                        }
                        break;
                    case NetworkMode.Static:
                        {
                            var numOfIp = AnsiConsole.Prompt(
                                new TextPrompt<int>("How many IPs?")
                                    .Validate(
                                        n => n is >= 2 and <= 5
                                        ? ValidationResult.Success()
                                        : ValidationResult.Error("[red]Please enter a number between 2 and 5.[/]")));

                            var profile = profileService.CreateStaticProfile(
                                    settings.ProfileName,
                                    adapter,
                                    networkMode,
                                    numOfIp);
                            profileService.SaveProfile(profile);

                            AnsiConsole.Status()
                                .Start("Saving Profile...", ctx =>
                                {
                                    var saved = profileService.SaveProfile(profile);
                                    if (saved) AnsiConsole.MarkupLine("[green]Profile saved![/]");
                                    else AnsiConsole.MarkupLine("[red]Profile failed to save![/]");
                                });
                        }
                        break;
                }
            }
            return 0;
        }
    }
}
