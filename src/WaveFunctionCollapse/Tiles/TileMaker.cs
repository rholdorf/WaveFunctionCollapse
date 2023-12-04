using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WaveFunctionCollapse.Tiles;

public class TileMaker
{
    private readonly Rectangle[] _allRectanglesInTileSet;
    private readonly int[] _tileMap;
    private readonly int _max;
    private readonly Texture2D _texture;
    private readonly Tile[] _uniqueTiles;
    private Random _random = new(1);
    
    public TileMaker(Texture2D texture, int tileWidth, int tileHeight)
    {
        _texture = texture;
        _allRectanglesInTileSet = texture.SplitTileSetInTileRectangles(tileWidth, tileHeight);
        _max = _texture.Width;
        (_uniqueTiles, _tileMap) = texture.FindUniqueTiles(_allRectanglesInTileSet);
        FindValidConnections();
    }
    
    public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle)
    {
        for (var i = 0; i < _tileMap.Length; i++)
        {
            var index = _tileMap[i];
            var sourceRectangle = _allRectanglesInTileSet[index];
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
    
    public void DrawFullTileSet(SpriteBatch spriteBatch, Rectangle destinationRectangle)
    {
        for (var i = 0; i < _allRectanglesInTileSet.Length; i++)
        {
            var sourceRectangle = _allRectanglesInTileSet[i];
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




    private void FindValidConnections()
    {
        for(var i=0; i<_uniqueTiles.Length; i++)
        {
            var tile = _uniqueTiles[i];
            for(var j=0; j<_uniqueTiles.Length; j++)
            {
                if (i == j) 
                    continue;
                
                var otherTile = _uniqueTiles[j];
                if (tile.TopEdgeColors.SequenceEqual(otherTile.BottomEdgeColors))
                    tile.TopEdgeValidConnections.Add(otherTile);
                if (tile.RightEdgeColors.SequenceEqual(otherTile.LeftEdgeColors))
                    tile.RightEdgeValidConnections.Add(otherTile);
                if (tile.BottomEdgeColors.SequenceEqual(otherTile.TopEdgeColors))
                    tile.BottomEdgeValidConnections.Add(otherTile);
                if (tile.LeftEdgeColors.SequenceEqual(otherTile.RightEdgeColors))
                    tile.LeftEdgeValidConnections.Add(otherTile);
            }
        }
    }




}
