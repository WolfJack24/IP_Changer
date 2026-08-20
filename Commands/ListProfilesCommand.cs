using Spectre.Console.Cli;

namespace IP_Changer.Commands
{
    public class ListProfilesCommand : Command<ListProfilesCommand.Settings>
    {
        public class Settings : CommandSettings
        {
        }

        protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            Console.WriteLine("TODO: List all profiles");
            return 0;
        }
    }
}
