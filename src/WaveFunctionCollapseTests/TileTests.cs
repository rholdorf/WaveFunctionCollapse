using FluentAssertions;
using Microsoft.Xna.Framework;
using WaveFunctionCollapse;
using WaveFunctionCollapse.Tiles;

namespace WaveFunctionCollapseTests;

public class TileTests
{
    [Fact]
    public void TileDetectsEdgeColors()
    {
        // Arrange
        var rectangle = new Rectangle(0, 0, 4, 4);
        var colors = new[]
        {
            new Color(1, 1, 1, 1), new Color(2, 2, 2, 2), new Color(3, 3, 3, 3), new Color(4, 4, 4, 4),
            new Color(5, 5, 5, 5), new Color(6, 6, 6, 6), new Color(7, 7, 7, 7), new Color(8, 8, 8, 8),
            new Color(9, 9, 9, 9), new Color(10, 10, 10, 10), new Color(11, 11, 11, 11), new Color(12, 12, 12, 12),
            new Color(13, 13, 13, 13), new Color(14, 14, 14, 14), new Color(15, 15, 15, 15), new Color(16, 16, 16, 16)
        };

        // Act
        var tile = new Tile(rectangle, colors, 0);

        // Assert
        Assert.Equal(new[] { new Color(1, 1, 1, 1), new Color(2, 2, 2, 2), new Color(3, 3, 3, 3), new Color(4, 4, 4, 4) }, tile.TopEdge);
        Assert.Equal(new[] { new Color(1, 1, 1, 1), new Color(5, 5, 5, 5), new Color(9, 9, 9, 9), new Color(13, 13, 13, 13) }, tile.LeftEdge);
        Assert.Equal(new[] { new Color(4, 4, 4, 4), new Color(8, 8, 8, 8), new Color(12, 12, 12, 12), new Color(16, 16, 16, 16) }, tile.RightEdge);
        Assert.Equal(new[] { new Color(13, 13, 13, 13), new Color(14, 14, 14, 14), new Color(15, 15, 15, 15), new Color(16, 16, 16, 16) }, tile.BottomEdge);
    }

    [Fact]
    public void TilesFindValidConnections()
    {
        // Arrange
        var tile0 = new Tile(new Rectangle(0, 0, 4, 4), new[]
        {
            new Color(1, 1, 1, 1), new Color(2, 2, 2, 2), new Color(3, 3, 3, 3), new Color(4, 4, 4, 4),
            new Color(5, 5, 5, 5), new Color(6, 6, 6, 6), new Color(7, 7, 7, 7), new Color(8, 8, 8, 8),
            new Color(9, 9, 9, 9), new Color(10, 10, 10, 10), new Color(11, 11, 11, 11), new Color(12, 12, 12, 12),
            new Color(13, 13, 13, 13), new Color(14, 14, 14, 14), new Color(15, 15, 15, 15), new Color(16, 16, 16, 16)
        }, 0);

        var tile1 = new Tile(new Rectangle(0, 0, 4, 4), new[]
        {
            new Color(5, 5, 5, 5), new Color(6, 6, 6, 6), new Color(7, 7, 7, 7), new Color(8, 8, 8, 8),
            new Color(9, 9, 9, 9), new Color(10, 10, 10, 10), new Color(11, 11, 11, 11), new Color(12, 12, 12, 12),
            new Color(13, 13, 13, 13), new Color(14, 14, 14, 14), new Color(15, 15, 15, 15), new Color(16, 16, 16, 16),
            new Color(1, 1, 1, 1), new Color(2, 2, 2, 2), new Color(3, 3, 3, 3), new Color(4, 4, 4, 4)
        }, 1);

        var tile2 = new Tile(new Rectangle(0, 0, 4, 4), new[]
        {
            new Color(1, 0, 0, 1), new Color(2, 2, 2, 2), new Color(3, 3, 3, 3), new Color(0, 4, 4, 4),
            new Color(2, 5, 0, 5), new Color(6, 6, 6, 6), new Color(7, 7, 7, 7), new Color(0, 8, 8, 8),
            new Color(3, 9, 0, 9), new Color(10, 10, 10, 10), new Color(11, 11, 11, 11), new Color(0, 12, 12, 12),
            new Color(4, 13, 0, 13), new Color(14, 0, 0, 14), new Color(15, 15, 15, 15), new Color(0, 16, 16, 16)
        }, 2);

        var tile3 = new Tile(new Rectangle(0, 0, 4, 4), new[]
        {
            new Color(1, 1, 1, 1), new Color(2, 2, 2, 2), new Color(3, 3, 3, 3), new Color(4, 4, 4, 1),
            new Color(2, 2, 2, 2), new Color(6, 6, 6, 6), new Color(7, 7, 7, 7), new Color(8, 8, 8, 2),
            new Color(3, 3, 3, 3), new Color(10, 10, 10, 10), new Color(11, 11, 11, 11), new Color(12, 12, 12, 3),
            new Color(4, 4, 4, 4), new Color(14, 14, 14, 14), new Color(15, 15, 15, 15), new Color(16, 16, 16, 4)
        }, 3);

        var tile4 = new Tile(new Rectangle(0, 0, 4, 4), new[]
        {
            new Color(1, 1, 1, 1), new Color(2, 2, 2, 2), new Color(3, 3, 3, 3), new Color(1, 1, 1, 1),
            new Color(5, 5, 5, 5), new Color(6, 6, 6, 6), new Color(7, 7, 7, 7), new Color(2, 2, 2, 2),
            new Color(9, 9, 9, 9), new Color(10, 10, 10, 10), new Color(11, 11, 11, 11), new Color(3, 3, 3, 3),
            new Color(13, 13, 13, 13), new Color(14, 14, 14, 14), new Color(15, 15, 15, 15), new Color(4, 4, 4, 4)
        }, 4);

        var tile5 = new Tile(new Rectangle(0, 0, 4, 4), new[]
        {
            new Color(13, 13, 13, 13), new Color(14, 14, 14, 14), new Color(15, 15, 15, 15), new Color(16, 16, 16, 16),
            new Color(5, 5, 5, 5), new Color(6, 6, 6, 6), new Color(7, 7, 7, 7), new Color(2, 2, 2, 2),
            new Color(9, 9, 9, 9), new Color(10, 10, 10, 10), new Color(11, 11, 11, 11), new Color(3, 3, 3, 3),
            new Color(13, 13, 13, 13), new Color(14, 14, 14, 14), new Color(15, 15, 15, 15), new Color(4, 4, 4, 4)
        }, 5);
        
        var tile6 = new Tile(new Rectangle(0, 0, 4, 4), new[]
        {
            new Color(13, 13, 13, 13), new Color(14, 14, 14, 14), new Color(15, 15, 15, 15), new Color(16, 16, 16, 16),
            new Color(0, 5, 5, 5), new Color(0, 6, 6, 6), new Color(0, 7, 7, 7), new Color(0, 2, 2, 2),
            new Color(9, 0, 9, 9), new Color(0, 10, 10, 10), new Color(0, 11, 11, 11), new Color(0, 3, 3, 3),
            new Color(13, 13, 0, 13), new Color(0, 14, 14, 14), new Color(0, 15, 15, 15), new Color(0, 4, 4, 4)
        }, 6);        

        var tileCollection = new[] { tile0, tile1, tile2, tile3, tile4, tile5, tile6 };
        
        // Act
        tileCollection.FindValidConnections();
        
        // Assert
        // tile0 top connects with tile1 bottom
        tile0.TopEdgeConnections.Should().Contain(1);
        // tile0 bottom connects with tile5 top and tile6 top
        tile0.BottomEdgeConnections.Should().Contain(5);
        tile0.BottomEdgeConnections.Should().Contain(6);
        // tile0 top has only one connection
        tile0.TopEdgeConnections.Count.Should().Be(1);
        // tile0 bottom has two connections
        tile0.BottomEdgeConnections.Count.Should().Be(2);
        // tile0 left doesn't connect with any other tile
        tile0.LeftEdgeConnections.Should().BeEmpty();
        // tile0 right doesn't connect with any other tile
        tile0.RightEdgeConnections.Should().BeEmpty();
        // tile3 left connects with tile4 right
        tile3.LeftEdgeConnections.Should().Contain(4);
        // tile3 left has only one connection
        tile3.LeftEdgeConnections.Count.Should().Be(1);
        // tile2 doesn't connect with any other tile
        tile2.TopEdgeConnections.Should().BeEmpty();
        tile2.BottomEdgeConnections.Should().BeEmpty();
        tile2.LeftEdgeConnections.Should().BeEmpty();
        tile2.RightEdgeConnections.Should().BeEmpty();
    }
}