namespace ConsoleMenuBuilder;

internal sealed class ItemsCollection
{
    private readonly List<MenuItem> menuItems = new List<MenuItem>();
    private readonly MenuConfiguration config = new MenuConfiguration();
    private int? selectedIndex;
    private string? selectedName;
    private int currentItemIndex;

    public ItemsCollection()
    {
    }

    public ItemsCollection(string[] args, int level)
    {
        this.SetSelectedItems(args, level);
    }

    public List<MenuItem> Items => this.menuItems;

    public void SetSelectedItems(string[] args, int level)
    {
        var arg = Array.Find(args, a => a.StartsWith(this.config.ArgsPreselectedItemsKey));
        this.SetSelectedItems(level, this.config.ArgsPreselectedItemsKey, ref arg);
    }

    private void SetSelectedItems(int level, string paramKey, ref string? arg)
    {
        if (arg == null)
        {
            return;
        }

        arg = arg.Replace(paramKey, string.Empty).Trim();
        var items = arg.SplitItems(this.config.ArgsPrecelectedItemsValueSeperator, '\'');
        
    }
}