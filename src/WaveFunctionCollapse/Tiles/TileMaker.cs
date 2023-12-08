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
    private readonly Random _random = new Random(1);
    
    public TileMaker(Texture2D texture, int tileWidth, int tileHeight)
    {
        _texture = texture;
        var widthInCells = _texture.Width / tileWidth;
        var heightInCells = _texture.Height / tileHeight;
        _rectanglesMap = texture.MapRectangles(tileWidth, tileHeight);
        
        (_uniqueTiles, _cellMap2) = texture.FindUniqueTiles(_rectanglesMap, widthInCells, heightInCells);
        //_uniqueTiles.FindValidConnections();
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
                    var topIndex = _cellMap2.Cells[x][y - 1].Index;
                    _uniqueTiles[cell.Index].TopEdgeConnections.Add(topIndex);
                }
                if(cell.CanConnectRight)
                {
                    var rightIndex = _cellMap2.Cells[x + 1][y].Index;
                    _uniqueTiles[cell.Index].RightEdgeConnections.Add(rightIndex);
                }
                if(cell.CanConnectBottom)
                {
                    var bottomIndex = _cellMap2.Cells[x][y + 1].Index;
                    _uniqueTiles[cell.Index].BottomEdgeConnections.Add(bottomIndex);
                }
                if(cell.CanConnectLeft)
                {
                    var leftIndex = _cellMap2.Cells[x - 1][y].Index;
                    _uniqueTiles[cell.Index].LeftEdgeConnections.Add(leftIndex);
                }
            }
        }
    }
    
    private void Collapse()
    {
        for (var y = 0; y < _cellMap2.Height; y++)
        {
            for (var x = 0; x < _cellMap2.Width; x++)
            {
                var cell = _cellMap2.Cells[x][y];

                if(cell.CanConnectTop && _uniqueTiles[cell.Index].HasTopEdgeConnection())
                {
                    var topCell = cell.TopCell;
                    if (!topCell.Collapsed)
                    {
                        var topIndex = _uniqueTiles[cell.Index].PickRandomTopEdgeConnection(_random);
                        topCell.Index = topIndex;
                        topCell.Collapsed = true;
                    }
                }
                if(cell.CanConnectRight && _uniqueTiles[cell.Index].HasRightEdgeConnection())
                {
                    var rightCell = cell.RightCell;
                    if (!rightCell.Collapsed)
                    {
                        var rightIndex = _uniqueTiles[cell.Index].PickRandomRightEdgeConnection(_random);
                        rightCell.Index = rightIndex;
                        rightCell.Collapsed = true;
                    }
                }
                if(cell.CanConnectBottom && _uniqueTiles[cell.Index].HasBottomEdgeConnection())
                {
                    var bottomCell = cell.BottomCell;
                    if (!bottomCell.Collapsed)
                    {
                        var bottomIndex = _uniqueTiles[cell.Index].PickRandomBottomEdgeConnection(_random);
                        bottomCell.Index = bottomIndex;
                        bottomCell.Collapsed = true;
                    }
                }
                if(cell.CanConnectLeft && _uniqueTiles[cell.Index].HasLeftEdgeConnection())
                {
                    var leftCell = cell.LeftCell;
                    if (!leftCell.Collapsed)
                    {
                        var leftIndex = _uniqueTiles[cell.Index].PickRandomLeftEdgeConnection(_random);
                        leftCell.Index = leftIndex;
                        leftCell.Collapsed = true;
                    }
                }
            }
        }
    }
    
    
   

}
