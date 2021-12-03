namespace VanLaw.AdventOfCode.CLI.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;

    using McMaster.Extensions.CommandLineUtils;

    public interface ICommand
    {
        Task<int> ExecuteAsync(CancellationToken cancellationToken);
        void Configure(CommandLineApplication commandLineApplication);
    }
}