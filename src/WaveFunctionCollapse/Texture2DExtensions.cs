using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WaveFunctionCollapse.Tiles;

namespace WaveFunctionCollapse;

public static class Texture2DExtensions
{
    public static Rectangle[][] SplitTileSetInTileRectangles(this Texture2D texture, int tileWidth, int tileHeight)
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
    
    public static (Tile[], CellMap) FindUniqueTiles(this Texture2D texture, Rectangle[][] tileMap, int widthInCells, int heightInCells)
    {
        var uniqueTiles = new List<Tile>();
        var cellMap = new CellMap(widthInCells, heightInCells); 
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
                    uniqueTiles.Add(new Tile(rectangle, currentTileColorData, index));
                    cellMap.Cells[x][y] = index;
                }
                else
                {
                    cellMap.Cells[x][y] = existsAtPosition;
                }                
            }
        }

        Console.WriteLine($"{texture.Name} unique tiles: {uniqueTilesColorData.Count}");
        return (uniqueTiles.ToArray(), cellMap);
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
