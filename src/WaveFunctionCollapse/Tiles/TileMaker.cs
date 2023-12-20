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

    public TileMaker(Texture2D texture, int tileWidth, int tileHeight)
    {
        _texture = texture;
        var widthInCells = _texture.Width / tileWidth;
        var heightInCells = _texture.Height / tileHeight;
        _rectanglesMap = texture.MapRectangles(tileWidth, tileHeight);

        _cellMap2 = texture.FindUniqueTiles(_rectanglesMap, widthInCells, heightInCells);
        _uniqueTiles = _cellMap2.TileSet.ToArray();
        LearnAccordingToTheTexture();
        Collapse();
    }

    public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle)
    {
        DrawCellMap(spriteBatch, destinationRectangle);
        //DrawTiles(spriteBatch, destinationRectangle);
    }

    private void Collapse()
    {
        var collapser = new TheOtherMethod(_cellMap2);
        collapser.Fill(5, 5);
    }

    private void DrawCellMap(SpriteBatch spriteBatch, Rectangle destinationRectangle)
    {
        var offsetRectangle = new Rectangle(0, 0, 0, 0);
        for (var y = 0; y < _cellMap2.Height; y++)
        {
            for (var x = 0; x < _cellMap2.Width; x++)
            {
                var cell = _cellMap2.Cells[x][y];
                if (!cell.Collapsed)
                {
                    continue;
                }

                var index = cell.TileIndex;
                var sourceRectangle = _rectanglesMap[x][y];
                offsetRectangle.X = destinationRectangle.X + sourceRectangle.X;
                offsetRectangle.Y = destinationRectangle.Y + sourceRectangle.Y;
                offsetRectangle.Height = sourceRectangle.Height;
                offsetRectangle.Width = sourceRectangle.Width;

                spriteBatch.Draw(_texture, offsetRectangle, _uniqueTiles[index].SourceRectangle, Color.White);
            }
        }
    }

    private void DrawTiles(SpriteBatch spriteBatch, Rectangle destinationRectangle)
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

            if (tile.HasTopConnection)
            {
                var topTile = _uniqueTiles[tile.TopConnections.First()];
                spriteBatch.Draw(_texture, middleTopRect, topTile.SourceRectangle, Color.White);
            }

            if (tile.HasLeftConnection)
            {
                var leftTile = _uniqueTiles[tile.LeftConnections.First()];
                spriteBatch.Draw(_texture, leftMiddleRect, leftTile.SourceRectangle, Color.White);
            }

            spriteBatch.Draw(_texture, middleMiddleRect, tile.SourceRectangle, Color.White);

            if (tile.HasRightConnection)
            {
                var rightTile = _uniqueTiles[tile.RightConnections.First()];
                spriteBatch.Draw(_texture, rightMiddleRect, rightTile.SourceRectangle, Color.White);
            }

            if (tile.HasBottomConnection)
            {
                var bottomTile = _uniqueTiles[tile.BottomConnections.First()];
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

    private void LearnAccordingToTheTexture()
    {
        for (var y = 0; y < _cellMap2.Height; y++)
        {
            for (var x = 0; x < _cellMap2.Width; x++)
            {
                var cell = _cellMap2.Cells[x][y];
                var tile = _uniqueTiles[cell.TileIndex]; 

                if(cell.CanConnectTop)
                    tile.TopConnections.Add(cell.TopCell.TileIndex);

                if(cell.CanConnectRight)
                    tile.RightConnections.Add(cell.RightCell.TileIndex);

                if(cell.CanConnectBottom)
                    tile.BottomConnections.Add(cell.BottomCell.TileIndex);

                if(cell.CanConnectLeft)
                    tile.LeftConnections.Add(cell.LeftCell.TileIndex);
            }
        }
    }
}
