using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using McMaster.Extensions.CommandLineUtils;

using Microsoft.Extensions.Logging;

namespace VanLaw.AdventOfCode.CLI.Commands
{
    public class DayThreeCommand : CommandBase
    {

        public DayThreeCommand(ILogger<CommandBase> logger) : base(logger)
        {
        }

        public override async Task<int> ExecuteAsync(CancellationToken cancellationToken)
        {
            var lines = await this.ReadAllLinesAsync();


            this._logger.LogInformation($"Part One: {this.SolvePartOneAsync(lines)}");
            this._logger.LogInformation($"Part Two: {this.SolvePartTwoAsync(lines)}");

            return 0;
        }

        private string SolvePartOneAsync(List<string> lines)
        {
            var linesArrays = lines.Select(x => x.ToArray()).ToList();
            var gammeRate = new int[12];
            for(var i = 0; i < 12; i++)
            {
                var average = linesArrays.Select(x => (int)x[i]).Average();
                gammeRate[i] = average > 48.5 ? 1 : 0;
            }

            var epsilonRate = new int[12];
            for(var i = 0; i < 12; i++)
            {
                epsilonRate[i] = linesArrays.Select(x => (double)x[i]).Sum() / (double)lines.Count < 48.5 ? 1 : 0;
            }

            var gammaDecimal = Convert.ToInt64(string.Join(null, gammeRate), 2);
            var epsilonDecimal = Convert.ToInt64(string.Join(null, epsilonRate), 2);
            this._logger.LogInformation($"E: {epsilonDecimal} G: {gammaDecimal}");
            return (gammaDecimal * epsilonDecimal).ToString();
        }

        private string SolvePartTwoAsync(List<string> lines)
        {
            return "unsolved";
        }

        public override CommandLineApplication Configure(CommandLineApplication commandLineApplication)
        {
            commandLineApplication.Command("day-three", cmd =>
            {
                cmd.Description = "Solve Day Three of AOC2021";
                base.Configure(cmd);
                cmd.OnExecuteAsync((token) => this.ExecuteAsync(token));
            });

            return commandLineApplication;
        }
    }
}