public static class Pieces
{
    public static Piece[] AllPieces { get; } = GeneratePieces().ToArray();

    public static IEnumerable<Piece> GeneratePieces()
    {
        //  ■■
        //  ■ 
        // ■■
        yield return CreatePiece("High-Zäta", [new(0, 2), new(1, 2), new(1, 1), new(1, 0), new(2, 0)]);

        // ■
        // ■
        // ■■■
        yield return CreatePiece("Corner", [new(0, 0), new(0, 1), new(0, 2), new(1, 2), new(2, 2)]);

        // ■■
        //  ■■■
        yield return CreatePiece("Low-Zäta", [new(0, 0), new(1, 0), new(1, 1), new(2, 1), new(3, 1)]);

        //  ■
        // ■■■■
        yield return CreatePiece("Litte-T", [new(0, 1), new(1, 1), new(1, 0), new(2, 1), new(3, 1)]);

        // ■■■
        // ■■■
        yield return CreatePiece("Block", [new(0, 0), new(0, 1), new(1, 0), new(1, 1), new(2, 0), new(2, 1)]);

        // ■ ■
        // ■■■
        yield return CreatePiece("U", [new(0, 0), new(0, 1), new(1, 1), new(2, 0), new(2, 1)]);

        // ■
        // ■■■■
        yield return CreatePiece("L", [new(0, 0), new(0, 1), new(1, 1), new(2, 1), new(3, 1)]);

        // ■■■
        //  ■■
        yield return CreatePiece("Gun", [new(0, 0), new(1, 0), new(2, 0), new(1, 1), new(2, 1)]);
    }

    private static Piece CreatePiece(string pieceName, Pos[] positions)
    {
        List<Block> blocks = [];
        HashSet<int> hash = [];

        // Rotate 4 times
        for (int i = 0; i < 4; i++)
        {
            if (i > 0)
                Rotate(positions);
            if (hash.Add(GetHashValue(positions)))
                blocks.Add(new Block(pieceName, positions));
        }

        Flip(positions);

        // Rotate 4 more times
        for (int i = 0; i < 4; i++)
        {
            if (i > 0)
                Rotate(positions);
            if (hash.Add(GetHashValue(positions)))
                blocks.Add(new Block(pieceName, positions));
        }

        return new Piece(pieceName, blocks.ToArray());
    }

    private static int GetHashValue(IEnumerable<Pos> positions)
    {
        int value = 0;
        foreach (var pos in positions)
        {
            value |= 1 << pos.Y * 4 + pos.X;
        }
        return value;
    }

    private static void Rotate(Pos[] positions)
    {
        var pivot = positions[0];
        for (int i = 1; i < positions.Length; i++)
        {
            positions[i] = new Pos(
                pivot.X + positions[i].Y - pivot.Y,
                pivot.Y + pivot.X - positions[i].X);
        }
        Normalize(positions);
    }

    private static void Flip(Pos[] positions)
    {
        var pivot = positions[0];
        for (int i = 1; i < positions.Length; i++)
        {
            positions[i] = new Pos(
                pivot.X + pivot.X - positions[i].X,
                positions[i].Y);
        }
        Normalize(positions);
    }

    private static void Normalize(Pos[] positions)
    {
        int lowX = positions.Min(p => p.X);
        int lowY = positions.Min(p => p.Y);

        if (lowX == 0 && lowY == 0)
            return;

        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = new Pos(
                positions[i].X - lowX,
                positions[i].Y - lowY);
        }
    }
}
