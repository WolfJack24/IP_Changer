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
            Console.WriteLine("TODO: List all adapters");
            return 0;
        }
    }
}