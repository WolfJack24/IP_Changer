using Spectre.Console.Cli;
using System.ComponentModel;

namespace IP_Changer.Commands
{
    public class ApplyCommand : Command<ApplyCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<ProfileName>")]
            [Description("The name of the profile to apply.")]
            public string ProfileName { get; init; } = string.Empty;
        }

        protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            Console.WriteLine($"TODO: Apply profile with the name '{settings.ProfileName}'");
            return 0;
        }
    }
}
