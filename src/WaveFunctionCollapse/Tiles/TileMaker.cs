using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WaveFunctionCollapse.Tiles;

public class TileMaker
{
    private readonly Rectangle[][] _rectanglesMap;
    private readonly Texture2D _texture;
    private readonly List<Tile> _uniqueTiles;
    private readonly CellCollection _cellMap2;

    public TileMaker(Texture2D texture, int tileWidth, int tileHeight)
    {
        _texture = texture;
        var widthInCells = _texture.Width / tileWidth;
        var heightInCells = _texture.Height / tileHeight;
        _rectanglesMap = texture.MapRectangles(tileWidth, tileHeight);

        _cellMap2 = texture.FindUniqueTiles(_rectanglesMap, widthInCells, heightInCells);
        _uniqueTiles = _cellMap2.TileSet;
        LearnAccordingToTheTexture();
        //RemoveBorderTiles();
        Collapse();
    }

    public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle)
    {
        //DrawCellMap(spriteBatch, destinationRectangle);
        DrawTiles(spriteBatch, destinationRectangle);
    }
    
    private void RemoveBorderTiles()
    {
        var borderTiles = _uniqueTiles.Where(o => !o.HasTopCompatibleTileIndices || !o.HasRightCompatibleTileIndices || !o.HasBottomCompatibleTileIndices || !o.HasLeftCompatibleTileIndices);
        for (var i = 0; i < _uniqueTiles.Count; i++)
        {
            var tile = _uniqueTiles[i];
            tile.TopCompatibleTileIndices.RemoveAll(o => borderTiles.Any(b => b.Index == o));
            tile.RightCompatibleTileIndices.RemoveAll(o => borderTiles.Any(b => b.Index == o));
            tile.BottomCompatibleTileIndices.RemoveAll(o => borderTiles.Any(b => b.Index == o));
            tile.LeftCompatibleTileIndices.RemoveAll(o => borderTiles.Any(b => b.Index == o));
        }
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

        for (var i = 0; i < _uniqueTiles.Count; i++)
        {
            var tile = _uniqueTiles[i];
            var middleTopRect = new Rectangle(offsetRectangle.X + cellWidth, offsetRectangle.Y, cellWidth, cellHeight);
            var leftMiddleRect = new Rectangle(offsetRectangle.X, offsetRectangle.Y + cellHeight, cellWidth, cellHeight);
            var middleMiddleRect = new Rectangle(offsetRectangle.X + cellWidth, offsetRectangle.Y + cellHeight, cellWidth, cellHeight);
            var rightMiddleRect = new Rectangle(offsetRectangle.X + cellHeight + cellWidth, offsetRectangle.Y + cellHeight, cellWidth, cellHeight);
            var middleBottomRect = new Rectangle(offsetRectangle.X + cellWidth, offsetRectangle.Y + cellHeight + cellHeight, cellWidth, cellHeight);

            if (tile.HasTopCompatibleTileIndices)
            {
                var topTile = _uniqueTiles[tile.TopCompatibleTileIndices.First()];
                spriteBatch.Draw(_texture, middleTopRect, topTile.SourceRectangle, Color.White);
            }

            if (tile.HasLeftCompatibleTileIndices)
            {
                var leftTile = _uniqueTiles[tile.LeftCompatibleTileIndices.First()];
                spriteBatch.Draw(_texture, leftMiddleRect, leftTile.SourceRectangle, Color.White);
            }

            spriteBatch.Draw(_texture, middleMiddleRect, tile.SourceRectangle, Color.White);
            var text = tile.Index.ToString();
            var origin = Game1.Font.MeasureString(text);
            spriteBatch.DrawString(Game1.Font, text, new Vector2(middleMiddleRect.X, middleMiddleRect.Y), Color.White, 0, origin, 0.6f, SpriteEffects.None, 0.5f);

            if (tile.HasRightCompatibleTileIndices)
            {
                var rightTile = _uniqueTiles[tile.RightCompatibleTileIndices.First()];
                spriteBatch.Draw(_texture, rightMiddleRect, rightTile.SourceRectangle, Color.White);
            }

            if (tile.HasBottomCompatibleTileIndices)
            {
                var bottomTile = _uniqueTiles[tile.BottomCompatibleTileIndices.First()];
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
                // if(!cell.IsMiddle)
                //     continue;
                
                var tile = _uniqueTiles[cell.TileIndex]; 

                if(cell.CanConnectTop)
                    tile.TopCompatibleTileIndices.Add(cell.TopCell.TileIndex);

                if(cell.CanConnectRight)
                    tile.RightCompatibleTileIndices.Add(cell.RightCell.TileIndex);

                if(cell.CanConnectBottom)
                    tile.BottomCompatibleTileIndices.Add(cell.BottomCell.TileIndex);

                if(cell.CanConnectLeft)
                    tile.LeftCompatibleTileIndices.Add(cell.LeftCell.TileIndex);
            }
        }
    }
}
