
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using McMaster.Extensions.CommandLineUtils;

using Microsoft.Extensions.Logging;

namespace VanLaw.AdventOfCode.CLI.Commands
{
    public class DayOneCommand : CommandBase
    {
        
        public DayOneCommand(ILogger<CommandBase> logger) : base(logger)
        {
        }

        public override async Task<int> ExecuteAsync(CancellationToken cancellationToken)
        {
            var lines = await this.ReadAllLinesAsync();
            

            if(!int.TryParse(lines.FirstOrDefault(), out var previous))
            {
                return 1;
            }
            this.SolvePartOneAsync(lines, previous);
            this.SolvePartTwoAsync(lines);

            return 0;
        }

        private void SolvePartOneAsync(List<string> lines, int previous)
        {
            var result = 0;
            
            for(var i = 1; i < lines.Count; i++)
            {
                var current = int.Parse(lines[i]);

                result += current > previous ? 1 : 0;
                previous = current;
            }

            this._logger.LogInformation($"Part One: {result.ToString()}");
        }

        private void SolvePartTwoAsync(List<string> lines)
        {
            var result = 0;

            int getWindowSum(int startIndex)
            {
                return int.Parse(lines[startIndex]) + int.Parse(lines[startIndex - 1]) + int.Parse(lines[startIndex - 2]);
            }

            for(var i = 3; i < lines.Count; i++)
            {
                var current = getWindowSum(i);
                var previous = getWindowSum(i - 1);
                result += current > previous ? 1 : 0;
            }

            this._logger.LogInformation($"Part Two: {result.ToString()}");
        }

        public override CommandLineApplication Configure(CommandLineApplication commandLineApplication)
        {
            commandLineApplication.Command("day-one", cmd => 
            {
                cmd.Description = "Solve Day One of AOC2021";
                base.Configure(cmd);
                cmd.OnExecuteAsync((token) => this.ExecuteAsync(token));
            });

            return commandLineApplication;
        }
    }
}