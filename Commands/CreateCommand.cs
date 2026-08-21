using IP_Changer.Enums;
using IP_Changer.Models;
using IP_Changer.Services;
using IP_Changer.Storage;
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
            if (context.Data is ProfileStore profileStore)
            {
                AnsiConsole.MarkupLine($"Creating a profile with the name: [blue]{settings.ProfileName}[/]\n");

                var adapters = AdapterService.GetAdapters();
                var choices = adapters.Select(a => Markup.Escape(a.Name)).ToList();

                var adapterName = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]What adapter would you like to configure?[/]")
                        .AddChoices(choices)
                );

                var adapter = AdapterService.GetAdapter(adapterName);

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
                    case NetworkMode.DHCP: break;
                    case NetworkMode.Static:
                        {
                            if (AnsiConsole.Confirm("Will there be multiple IPs?"))
                            {
                                var numOfIp = AnsiConsole.Ask<int>("How many?");

                                if (numOfIp >= 2 && numOfIp <= 5)
                                {
                                    for (int i = 0; i < numOfIp; i++)
                                    {
                                        Ip = AnsiConsole.Ask<string>("IP:");
                                        Subnet = AnsiConsole.Ask<string>("Subnet:");

                                        Ips.Add(i, Ip);
                                        Subnets.Add(i, Subnet);
                                    }

                                    Gateway = AnsiConsole.Ask<string>("Gateway:");

                                    AnsiConsole.WriteLine();

                                    if (AnsiConsole.Confirm("Set DNS automatically?"))
                                    {
                                        PriDns = Gateway; // TODO: do better
                                    }
                                    else
                                    {
                                        AnsiConsole.WriteLine();

                                        PriDns = AnsiConsole.Ask<string>("Primary DNS:");
                                        SecDns = AnsiConsole.Ask<string>("Secondary DNS:");
                                    }

                                    var profile = new NetworkProfile()
                                    {
                                        Name = settings.ProfileName,
                                        Adapter = adapter,
                                        Mode = networkMode,
                                        IpAddress = Ips,
                                        SubnetMask = Subnets,
                                        Gateway = Gateway,
                                        DnsServers = [PriDns, SecDns]
                                    };

                                    AnsiConsole.Status()
                                        .Start("Saving Profile...", ctx =>
                                        {
                                            profileStore.Save(profile);
                                            AnsiConsole.MarkupLine("[green]Profile saved![/]");
                                        });
                                }
                                else
                                {
                                    AnsiConsole.MarkupLine($"[red]Can not have this many Ips: [blue]{numOfIp}[/], must be between 2 and 5[/]");
                                }
                            }
                            else
                            {
                                AnsiConsole.WriteLine();

                                Ip = AnsiConsole.Ask<string>("IP:");
                                Subnet = AnsiConsole.Ask<string>("Subnet:");
                                Gateway = AnsiConsole.Ask<string>("Gateway:");

                                Ips.Add(0, Ip);
                                Subnets.Add(0, Subnet);

                                AnsiConsole.WriteLine();

                                if (AnsiConsole.Confirm("Set DNS automatically?"))
                                {
                                    PriDns = Gateway; // TODO: do better
                                }
                                else
                                {
                                    AnsiConsole.WriteLine();

                                    PriDns = AnsiConsole.Ask<string>("Primary DNS:");
                                    SecDns = AnsiConsole.Ask<string>("Secondary DNS:");
                                }

                                var Profile = new NetworkProfile()
                                {
                                    Name = settings.ProfileName,
                                    Adapter = adapter,
                                    Mode = networkMode,
                                    IpAddress = Ips,
                                    SubnetMask = Subnets,
                                    Gateway = Gateway,
                                    DnsServers = [PriDns, SecDns]
                                };

                                AnsiConsole.Status()
                                    .Start("Saving Profile...", ctx =>
                                    {
                                        profileStore.Save(Profile);
                                        AnsiConsole.MarkupLine("[green]Profile saved![/]");
                                    });
                            }
                            break;
                        }
                }
            }
            return 0;
        }
    }
}
