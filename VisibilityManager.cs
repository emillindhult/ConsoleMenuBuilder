namespace ConsoleMenuBuilder;

internal sealed class VisibilityManager
{
    private readonly bool[] visibility;

    public VisibilityManager(int size)
    {
        bool[] visibility = new bool[size];
        
        for (int i = 0; i < visibility.Length; i++)
        {
            visibility[i] = true;
        }

        this.visibility = visibility;
    }

    public bool IsVisibleAt(int index)
    {
        return this.visibility[index];
    }

    public int IndexOfPreviousVisibleItem(int startIndex)
    {
        int idx = -1;
        if (startIndex - 1 >= 0)
        {
            idx = Array.LastIndexOf(this.visibility, true, startIndex - 1);
        }

        if (idx == -1)
        {
            idx = Array.LastIndexOf(this.visibility, true, this.visibility.Length - 1);
        }

        if (idx == -1)
        {
            idx = startIndex;
        }

        return idx;
    }

    public int IndexOfNextVisibleItem(int startIndex)
    {
        int idx = -1;
        if (startIndex + 1 < this.visibility.Length)
        {
            idx = Array.LastIndexOf(this.visibility, true, startIndex + 1);
        }

        if (idx == -1)
        {
            idx = Array.LastIndexOf(this.visibility, true, 0);
        }

        if (idx == -1)
        {
            idx = startIndex;
        }

        return idx;
    }
}