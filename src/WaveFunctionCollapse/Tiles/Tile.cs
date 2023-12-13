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

    public List<int> TopEdgeConnections { get; } = new();
    public List<int> RightEdgeConnections { get; } = new();
    public List<int> BottomEdgeConnections { get; } = new();
    public List<int> LeftEdgeConnections { get; } = new();

    public bool HasTopEdgeConnection => TopEdgeConnections.Count > 0;

    public bool HasRightEdgeConnection => RightEdgeConnections.Count > 0;

    public bool HasBottomEdgeConnection => BottomEdgeConnections.Count > 0;

    public bool HasLeftEdgeConnection => LeftEdgeConnections.Count > 0;
}