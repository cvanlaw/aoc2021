
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
    public class DayFourCommand : CommandBase
    {
        public DayFourCommand(ILogger<CommandBase> logger) : base(logger)
        {
        }

        public override async Task<int> ExecuteAsync(CancellationToken cancellationToken)
        {
            var lines = await this.ReadAllLinesAsync();
            (var numbers, var boards) = this.ParseInput(lines);

            var solvedBoards = boards.Select(x => this.SolveBoard(x, numbers)).Where(x => x.score.HasValue).ToList();

            this._logger.LogInformation($"Part One: {this.SolvePartOneAsync(solvedBoards)}");
            this._logger.LogInformation($"Part Two: {this.SolvePartTwoAsync(solvedBoards)}");

            return 0;
        }

        private (int? turnNumber, int? score) SolveBoard(int[][] board, List<int> numbers)
        {
            var turnNumber = 0;
            foreach(var draw in numbers)
            {
                turnNumber++;
                for (var i = 0; i < 5; i++)
                for (var j = 0; j < 5; j++)
                {
                    if (board[i][j] == draw)
                    {
                        board[i][j] = -1;
                    }
                }

                if (this.IsBoardAWinner(board))
                {
                    return (turnNumber, this.ScoreBoard(board, draw));
                }
            }

            return (null, null);
        }

        private (List<int> numbers, List<int[][]> boards) ParseInput(List<string> lines)
        {
            var parsedNumbers = lines[0].Split(',').Select(x => int.Parse(x)).ToList();
            var boards = new List<int[][]>();
            var regex = new Regex(@"(\d{0,2}){5}");

            lines.RemoveAt(0);

            for (var i = 0; i < lines.Count - 6; i += 6)
            {
                var board = new List<int[]>
                {
                    regex.Matches(lines[i+1]).Where(x => !string.IsNullOrWhiteSpace(x.Value)).Select(x => int.Parse(x.Value)).ToArray(),
                    regex.Matches(lines[i+2]).Where(x => !string.IsNullOrWhiteSpace(x.Value)).Select(x => int.Parse(x.Value)).ToArray(),
                    regex.Matches(lines[i+3]).Where(x => !string.IsNullOrWhiteSpace(x.Value)).Select(x => int.Parse(x.Value)).ToArray(),
                    regex.Matches(lines[i+4]).Where(x => !string.IsNullOrWhiteSpace(x.Value)).Select(x => int.Parse(x.Value)).ToArray(),
                    regex.Matches(lines[i+5]).Where(x => !string.IsNullOrWhiteSpace(x.Value)).Select(x => int.Parse(x.Value)).ToArray()
                };

                boards.Add(board.ToArray());
            }

            return (parsedNumbers, boards);
        }

        private string SolvePartOneAsync(List<(int? turnNumber, int? score)> solvedBoards)
        {
            return solvedBoards.MinBy(x => x.turnNumber).score.ToString();
        }

        private int ScoreBoard(int[][] board, int lastNumber)
        {
            var sum = board.ToList().SelectMany(x => x.ToList()).Where(x => x != -1).Sum();
            return sum * lastNumber;
        }

        private bool IsBoardAWinner(int[][] board)
        {
            for (var i = 0; i < 5; i++)
            {
                var row = Enumerable.Range(0, 5)
                    .Select(x => board[i][x])
                    .ToArray();
                var column = Enumerable.Range(0, 5)
                    .Select(x => board[x][i])
                    .ToArray();
                if (this.IsVectorAWinner(row) || this.IsVectorAWinner(column))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsVectorAWinner(int[] vector)
        {
            return vector.All(x => x == -1);
        }

        private string SolvePartTwoAsync(List<(int? turnNumber, int? score)> solvedBoards)
        {
            
            return solvedBoards.MaxBy(x => x.turnNumber).score.ToString();
        }

        public override CommandLineApplication Configure(CommandLineApplication commandLineApplication)
        {
            commandLineApplication.Command("day-four", cmd =>
            {
                cmd.Description = "Solve Day Four of AOC2021";
                base.Configure(cmd);
                cmd.OnExecuteAsync((token) => this.ExecuteAsync(token));
            });

            return commandLineApplication;
        }
    }
}