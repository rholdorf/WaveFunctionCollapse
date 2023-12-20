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

    public List<int> TopCompatibleTileIndices { get; } = new();
    public List<int> RightCompatibleTileIndices { get; } = new();
    public List<int> BottomCompatibleTileIndices { get; } = new();
    public List<int> LeftCompatibleTileIndices { get; } = new();

    public List<int> GetCompatibleTileIndices(Position position)
    {
        return position switch
        {
            Position.Top => TopCompatibleTileIndices,
            Position.Right => RightCompatibleTileIndices,
            Position.Bottom => BottomCompatibleTileIndices,
            _ => LeftCompatibleTileIndices
        };
    }

    public bool HasTopCompatibleTileIndices => TopCompatibleTileIndices.Count > 0;

    public bool HasRightCompatibleTileIndices => RightCompatibleTileIndices.Count > 0;

    public bool HasBottomCompatibleTileIndices => BottomCompatibleTileIndices.Count > 0;

    public bool HasLeftCompatibleTileIndices => LeftCompatibleTileIndices.Count > 0;
}