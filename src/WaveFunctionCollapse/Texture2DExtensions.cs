using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WaveFunctionCollapse.Tiles;

namespace WaveFunctionCollapse;

public static class Texture2DExtensions
{
    public static Rectangle[][] MapRectangles(this Texture2D texture, int tileWidth, int tileHeight)
    {
        var width = texture.Width / tileWidth;
        var height = texture.Height / tileHeight;
        var ret = new Rectangle[width][];
        for (var i = 0; i < width; i++)
        {
            ret[i] = new Rectangle[height];
        }

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                ret[x][y] = new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight);
            }
        }

        return ret;
    }    
    
    public static CellCollection FindUniqueTiles(this Texture2D texture, Rectangle[][] tileMap, int widthInCells, int heightInCells, Random random)
    {
        var uniqueTiles = new List<Tile>();
        var cellMap = new CellCollection(widthInCells, heightInCells, uniqueTiles, random);
        var uniqueTilesColorData = new List<Color[]>();

        for (var y = 0; y < heightInCells; y++)
        {
            for (var x = 0; x < widthInCells; x++)
            {
                var rectangle = tileMap[x][y];
                var currentTileColorData = new Color[rectangle.Width * rectangle.Height];
                texture.GetData(0, rectangle, currentTileColorData, 0, currentTileColorData.Length);
                var existsAtPosition = Find(currentTileColorData, uniqueTilesColorData);
                if (existsAtPosition == -1)
                {
                    uniqueTilesColorData.Add(currentTileColorData);
                    var index = uniqueTilesColorData.Count - 1;
                    uniqueTiles.Add(new Tile(rectangle, index));
                    cellMap.Cells[x][y].Index = index;
                }
                else
                {
                    cellMap.Cells[x][y].Index = existsAtPosition;
                }                
            }
        }

        var ret = uniqueTiles.ToArray();
        cellMap.SetUniqueTiles(uniqueTiles);
        Console.WriteLine($"{texture.Name} unique tiles: {ret.Length}");
        return cellMap;
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
