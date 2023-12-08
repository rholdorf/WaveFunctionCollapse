using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WaveFunctionCollapse.Tiles;

public class TileMaker
{
    private readonly Rectangle[][] _allRectanglesInTileSet;
    private readonly int _max;
    private readonly Texture2D _texture;
    private readonly Tile[] _uniqueTiles;
    private readonly int _widthInCells;
    private readonly int _heightInCells;
    private readonly CellMap _cellMap;
    
    public TileMaker(Texture2D texture, int tileWidth, int tileHeight)
    {
        _texture = texture;
        _widthInCells = _texture.Width / tileWidth;
        _heightInCells = _texture.Height / tileHeight;
        _max = _texture.Width;
        _allRectanglesInTileSet = texture.SplitTileSetInTileRectangles(tileWidth, tileHeight);
        
        (_uniqueTiles, _cellMap) = texture.FindUniqueTiles(_allRectanglesInTileSet, _widthInCells, _heightInCells);
        _uniqueTiles.FindValidConnections();
    }
    
    public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle)
    {
        for (var y = 0; y < _cellMap.Height; y++)
        {
            for (var x = 0; x < _cellMap.Width; x++)
            {
                var index = _cellMap.Cells[x][y];
                var sourceRectangle = _allRectanglesInTileSet[x][y];
                destinationRectangle.Height = sourceRectangle.Height;
                destinationRectangle.Width = sourceRectangle.Width;
        
                spriteBatch.Draw(_texture, destinationRectangle, _uniqueTiles[index].SourceRectangle, Color.White);
            
                // move to next tile position
                destinationRectangle.X += sourceRectangle.Width;
                if (destinationRectangle.X >= _max)
                {
                    destinationRectangle.X = 0;
                    destinationRectangle.Y += sourceRectangle.Height;
                }                
            }
        }
    }
    
    private void HalfBakedCollapse()
    {
        var bidimensionalArray = new int[_widthInCells, _heightInCells];
        bidimensionalArray.Fill(-1);
        var random = new Random(1);
        bidimensionalArray[0, 0] = random.Next(_uniqueTiles.Length);
        PopulateNeighbours(ref bidimensionalArray, 0, 0, random);
        
        for (var y = 0; y < _heightInCells; y++)
        {
            for (var x = 0; x < _widthInCells; x++)
            {
                PopulateNeighbours(ref bidimensionalArray, x, y, random);
            }
        }

        // transpose bidimensional array into linear array
        for (var y = 0; y < _heightInCells; y++)
        {
            for (var x = 0; x < _widthInCells; x++)
            {
                _cellMap.Cells[x][y] = bidimensionalArray[x, y];
            }
        }
    }

    private void PopulateNeighbours(ref int[,] bidimensionalArray, int x, int y, Random random)
    {
        var canConnectTop = y > 0 && bidimensionalArray[x, y - 1] == -1;
        var canConnectRight = x < _widthInCells - 1 && bidimensionalArray[x + 1, y] == -1;
        var canConnectBottom = y < _heightInCells - 1 && bidimensionalArray[x, y + 1] == -1;
        var canConnectLeft = x > 0 && bidimensionalArray[x - 1, y] == -1;

        var index = bidimensionalArray[x, y];
        if(index == -1)
            index = random.Next(_uniqueTiles.Length); // half baked solution
        
        var currentTile = _uniqueTiles[index];
        if (canConnectTop && currentTile.TopEdgeConnections.Any())
        {
            bidimensionalArray[x, y - 1] = currentTile.PickRandomTopEdgeConnection(random);
        }
        if (canConnectRight && currentTile.RightEdgeConnections.Any())
        {
            bidimensionalArray[x + 1, y] = currentTile.PickRandomRightEdgeConnection(random);
        }
        if (canConnectBottom && currentTile.BottomEdgeConnections.Any())
        {
            bidimensionalArray[x, y + 1] = currentTile.PickRandomBottomEdgeConnection(random);
        }
        if (canConnectLeft && currentTile.LeftEdgeConnections.Any())
        {
            bidimensionalArray[x - 1, y] = currentTile.PickRandomLeftEdgeConnection(random);
        }        
    }
}
