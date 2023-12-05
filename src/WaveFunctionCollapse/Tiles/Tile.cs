using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace WaveFunctionCollapse.Tiles;

public class Tile
{
    public Rectangle SourceRectangle { get;}
    public string Name { get; set; }
    public int Index { get; }

    public Tile(Rectangle sourceRectangle, IReadOnlyList<Color> tileData, int index)
    {
        SourceRectangle = sourceRectangle;
        Index = index;
        
        TopEdge = new Color[sourceRectangle.Width];
        for(var i=0; i< sourceRectangle.Width; i++)
            TopEdge[i] = tileData[i];
        
        BottomEdge = new Color[sourceRectangle.Width];
        for(var i=0; i< sourceRectangle.Width; i++)
            BottomEdge[i] = tileData[(sourceRectangle.Height - 1) * sourceRectangle.Width + i];
        
        RightEdge = new Color[sourceRectangle.Height];
        for(var i=0; i< sourceRectangle.Height; i++)
            RightEdge[i] = tileData[i * sourceRectangle.Width + sourceRectangle.Width - 1];
        
        LeftEdge = new Color[sourceRectangle.Height];
        for(var i=0; i< sourceRectangle.Height; i++)
            LeftEdge[i] = tileData[i * sourceRectangle.Width];
    }

    public Color[] TopEdge { get;}
    public Color[] RightEdge { get; }
    public Color[] BottomEdge { get;}
    public Color[] LeftEdge { get;}
    public List<Tile> TopEdgeConnections { get; } = new();
    public List<Tile> RightEdgeConnections { get; } = new();
    public List<Tile> BottomEdgeConnections { get; } = new();
    public List<Tile> LeftEdgeConnections { get; } = new();
    
    public Tile PickRandomTopEdgeConnection(Random random)
    {
        return TopEdgeConnections[random.Next(TopEdgeConnections.Count)];
    }
    
    public Tile PickRandomLeftEdgeConnection(Random random)
    {
        return LeftEdgeConnections[random.Next(LeftEdgeConnections.Count)];
    }    
    
    public Tile PickRandomRightEdgeConnection(Random random)
    {
        return RightEdgeConnections[random.Next(RightEdgeConnections.Count)];
    }
    
    public Tile PickRandomBottomEdgeConnection(Random random)
    {
        return BottomEdgeConnections[random.Next(BottomEdgeConnections.Count)];
    }
}
