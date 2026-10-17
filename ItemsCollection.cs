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

    private void SetSelectedItems(string[] args, int level)
    {
        var arg = Array.Find(args, a => a.StartsWith(this.config.ArgsPreselectedItemsKey));
        this.SetSelectedItems(this.config.ArgsPreselectedItemsKey, level, ref arg);
    }
}