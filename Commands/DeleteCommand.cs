using Spectre.Console.Cli;
using System.ComponentModel;

namespace IP_Changer.Commands
{
    public class DeleteCommand : Command<DeleteCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<ProfileName>")]
            [Description("The name of the profile to delete.")]
            public string ProfileName { get; init; } = string.Empty;
        }

        protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            Console.WriteLine($"TODO: Delete profile with the name '{settings.ProfileName}'");
            return 0;
        }
    }
}
