using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WaveFunctionCollapse;

public class Tiles
{
    private readonly Texture2D[] _entries;

    private readonly int _max = 220;
    
    public Tiles(Texture2D image, int tileWidth, int tileHeight, int tileOffsetVertical, int tileOffsetHorizontal)
    {
        var tw = (int)Math.Round((double)image.Width / (tileWidth + tileOffsetVertical));
        var th = (int)Math.Round((double)image.Height / (tileHeight + tileOffsetHorizontal));
        var tileCount = tw * th;
        _entries = new Texture2D[tileCount];
        var entryCount = 0;

        for (var y = 0; y < image.Height; y += tileHeight + tileOffsetHorizontal)
        {
            for (var x = 0; x < image.Width; x += tileWidth + tileOffsetVertical)
            {
                var tileData = new Color[tileHeight * tileWidth];
                image.GetData(0, new Rectangle(x, y, tileWidth, tileHeight), tileData, 0, tileData.Length);
                _entries[entryCount] = new Texture2D(image.GraphicsDevice, tileWidth, tileHeight);
                _entries[entryCount].SetData(tileData);
                entryCount++;
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch, Rectangle position)
    {
        for (var i = 0; i < _entries.Length; i++)
        {
            var tile = _entries[i];
            position.Height = tile.Height;
            position.Width = tile.Width;

            spriteBatch.Draw(tile, position, Color.White);
            position.X += tile.Width;
            if (position.X <= _max/*GraphicsDevice.Viewport.Width*/) 
                continue;
            position.X = 0;
            position.Y += tile.Height;
        }        
    }
}
