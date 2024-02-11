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

var solvedBlocks = Solve(new Board(DateTime.Today.Month, DateTime.Today.Day), 0b11111111);
if (solvedBlocks != null)
{
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
}
else
{
    Console.WriteLine("UNSOLVED :-(");
}

IReadOnlyList<BlockTrace>? Solve(Board board, int piecesLeft)
{
    for (int i = 0; i < Pieces.AllPieces.Length; i++)
    {
        int pieceBit = 1 << i;
        if ((piecesLeft & pieceBit) == 0)
            continue;

        var nextPiecesLeft = piecesLeft ^ pieceBit;
        foreach (var block in Pieces.AllPieces[i].Blocks)
        {
            if (board.CanBePlaced(block, out Board? nextBoard))
            {
                if (nextPiecesLeft == 0)
                {
                    return nextBoard.PlacedBlocks;
                }

                var solvedBlocks = Solve(nextBoard, nextPiecesLeft);
                if (solvedBlocks != null)
                {
                    return solvedBlocks;
                }
            }
        }
    }

    return null;
}
