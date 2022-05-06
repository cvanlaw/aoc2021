
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using McMaster.Extensions.CommandLineUtils;

using Microsoft.Extensions.Logging;

namespace VanLaw.AdventOfCode.CLI.Commands
{
    public class DayTwoCommand : CommandBase
    {

        public DayTwoCommand(ILogger<CommandBase> logger) : base(logger)
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
            (int x, int y) pos = (x: 0, y: 0);
            var regex = new Regex(@"(?<command>up|down|forward) (?<value>\d)");

            foreach (var line in lines)
            {
                var match = regex.Match(line);
                var cmd = match.Groups["command"].Value;
                var value = int.Parse(match.Groups["value"].Value);
                pos = DayTwoCommand.Move(cmd, value, pos);
            }

            return (pos.x * pos.y).ToString();
        }

        private static (int x, int y) Move(string command, int value, (int x, int y) position) => command switch
        {
            "up" => (x: position.x, y: position.y - value),
            "down" => (x: position.x, y: position.y + value),
            "forward" => (x: position.x + value, y: position.y),
            _ => position,
        };

        private static (int x, int y, int aim) MoveV2(string command, int value, (int x, int y, int aim) position) => command switch
        {
            "up" => (x: position.x, y: position.y, aim: position.aim - value),
            "down" => (x: position.x, y: position.y, aim: position.aim + value),
            "forward" => (x: position.x + value, y: position.y + (position.aim * value), aim: position.aim),
            _ => position,
        };

        private string SolvePartTwoAsync(List<string> lines)
        {
            (int x, int y, int aim) pos = (x: 0, y: 0, aim: 0);
            var regex = new Regex(@"(?<command>up|down|forward) (?<value>\d)");

            foreach (var line in lines)
            {
                var match = regex.Match(line);
                var cmd = match.Groups["command"].Value;
                var value = int.Parse(match.Groups["value"].Value);
                pos = DayTwoCommand.MoveV2(cmd, value, pos);
            }

            return (pos.x * pos.y).ToString();
        }

        public override CommandLineApplication Configure(CommandLineApplication commandLineApplication)
        {
            commandLineApplication.Command("day-two", cmd =>
            {
                cmd.Description = "Solve Day Two of AOC2021";
                base.Configure(cmd);
                cmd.OnExecuteAsync((token) => this.ExecuteAsync(token));
            });

            return commandLineApplication;
        }
    }
}