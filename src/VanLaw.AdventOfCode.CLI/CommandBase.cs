namespace VanLaw.AdventOfCode.CLI
{
    using System.Threading;
    using System.Threading.Tasks;

    using McMaster.Extensions.CommandLineUtils;

    using Microsoft.Extensions.Logging;

    public abstract class CommandBase : ICommand
    {
        protected readonly ILogger<CommandBase> _logger;

        protected CommandBase(ILogger<CommandBase> logger)
        {
            _logger = logger;
        }

        protected CommandOption<int> SchoolCodeOption { get; set; }

        public abstract Task<int> ExecuteAsync(CancellationToken cancellationToken);

        public virtual void Configure(CommandLineApplication commandLineApplication)
        { }
    }
}