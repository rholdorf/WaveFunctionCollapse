namespace WaveFunctionCollapse;

public static class CellExtensions
{
    public static bool CanCollapse(this Cell cell)
    {
        if (cell is null) return false;
        return !cell.Collapsed && cell.Entropy.Count > 0;
    }
}
