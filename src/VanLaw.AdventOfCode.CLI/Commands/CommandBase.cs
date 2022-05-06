namespace VanLaw.AdventOfCode.CLI.Commands
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using McMaster.Extensions.CommandLineUtils;
    using Microsoft.Extensions.Logging;
    using VanLaw.AdventOfCode.CLI.Interfaces;

    public abstract class CommandBase : ICommand
    {
        protected readonly ILogger<CommandBase> _logger;

        protected CommandBase(ILogger<CommandBase> logger)
        {
            _logger = logger;
        }

        protected CommandOption<string> InputFile { get; set; }

        public abstract Task<int> ExecuteAsync(CancellationToken cancellationToken);

        public virtual CommandLineApplication Configure(CommandLineApplication commandLineApplication)
        { 
            this.InputFile = commandLineApplication.Option<string>(
                "-f|--input-file <FILE>", 
            "Path to the file containing puzzle input.", 
            CommandOptionType.SingleValue);
            this.InputFile
                .IsRequired()
                .Accepts<string>()
                .ExistingFile();

            return commandLineApplication;
        }

        protected async Task<List<string>> ReadAllLinesAsync()
        {
            var lines = new List<string>();

            using(var stream = new FileStream(this.InputFile.ParsedValue, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            using(var reader = new StreamReader(stream))
            {
                string line;
                while((line = await reader.ReadLineAsync()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines;
        }
    }
}