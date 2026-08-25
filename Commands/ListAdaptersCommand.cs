using IP_Changer.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace IP_Changer.Commands
{
    public class ListAdaptersCommand : Command<ListAdaptersCommand.Settings>
    {
        public class Settings : CommandSettings
        {
        }

        protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            var adapters = AdapterService.GetAdapters();

            var table = new Table().RoundedBorder();
            table.Title("Adapters");

            table.AddColumn("Name");
            table.AddColumn("Description");
            table.AddColumn("IsPhysical");
            table.AddColumn("Type");

            for (int i = 0; i < adapters.Count - 1; i++)
            {
                var adapter = adapters[i];
                table.AddRow(
                    Markup.Escape(adapter.Name ?? string.Empty),
                    Markup.Escape(adapter.Description ?? string.Empty),
                    Markup.Escape(adapter.IsPhysical.ToString()),
                    Markup.Escape(adapter.Type.ToString())
                );
            }

            AnsiConsole.Write(table);

            return 0;
        }
    }
}