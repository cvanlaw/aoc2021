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

            var startingPopulation = new Dictionary<int, long>
            {
                {0, input.Count(x => x == 0)},
                {1, input.Count(x => x == 1)},
                {2, input.Count(x => x == 2)},
                {3, input.Count(x => x == 3)},
                {4, input.Count(x => x == 4)},
                {5, input.Count(x => x == 5)},
                {6, input.Count(x => x == 6)},
                {7, input.Count(x => x == 7)},
                {8, input.Count(x => x == 8)},
            };

            this._logger.LogInformation($"Part One: {this.SolvePartOneAsync(startingPopulation)}");
            this._logger.LogInformation($"Part Two: {this.SolvePartTwoAsync(startingPopulation)}");

            return 0;
        }

        private string SolvePartOneAsync(Dictionary<int, long> input)
        {
            return this.CalculatePopulationAfterDays(80, input);
        }

        private string CalculatePopulationAfterDays(int days, Dictionary<int, long> startingPopulation)
        {
            this._logger.LogInformation($"Calculating population after {days} days. Starting population is {startingPopulation.Count} fish.");

            var population = new Dictionary<int, long>(startingPopulation);

            for(var i = 0; i < days; i++)
            {
                population = this.AdvanceDay(population);
            }
            
           return $"{population.Values.Sum()} fish";
        }

        private Dictionary<int, long> AdvanceDay(Dictionary<int, long> population)
        {
            return new Dictionary<int, long>
            {
                {0, population[1]},
                {1, population[2]},
                {2, population[3]},
                {3, population[4]},
                {4, population[5]},
                {5, population[6]},
                {6, population[0] + population[7]},
                {7, population[8]},
                {8, population[0]},
            };
        }

        private string SolvePartTwoAsync(Dictionary<int, long> input)
        {
            return this.CalculatePopulationAfterDays(256, input);
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