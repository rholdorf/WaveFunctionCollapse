using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WaveFunctionCollapse;

public class Tiles
{
    private readonly Rectangle[] _rectangles;
    private readonly int[] _indexes;
    private readonly int _max = 220;
    private readonly Texture2D _texture;
    
    public Tiles(Texture2D texture, int tileWidth, int tileHeight, int tileOffsetVertical, int tileOffsetHorizontal)
    {
        _texture = texture;
        var tw = (int)Math.Round((double)texture.Width / (tileWidth + tileOffsetVertical));
        var th = (int)Math.Round((double)texture.Height / (tileHeight + tileOffsetHorizontal));
        var tileCount = tw * th;
        _rectangles = new Rectangle[tileCount];
        _indexes = new int[tileCount];
        var entryCount = 0;

        for (var y = 0; y < texture.Height; y += tileHeight + tileOffsetHorizontal)
        {
            for (var x = 0; x < texture.Width; x += tileWidth + tileOffsetVertical)
            {
                _rectangles[entryCount] = new Rectangle(x, y, tileWidth, tileHeight);
                entryCount++;
            }
        }

        FindUniqueTiles();
    }

    public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle)
    {
        for (var i = 0; i < _rectangles.Length; i++)
        {
            var tileIndex = _indexes[i];
            //Console.WriteLine($"Tile index: {tileIndex}");
            var sourceRectangle = _rectangles[i /*tileIndex*/];
            destinationRectangle.Height = sourceRectangle.Height;
            destinationRectangle.Width = sourceRectangle.Width;

            spriteBatch.Draw(_texture, destinationRectangle, sourceRectangle, Color.White);
            
            // move to next tile position
            destinationRectangle.X += sourceRectangle.Width;
            if (destinationRectangle.X <= _max) 
                continue;
            destinationRectangle.X = 0;
            destinationRectangle.Y += sourceRectangle.Height;
        }        
    }
    
    private void FindUniqueTiles()
    {
        var uniqueTiles = new List<Color[]>();
        for (var index = 0; index < _rectangles.Length; index++)
        {
            var rectangle = _rectangles[index];
            var tileData = new Color[rectangle.Width * rectangle.Height];
            _texture.GetData(0, rectangle, tileData, 0, tileData.Length);
            var exists = GetIndexFor(tileData, uniqueTiles);
            if (exists == -1)
            {
                uniqueTiles.Add(tileData);
                _indexes[index] = uniqueTiles.Count - 1;
            }
            else
            {
                _indexes[index] = exists;
            }
        }

        Console.WriteLine($"{_texture.Name} unique tiles: {uniqueTiles.Count}");
    }
    
    private static int GetIndexFor(IReadOnlyList<Color> tileData, IReadOnlyList<Color[]> uniqueTiles)
    {
        for (var index = 0; index < uniqueTiles.Count; index++)
        {
            var existingTile = uniqueTiles[index];
            var isEqual = true;
            for (var i = 0; i < tileData.Count; i++)
            {
                if (existingTile[i] == tileData[i]) 
                    continue;
                
                isEqual = false;
                break; // at least one pixel is different
            }

            if (isEqual) return index;
        }

        return -1;
    }
}
