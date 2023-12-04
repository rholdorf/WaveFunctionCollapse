using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace WaveFunctionCollapse.Tiles;

public class Tile
{
    public Rectangle SourceRectangle { get;}

    public Tile(Rectangle sourceRectangle, IReadOnlyList<Color> tileData)
    {
        SourceRectangle = sourceRectangle;
        
        TopEdgeColors = new Color[sourceRectangle.Width];
        for(var i=0; i< sourceRectangle.Width; i++)
            TopEdgeColors[i] = tileData[i];
        
        BottomEdgeColors = new Color[sourceRectangle.Width];
        for(var i=0; i< sourceRectangle.Width; i++)
            BottomEdgeColors[i] = tileData[(sourceRectangle.Height - 1) * sourceRectangle.Width + i];
        
        RightEdgeColors = new Color[sourceRectangle.Height];
        for(var i=0; i< sourceRectangle.Height; i++)
            RightEdgeColors[i] = tileData[i * sourceRectangle.Width + sourceRectangle.Width - 1];
        
        LeftEdgeColors = new Color[sourceRectangle.Height];
        for(var i=0; i< sourceRectangle.Height; i++)
            LeftEdgeColors[i] = tileData[i * sourceRectangle.Width];
    }

    public Color[] TopEdgeColors { get;}
    public Color[] RightEdgeColors { get; }
    public Color[] BottomEdgeColors { get;}
    public Color[] LeftEdgeColors { get;}
    public List<Tile> TopEdgeValidConnections { get; set; } = new();
    public List<Tile> RightEdgeValidConnections { get; set; } = new();
    public List<Tile> BottomEdgeValidConnections { get; set; } = new();
    public List<Tile> LeftEdgeValidConnections { get; set; } = new();
}
