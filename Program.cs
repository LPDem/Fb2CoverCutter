using System.CommandLine;

namespace Fb2CoverCutter
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var rootCommand = new RootCommand("FB2 Cover images cutter");

            var inputFileArgument = new Argument<string>("Input file");
            var outputFileArgument = new Argument<string?>("Output file", () => null, "If not specified, input file will be overwritten");
            var quietOption = new Option<bool>(new[] { "--quiet", "-q" }, "Do not write anything to console");

            rootCommand.AddArgument(inputFileArgument);
            rootCommand.AddArgument(outputFileArgument);
            rootCommand.AddOption(quietOption);

            rootCommand.SetHandler(
                (inputFile, outputFile, quiet) =>
                {
                    var cutter = new Fb2CoverCutter(inputFile, outputFile ?? inputFile, quiet);
                    cutter.Execute();
                },
                inputFileArgument,
                outputFileArgument,
                quietOption
            );

            rootCommand.Invoke(args);
        }
    }
}