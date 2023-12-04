using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WaveFunctionCollapse.Tiles;

namespace WaveFunctionCollapse;

public static class Texture2DExtensions
{
    public static Rectangle[] SplitTileSetInTileRectangles(this Texture2D texture, int tileWidth, int tileHeight)
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
    
    public static (Tile[], int[]) FindUniqueTiles(this Texture2D texture, Rectangle[] tileMap)
    {
        var uniqueTiles = new List<Tile>();
        var fullTileSetIndexes = new int[tileMap.Length]; 
        var uniqueTilesColorData = new List<Color[]>();
        for (var i = 0; i < tileMap.Length; i++)
        {
            var rectangle = tileMap[i];
            var currentTileColorData = new Color[rectangle.Width * rectangle.Height];
            texture.GetData(0, rectangle, currentTileColorData, 0, currentTileColorData.Length);
            var existsAtPosition = Find(currentTileColorData, uniqueTilesColorData);
            if (existsAtPosition == -1)
            {
                uniqueTilesColorData.Add(currentTileColorData);
                uniqueTiles.Add(new Tile(rectangle, currentTileColorData));
                fullTileSetIndexes[i] = uniqueTilesColorData.Count - 1;
            }
            else
            {
                fullTileSetIndexes[i] = existsAtPosition;
            }
        }
        
        Console.WriteLine($"{texture.Name} unique tiles: {uniqueTilesColorData.Count}");
        return (uniqueTiles.ToArray(), fullTileSetIndexes);
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
