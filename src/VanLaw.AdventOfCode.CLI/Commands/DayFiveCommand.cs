
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
    public class DayFiveCommand : CommandBase
    {
        private class Point
        {
            
            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X { get; private set; }
            public int Y { get; private set; }

            public override bool Equals(object obj)
            {
                return obj is Point point &&
                       X == point.X &&
                       Y == point.Y;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(X, Y);
            }
        }
        private class VentLine
        {
            public VentLine(int x1,  int y1,int x2, int y2)
            {
                this.Start = new Point(x1, y1);
                this.End = new Point(x2, y2);
            }

            public Point Start { get; private set; }

            public Point End { get; private set; }

            public bool IsValid()
            {
                return this.Start.X == this.End.X || this.Start.Y == this.End.Y;
            }

            public List<Point> ProjectPoints()
            {
                var points = new List<Point>();
                points.Add(this.Start);
                points.Add(this.End);

                var isHorizontal = this.Start.X - this.End.X != 0;

                if(isHorizontal)
                {
                    for(var i = 1; i < Math.Abs(this.Start.X - this.End.X); i++)
                    {
                        points.Add(new Point(Math.Min(this.Start.X, this.End.X) + i, this.Start.Y));
                    }
                }
                else
                {
                    for(var i = 1; i < Math.Abs(this.Start.Y - this.End.Y); i++)
                    {
                        points.Add(new Point(this.Start.X, Math.Min(this.Start.Y, this.End.Y) + i));
                    }
                }

                return points;
            }
        }
        private readonly Regex _regex = new Regex(@"(?<x1>\d*),(?<y1>\d*) -> (?<x2>\d*),(?<y2>\d*)");

        public DayFiveCommand(ILogger<CommandBase> logger) : base(logger)
        {
        }

        public override async Task<int> ExecuteAsync(CancellationToken cancellationToken)
        {
            var lines = await this.ReadAllLinesAsync();
            var ventLines = this.ParseInput(lines);

            this._logger.LogInformation($"Part One: {this.SolvePartOneAsync(ventLines)}");
            this._logger.LogInformation($"Part Two: {this.SolvePartTwoAsync(ventLines)}");

            return 0;
        }

        private List<VentLine> ParseInput(List<string> lines)
        {
            return lines
                .Select(x => this.ParseLine(x))
                .Where(x => x.IsValid())
                .ToList();
        }

        private VentLine ParseLine(string line)
        {
            var match = this._regex.Match(line);
            var vent = new VentLine(
                int.Parse(match.Groups["x1"].ValueSpan),
                int.Parse(match.Groups["y1"].ValueSpan),
                int.Parse(match.Groups["x2"].ValueSpan),
                int.Parse(match.Groups["y2"].ValueSpan)
            );

            return vent;
        }

        private string SolvePartOneAsync(List<VentLine> ventLines)
        {
            var allPoints = ventLines.SelectMany(line => line.ProjectPoints()).ToList();
            var groupedPoints = allPoints.GroupBy(
                point => point,
                (basePoint, points) => new
                {
                    Key = basePoint,
                    Count = points.Count()
                }
            );

            var overlaps = groupedPoints.Count(gp => gp.Count >= 2);
            return overlaps.ToString();
        }

       

        private string SolvePartTwoAsync(List<VentLine> ventLines)
        {
            return "unsolved";
        }

        public override CommandLineApplication Configure(CommandLineApplication commandLineApplication)
        {
            commandLineApplication.Command("day-five", cmd =>
            {
                cmd.Description = "Solve Day Five of AOC2021";
                base.Configure(cmd);
                cmd.OnExecuteAsync((token) => this.ExecuteAsync(token));
            });

            return commandLineApplication;
        }
    }
}