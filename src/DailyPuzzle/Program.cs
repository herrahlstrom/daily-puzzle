using System.Diagnostics;

ConsoleColor[] resultColors = [
    ConsoleColor.Blue,
    ConsoleColor.Green,
    ConsoleColor.DarkGray,
    ConsoleColor.Cyan,
    ConsoleColor.White,
    ConsoleColor.DarkRed,
    ConsoleColor.Magenta,
    ConsoleColor.DarkYellow];
const int PrintResultSize = 3;

int month = DateTime.Today.Month;
int day = DateTime.Today.Day;

if (args.Length >= 2)
{
    if (int.TryParse(args[0], out int arg0) && int.TryParse(args[1], out int arg1))
    {
        month = arg0;
        day = arg1;
    }
}

var solvedBlocks = Board.Solve(month, day)
    ?? throw new NotSupportedException("UNSOLVABLE :-(");

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
                Console.Write("⦾⦾");
            }
        }
        Console.WriteLine();
    }
}

Console.ForegroundColor = backupForegroundColor;