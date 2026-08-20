using IP_Changer.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace IP_Changer.Commands
{
    public class SelectApdaterCommand : Command<SelectApdaterCommand.Settings>
    {
        public class Settings : CommandSettings
        {
        }

        protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            var adapters = AdapterService.GetAdapters();
            var choices = adapters.Select(a => Markup.Escape(a.Name)).ToList();

            var adapterName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[blue]What adapter would you like to configure?[/]")
                    .AddChoices(choices)
            );

            var adapter = AdapterService.GetAdapter(adapterName);

            AnsiConsole.MarkupLine($"You have selected the adapter with the name [blue]{adapterName}[/]");

            return 0;
        }
    }
}
