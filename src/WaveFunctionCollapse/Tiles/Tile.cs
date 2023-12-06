using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace WaveFunctionCollapse.Tiles;

public class Tile
{
    public Rectangle SourceRectangle { get;}

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
    public List<int> TopEdgeConnections { get; } = new();
    public List<int> RightEdgeConnections { get; } = new();
    public List<int> BottomEdgeConnections { get; } = new();
    public List<int> LeftEdgeConnections { get; } = new();
    
    public int PickRandomTopEdgeConnection(Random random)
    {
        return TopEdgeConnections[random.Next(TopEdgeConnections.Count)];
    }
    
    public int PickRandomLeftEdgeConnection(Random random)
    {
        return LeftEdgeConnections[random.Next(LeftEdgeConnections.Count)];
    }    
    
    public int PickRandomRightEdgeConnection(Random random)
    {
        return RightEdgeConnections[random.Next(RightEdgeConnections.Count)];
    }
    
    public int PickRandomBottomEdgeConnection(Random random)
    {
        return BottomEdgeConnections[random.Next(BottomEdgeConnections.Count)];
    }
}
