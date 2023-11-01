// See https://aka.ms/new-console-template for more information

using System.CommandLine.Builder;
using System.CommandLine.Invocation;

var cli = new CommandLineBuilder(new RootCommand())
    .UseDefaults()
    .AddMiddleware((context, next) =>
    {
        return Task.CompletedTask;
    });
var namecheapCommand = new Command("namecheap", "");

var arg = new Argument<DomainOptions>
{
    Description = "Domain name",
    Name = "domain",
    IsHidden = false,
    Arity = ArgumentArity.ExactlyOne,
    HelpName = "domain"
};
var fileOption = new Option<FileInfo>(
    "--domain",
    description: "Domain to update",
    getDefaultValue: () => new FileInfo(Path.GetTempFileName()));

var lightModeOption = new Option<bool> (
    "--light-mode",
    description: "Determines whether the background color will be black or white");
var foregroundColorOption = new Option<ConsoleColor>(
    "--color",
    description: "Specifies the foreground color of console output",
    getDefaultValue: () => ConsoleColor.White);

var rootCommand = new RootCommand("Query and update namecheap api")
{
    fileOption,
    lightModeOption,
    foregroundColorOption
};

rootCommand.SetHandler((file, lightMode, color) =>
    {
        Console.BackgroundColor = lightMode ? ConsoleColor.White: ConsoleColor.Black;
        Console.ForegroundColor = color;
        Console.WriteLine($"--file = {file.FullName}");
        Console.WriteLine($"File contents:\n{file.OpenText().ReadToEnd()}");
    },
    fileOption,
    lightModeOption,
    foregroundColorOption);

await rootCommand.InvokeAsync(args);

public record DomainOptions
{
    public string Domain { get; set; }
}

public class CliMiddleware
{
    public void Apply(InvocationContext context)
    {
        throw new NotImplementedException();
    }
}