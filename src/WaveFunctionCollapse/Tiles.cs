using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WaveFunctionCollapse;

public class Tiles
{
    private readonly Rectangle[] _rectangles;
    private readonly int[] _indexes;
    private readonly int _max;
    private readonly Texture2D _texture;
    private static Texture2D _pixel;
    private List<Texture2D> _uniqueTiles = new ();
    
    public Tiles(Texture2D texture, int tileWidth, int tileHeight)
    {
        _texture = texture;
        _rectangles = DivideInRectangles(texture, tileWidth, tileHeight);
        _indexes = new int[_rectangles.Length];
        _max = _texture.Width;

        if (_pixel == null)
        {
            _pixel = new Texture2D(texture.GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
        }
        
        FindUniqueTiles();
    }
    
    private static Rectangle[] DivideInRectangles(Texture2D texture, int tileWidth, int tileHeight)
    {
        var tileCount = (texture.Width / tileWidth) * (texture.Height / tileHeight);
        var ret = new Rectangle[tileCount];
        var count = 0;

        for (var y = 0; y < texture.Height; y += tileHeight)
        {
            for (var x = 0; x < texture.Width; x += tileWidth)
            {
                ret[count] = new Rectangle(x, y, tileWidth, tileHeight);
                count++;
            }
        }

        return ret;
    }

    public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle)
    {
        //DrawIndexed(spriteBatch, destinationRectangle);
        DrawUniqueTilesFound(spriteBatch, destinationRectangle);
        //DrawEveryTile(spriteBatch, destinationRectangle);
    }
    
    private void DrawEveryTile(SpriteBatch spriteBatch, Rectangle destinationRectangle)
    {
        for (var i = 0; i < _rectangles.Length; i++)
        {
            var sourceRectangle = _rectangles[i];
            destinationRectangle.Height = sourceRectangle.Height;
            destinationRectangle.Width = sourceRectangle.Width;
        
            spriteBatch.Draw(_texture, destinationRectangle, sourceRectangle, Color.White);
            
            // move to next tile position
            destinationRectangle.X += sourceRectangle.Width;
            if (destinationRectangle.X < _max) 
                continue;
            destinationRectangle.X = 0;
            destinationRectangle.Y += sourceRectangle.Height;
        }
    }    
    
    private void DrawIndexed(SpriteBatch spriteBatch, Rectangle destinationRectangle)
    {
        for (var i = 0; i < _indexes.Length; i++)
        {
            var tileIndex = _indexes[i];
            var sourceRectangle = _rectangles[tileIndex];
            destinationRectangle.Height = sourceRectangle.Height;
            destinationRectangle.Width = sourceRectangle.Width;
        
            spriteBatch.Draw(_uniqueTiles[tileIndex], destinationRectangle, Color.White);
            
            // move to next tile position
            destinationRectangle.X += sourceRectangle.Width;
            if (destinationRectangle.X < _max) 
                continue;
            destinationRectangle.X = 0;
            destinationRectangle.Y += sourceRectangle.Height;
        }
    }    
    
    private void DrawUniqueTilesFound(SpriteBatch spriteBatch, Rectangle destinationRectangle)
    {
        for (var i = 0; i < _uniqueTiles.Count; i++)
        {
            var tile = _uniqueTiles[i];
            destinationRectangle.Height = tile.Height;
            destinationRectangle.Width = tile.Width;
            spriteBatch.Draw(_uniqueTiles[i], destinationRectangle, Color.White);
            
            // move to next tile position
            destinationRectangle.X += destinationRectangle.Width;
            if (destinationRectangle.X < _max) 
                continue;
            destinationRectangle.X = 0;
            destinationRectangle.Y += destinationRectangle.Height;
        }
    }    
    
    private void FindUniqueTiles()
    {
        var uniqueTiles = new List<Color[]>();
        for (var index = 0; index < _rectangles.Length; index++)
        {
            var rectangle = _rectangles[index];
            var buffer = new Color[rectangle.Width * rectangle.Height];
            _texture.GetData(0, rectangle, buffer, 0, buffer.Length);
            var existsAtPosition = Find(buffer, uniqueTiles);
            if (existsAtPosition == -1)
            {
                uniqueTiles.Add(buffer);
                var tile = new Texture2D(_texture.GraphicsDevice, rectangle.Width, rectangle.Height);
                tile.SetData(buffer);
                tile.Name = $"Tile_{index}";
                _uniqueTiles.Add(tile);
                _indexes[index] = uniqueTiles.Count - 1;
            }
            else
            {
                _indexes[index] = existsAtPosition;
            }
        }

        Console.WriteLine($"{_texture.Name} unique tiles: {uniqueTiles.Count}");
    }
    
    private static int Find(IReadOnlyList<Color> tileData, IReadOnlyList<Color[]> uniqueTiles)
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
