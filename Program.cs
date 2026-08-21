using IP_Changer.Commands;
using IP_Changer.Services;
using IP_Changer.Storage;
using Spectre.Console;
using Spectre.Console.Cli;

namespace IP_Changer
{
    class Program
    {
        static int Main(string[] args)
        {
            var profileStore = new ProfileStore();
            var adapterService = new AdapterService();
            var networkService = new NetworkService();

            var profileService = new ProfileService(profileStore, networkService);

            OnLoad.CheckLocations();
            (profileStore.profiles, bool ok) = profileStore.Load();

            if (ok) AnsiConsole.MarkupLine("[green]Loaded Profiles[/]");
            else AnsiConsole.MarkupLine($"[blue]No profiles or '{Locations.profileLocation}' is missing[/]");

            var app = new CommandApp();
            app.Configure(config =>
            {
                config.AddCommand<CreateCommand>("create")
                    .WithDescription("Create a new profile").WithData(profileStore);
                config.AddCommand<DeleteCommand>("delete").WithDescription("Delete an existing profile");
                config.AddCommand<ApplyCommand>("apply").WithDescription("Apply a profile");
                config.AddBranch("list", list =>
                {
                    list.SetDescription("TODO: Add list description");
                    list.AddCommand<ListProfilesCommand>("profiles").WithDescription("List all profiles");
                    list.AddCommand<ListAdaptersCommand>("adapters").WithDescription("List all adapters");
                });
                //config.AddBranch("select", select =>
                //{
                //    select.SetDescription("TODO: Add select description");
                //    select.AddCommand<SelectApdaterCommand>("adapter")
                //        .WithDescription("Selects the adpater to configure").WithData(adapterService);
                //});
            });

            return app.Run(args);
        }
    }
}