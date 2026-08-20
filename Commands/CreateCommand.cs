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
            Console.WriteLine($"TODO: Create a new profile with the name '{settings.ProfileName}'");
            return 0;
        }
    }
}
