using System.Collections;
using System.Collections.Generic;
using RogueProject.Utils;

public class Grid2D<T> : IEnumerable<T>
{
    private readonly T[] _data;

    public Grid2D(Vector2Int size)
    {
        _data = new T[size.x * size.y];
        Size = size;
    }

    public Vector2Int Size { get; }

    private int GetIndex(Vector2Int pos) {
        return pos.x + (Size.x * pos.y);
    }

    public bool InBounds(Vector2Int pos) {
        return pos.x >= 0 && pos.x < Size.x && pos.y >= 0 && pos.y < Size.y;
    }

    public T this[int x, int y] {
        get => this[new Vector2Int(x, y)];

        set => this[new Vector2Int(x, y)] = value;
    }

    public T this[Vector2Int pos] {
        get => _data[GetIndex(pos)];

        set => _data[GetIndex(pos)] = value;
    }

    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_data).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
