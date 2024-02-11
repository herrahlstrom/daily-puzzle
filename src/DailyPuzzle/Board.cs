using System.Diagnostics.CodeAnalysis;

public class Board
{
    const int Height = 7;
    const int Width = 7;
    private bool[,] _board;
    private Pos _nextPos;
    private List<BlockTrace> _placedBlocks = [];

    public Board(int month, int day)
    {
        _board = new bool[7, 7];
        for (int x = 0; x <= 5; x++) _board[x, 0] = true;
        for (int x = 0; x <= 5; x++) _board[x, 1] = true;
        for (int x = 0; x <= 6; x++) _board[x, 2] = true;
        for (int x = 0; x <= 6; x++) _board[x, 3] = true;
        for (int x = 0; x <= 6; x++) _board[x, 4] = true;
        for (int x = 0; x <= 6; x++) _board[x, 5] = true;
        for (int x = 0; x <= 2; x++) _board[x, 6] = true;

        switch (month)
        {
            case >= 1 and <= 6:
                _board[month - 1, 0] = false;
                break;
            case >= 7 and <= 12:
                _board[month - 7, 1] = false;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(month));
        }

        switch (day)
        {
            case >= 1 and <= 7:
                _board[day - 1, 2] = false;
                break;
            case >= 8 and <= 14:
                _board[day - 8, 3] = false;
                break;
            case >= 12 and <= 21:
                _board[day - 15, 4] = false;
                break;
            case >= 22 and <= 28:
                _board[day - 22, 5] = false;
                break;
            case >= 29 and <= 31:
                _board[day - 29, 6] = false;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(day));
        }

        _nextPos = GetNextPos();
    }

    private Board()
    {
        _board = new bool[Width, Height];
    }

    public IReadOnlyList<BlockTrace> PlacedBlocks => _placedBlocks;

    public static IReadOnlyList<BlockTrace>? Solve(int month, int day)
    {
        var board = new Board(month, day);
        return board.Solve(board, 0b11111111);
    }

    public bool CanBePlaced(Block block, [MaybeNullWhen(false)] out Board nextBoard)
    {
        nextBoard = null;
        List<Pos> positionsToBeBlocked = [];

        for (int i = 0; i < block.Positions.Count; i++)
        {
            positionsToBeBlocked.Clear();

            var pivot = block.Positions[i];
            for (int j = 0; j < block.Positions.Count; j++)
            {
                var x = _nextPos.X + block.Positions[j].X - pivot.X;
                var y = _nextPos.Y + block.Positions[j].Y - pivot.Y;
                positionsToBeBlocked.Add(new(x, y));
            }

            if (positionsToBeBlocked.All(pos => pos.X >= 0 && pos.X < Width && pos.Y >= 0 && pos.Y < Height && _board[pos.X, pos.Y]))
            {
                nextBoard = Clone();
                foreach (var pos in positionsToBeBlocked)
                {
                    nextBoard._board[pos.X, pos.Y] = false;
                }
                nextBoard._placedBlocks.Add(new BlockTrace(_nextPos - pivot, block));
                nextBoard._nextPos = nextBoard.GetNextPos();
                return true;
            }
        }

        return false;
    }

    public Board Clone()
    {
        var clone = new Board();

        Array.Copy(_board, clone._board, _board.Length);
        clone._nextPos = _nextPos;
        clone._placedBlocks.AddRange(_placedBlocks);

        return clone;
    }

    private Pos GetNextPos()
    {
        for (int y = 0; y < _board.GetLength(1); y++)
        {
            for (int x = 0; x < _board.GetLength(0); x++)
            {
                if (_board[x, y])
                {
                    return new Pos(x, y);
                }
            }
        }
        return new(-1, -1);
    }
    private IReadOnlyList<BlockTrace>? Solve(Board board, int piecesLeft)
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
}

public record struct BlockTrace(Pos Position, Block Block);
