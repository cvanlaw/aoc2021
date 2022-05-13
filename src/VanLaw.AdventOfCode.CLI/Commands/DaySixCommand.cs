using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using McMaster.Extensions.CommandLineUtils;

using Microsoft.Extensions.Logging;

namespace VanLaw.AdventOfCode.CLI.Commands
{
    public class DaySixCommand : CommandBase
    {

        public DaySixCommand(ILogger<CommandBase> logger) : base(logger)
        {
        }

        public override async Task<int> ExecuteAsync(CancellationToken cancellationToken)
        {
            var lines = await this.ReadAllLinesAsync();

            var input = lines[0]
                .Split(',')
                .Select(x => int.Parse(x))
                .ToList();

            this._logger.LogInformation($"Part One: {this.SolvePartOneAsync(input)}");
            this._logger.LogInformation($"Part Two: {this.SolvePartTwoAsync(input)}");

            return 0;
        }

        private string SolvePartOneAsync(List<int> input)
        {
            var fish = input;
            for(var i = 0; i < 80; i++)
            {
                var tempFish = new List<int>(fish);
                fish.Clear();
                fish.AddRange(tempFish.Select(f => 
                {
                    if(--f < 0)
                    {
                        f = 6;
                        fish.Add(8);
                    }

                    return f;
                }).ToList());
            }

           return $"{fish.Count} fish";
        }

        private string SolvePartTwoAsync(List<int> input)
        {
            return "unsolved";
        }

        public override CommandLineApplication Configure(CommandLineApplication commandLineApplication)
        {
            commandLineApplication.Command("day-six", cmd =>
            {
                cmd.Description = "Solve Day Six of AOC2021";
                base.Configure(cmd);
                cmd.OnExecuteAsync((token) => this.ExecuteAsync(token));
            });

            return commandLineApplication;
        }
    }
}