
public struct Block
{
    private readonly Pos[] _positions;

    public Block(string PieceName, IEnumerable<Pos> positions)
    {
        this.PieceName = PieceName;
        _positions = positions.OrderBy(p => p.X).ThenBy(p => p.Y).ToArray();
    }

    public string PieceName { get; }

    public IReadOnlyList<Pos> Positions => _positions;
}

public record struct Piece(string Name, Block[] Blocks);

public record struct Pos(int X, int Y)
{
    public static Pos operator +(Pos a, Pos b) => new Pos(a.X + b.X, a.Y + b.Y);
    public static Pos operator -(Pos a, Pos b) => new Pos(a.X - b.X, a.Y - b.Y);
}
