using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace WaveFunctionCollapse.Tiles;

public class Tile
{
    public Rectangle SourceRectangle { get; }

    public int Index { get; }

    public Tile(Rectangle sourceRectangle, int index)
    {
        SourceRectangle = sourceRectangle;
        Index = index;
    }

    public List<int> TopConnections { get; } = new();
    public List<int> RightConnections { get; } = new();
    public List<int> BottomConnections { get; } = new();
    public List<int> LeftConnections { get; } = new();

    public List<int> GetConnections(Position position)
    {
        return position switch
        {
            Position.Top => TopConnections,
            Position.Right => RightConnections,
            Position.Bottom => BottomConnections,
            _ => LeftConnections
        };
    }

    public bool HasTopConnection => TopConnections.Count > 0;

    public bool HasRightConnection => RightConnections.Count > 0;

    public bool HasBottomConnection => BottomConnections.Count > 0;

    public bool HasLeftConnection => LeftConnections.Count > 0;

    public bool IsMiddle => HasTopConnection && HasRightConnection && HasBottomConnection && HasLeftConnection;
}