using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WaveFunctionCollapse.Tiles;

public class TileMaker
{
    private readonly Rectangle[][] _rectanglesMap;
    private readonly Texture2D _texture;
    private readonly Tile[] _uniqueTiles;
    private readonly CellCollection _cellMap2;
    private readonly Random _random = new Random(2);
    
    public TileMaker(Texture2D texture, int tileWidth, int tileHeight)
    {
        _texture = texture;
        var widthInCells = _texture.Width / tileWidth;
        var heightInCells = _texture.Height / tileHeight;
        _rectanglesMap = texture.MapRectangles(tileWidth, tileHeight);
        
        (_uniqueTiles, _cellMap2) = texture.FindUniqueTiles(_rectanglesMap, widthInCells, heightInCells);
        Learn();
        Collapse();
    }
    
    public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle)
    {
        var offsetRectangle = new Rectangle(0, 0, 0, 0);
        for (var y = 0; y < _cellMap2.Height; y++)
        {
            for (var x = 0; x < _cellMap2.Width; x++)
            {
                var index = _cellMap2.Cells[x][y].Index;
                var sourceRectangle = _rectanglesMap[x][y];
                offsetRectangle.X = destinationRectangle.X + sourceRectangle.X;
                offsetRectangle.Y = destinationRectangle.Y + sourceRectangle.Y;
                offsetRectangle.Height = sourceRectangle.Height;
                offsetRectangle.Width = sourceRectangle.Width;
        
                spriteBatch.Draw(_texture, offsetRectangle, _uniqueTiles[index].SourceRectangle, Color.White);
            }
        }
    }
    
    private void Learn()
    {
        for (var y = 0; y < _cellMap2.Height; y++)   
        {
            for (var x = 0; x < _cellMap2.Width; x++)
            {
                var cell = _cellMap2.Cells[x][y];

                if(cell.CanConnectTop)
                {
                    var topIndex = cell.TopCell.Index;
                    _uniqueTiles[cell.Index].TopEdgeConnections.Add(topIndex);
                }
                if(cell.CanConnectRight)
                {
                    var rightIndex = cell.RightCell.Index;
                    _uniqueTiles[cell.Index].RightEdgeConnections.Add(rightIndex);
                }
                if(cell.CanConnectBottom)
                {
                    var bottomIndex = cell.BottomCell.Index;
                    _uniqueTiles[cell.Index].BottomEdgeConnections.Add(bottomIndex);
                }
                if(cell.CanConnectLeft)
                {
                    var leftIndex = cell.LeftCell.Index;
                    _uniqueTiles[cell.Index].LeftEdgeConnections.Add(leftIndex);
                }
            }
        }
    }
    
    private void Collapse()
    {
        var x = _random.Next(_cellMap2.Width);
        var y = _random.Next(_cellMap2.Height);
        FloodFill floodFill = new(_cellMap2, _uniqueTiles, _random);
        floodFill.Fill(x, y);
    }
}
