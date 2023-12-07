namespace WaveFunctionCollapse;

public class CellMap
{
    public int Width { get; }
    public int Height { get; }

    public readonly int[][] Cells;
    
    
    public CellMap(int width, int height)
    {
        Width = width;
        Height = height;
        Cells = new int[width][];
        for (var i = 0; i < width; i++)
        {
            Cells[i] = new int[height];
        }


    }

    
    
}
