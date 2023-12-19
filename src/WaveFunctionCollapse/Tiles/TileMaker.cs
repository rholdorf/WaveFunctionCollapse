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
        
        _cellMap2 = texture.FindUniqueTiles(_rectanglesMap, widthInCells, heightInCells, _random);
        _uniqueTiles = _cellMap2.UniqueTiles.ToArray();
        Learn();
        Collapse();
    }
    
    public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle)
    {
        DrawCellMap(spriteBatch, destinationRectangle);
        //DrawLearned(spriteBatch, destinationRectangle);
    }

    private void DrawCellMap(SpriteBatch spriteBatch, Rectangle destinationRectangle)
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

    private void DrawLearned(SpriteBatch spriteBatch, Rectangle destinationRectangle)
    {
        var cellWidth = _rectanglesMap[0][0].Width;
        var cellHeight = _rectanglesMap[0][0].Height;
        
        var offsetRectangle = new Rectangle(0, 0, cellWidth, cellHeight);
        var offsetX = 4;
        var offsetY = 4;
        offsetRectangle.X += destinationRectangle.X;
        offsetRectangle.Y += destinationRectangle.Y;

        for (var i = 0; i < _uniqueTiles.Length; i++)
        {
            var tile = _uniqueTiles[i];
            var middleTopRect = new Rectangle(offsetRectangle.X + cellWidth, offsetRectangle.Y, cellWidth, cellHeight);
            var leftMiddleRect = new Rectangle(offsetRectangle.X, offsetRectangle.Y + cellHeight, cellWidth, cellHeight);
            var middleMiddleRect = new Rectangle(offsetRectangle.X + cellWidth, offsetRectangle.Y + cellHeight, cellWidth, cellHeight);
            var rightMiddleRect = new Rectangle(offsetRectangle.X + cellHeight + cellWidth, offsetRectangle.Y + cellHeight, cellWidth, cellHeight);
            var middleBottomRect = new Rectangle(offsetRectangle.X + cellWidth, offsetRectangle.Y + cellHeight + cellHeight, cellWidth, cellHeight);

            if (tile.HasTopEdgeConnection)
            {
                var topTile = _uniqueTiles[tile.TopEdgeConnections.First()];
                spriteBatch.Draw(_texture, middleTopRect, topTile.SourceRectangle, Color.White);
            }

            if (tile.HasLeftEdgeConnection)
            {
                var leftTile = _uniqueTiles[tile.LeftEdgeConnections.First()];
                spriteBatch.Draw(_texture, leftMiddleRect, leftTile.SourceRectangle, Color.White);
            }

            spriteBatch.Draw(_texture, middleMiddleRect, tile.SourceRectangle, Color.White);

            if (tile.HasRightEdgeConnection)
            {
                var rightTile = _uniqueTiles[tile.RightEdgeConnections.First()];
                spriteBatch.Draw(_texture, rightMiddleRect, rightTile.SourceRectangle, Color.White);
            }

            if (tile.HasBottomEdgeConnection)
            {
                var bottomTile = _uniqueTiles[tile.BottomEdgeConnections.First()];
                spriteBatch.Draw(_texture, middleBottomRect, bottomTile.SourceRectangle, Color.White);
            }

            // move to next position
            offsetRectangle.X += cellWidth * 3 + offsetX; 
            if (offsetRectangle.X > _texture.Width)
            {
                offsetRectangle.Y += (cellHeight * 3) + offsetY;
                offsetRectangle.X = destinationRectangle.X;
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
        BorderInFill borderInFill = new(_cellMap2, _uniqueTiles, _random);
        borderInFill.Fill(x, y);
    }
}
