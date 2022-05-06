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
            for (var i = 0; i < 12; i++)
            {
                var average = linesArrays.Select(x => (int)x[i]).Average();
                gammeRate[i] = average > 48.5 ? 1 : 0;
            }

            var epsilonRate = new int[12];
            for (var i = 0; i < 12; i++)
            {
                // 0 and 1 are 48 and 49 in ASCII
                epsilonRate[i] = linesArrays.Select(x => (int)x[i]).Average() < 48.5 ? 1 : 0;
            }

            var gammaDecimal = Convert.ToInt64(string.Join(null, gammeRate), 2);
            var epsilonDecimal = Convert.ToInt64(string.Join(null, epsilonRate), 2);
            return (gammaDecimal * epsilonDecimal).ToString();
        }

        private string SolvePartTwoAsync(List<string> lines)
        {
            var linesArrays = lines.Select(x => x.ToArray()).ToList();

            char applyBitCriteria(Func<double, char> func, List<char[]> input, int index) => func.Invoke(input.Select(x => (int)x[index]).Average());
            List<char[]> filterByBitCriteria(List<char[]> input, char bitToSelect, int index) => input.Where(x => x[index] == bitToSelect).ToList();

            Func<double, char> o2GeneratorBitCriteria = (double x) => x >= 48.5 ? '1' : '0';
            Func<double, char> co2ScrubberBitCriteria = (double x) => x >= 48.5 ? '0' : '1';

            var o2GeneratorRating = new int[12];
            var co2ScrubberRating = new int[12];

            var o2Array = new List<char[]>(linesArrays);
            var co2Array = new List<char[]>(linesArrays);
            for (var i = 0; i < 12; i++)
            {
                if (o2Array.Count == 1)
                {
                    o2GeneratorRating[i] = int.Parse(o2Array.First()[i].ToString());
                }
                else
                {
                    var bit = applyBitCriteria(o2GeneratorBitCriteria, o2Array, i);
                    o2GeneratorRating[i] = int.Parse(bit.ToString());
                    o2Array = filterByBitCriteria(o2Array, bit, i);
                }

                if (co2Array.Count == 1)
                {
                    co2ScrubberRating[i] = int.Parse(co2Array.First()[i].ToString());
                }
                else
                {
                    var bit = applyBitCriteria(co2ScrubberBitCriteria, co2Array, i);
                    co2ScrubberRating[i] = int.Parse(bit.ToString());
                    co2Array = filterByBitCriteria(co2Array, bit, i);
                }
            }

            var o2RatingDecimal = Convert.ToInt64(string.Join(null, o2GeneratorRating), 2);
            var co2RatingDecimal = Convert.ToInt64(string.Join(null, co2ScrubberRating), 2);
            return (o2RatingDecimal * co2RatingDecimal).ToString();
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