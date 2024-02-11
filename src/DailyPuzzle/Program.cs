using System.Diagnostics;

ConsoleColor[] resultColors = [
    ConsoleColor.Red,
    ConsoleColor.Blue,
    ConsoleColor.Green,
    ConsoleColor.DarkGray,
    ConsoleColor.Cyan,
    ConsoleColor.DarkRed,
    ConsoleColor.Magenta,
    ConsoleColor.DarkYellow];
const int PrintResultSize = 3;

var timestamp = Stopwatch.GetTimestamp();

var solvedBlocks = Board.Solve(DateTime.Today.Month, DateTime.Today.Day)
    ?? throw new NotSupportedException("UNSOLVABLE :-(");

var elapsed = Stopwatch.GetElapsedTime(timestamp);
Console.WriteLine($"Time to solve: {elapsed.TotalMilliseconds:0}ms.");

Dictionary<Pos, ConsoleColor> pieceIndicators = [];

for (int i = 0; i < solvedBlocks.Count; i++)
{
    var traceBlock = solvedBlocks[i];
    foreach (var pos in traceBlock.Block.Positions)
    {
        pieceIndicators.Add(traceBlock.Position + pos, resultColors[i]);
    }
}

var backupForegroundColor = Console.ForegroundColor;
for (int y = 0; y < 7; y++)
{
    for (int size_y = 0; size_y < PrintResultSize; size_y++)
    {
        for (int x = 0; x < 7; x++)
        {
            Console.ForegroundColor = pieceIndicators.TryGetValue(new Pos(x, y), out ConsoleColor color) ? color : Console.BackgroundColor;
            for (int size_x = 0; size_x < PrintResultSize; size_x++)
            {
                Console.Write("%&");
            }
        }
        Console.WriteLine();
    }
}

Console.ForegroundColor = backupForegroundColor;