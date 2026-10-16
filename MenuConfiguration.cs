namespace ConsoleMenuBuilder;

public class MenuConfiguration
{
    public MenuConfiguration()
    {
        this.WriteItemAction = item => ItemAction(item);
    }
    internal MenuConfiguration(MenuConfiguration config) : this()
    {

    }

    public Action<MenuItem> WriteItemAction;

    private static void ItemAction(MenuItem item)
    {
        Console.WriteLine("[{0}] [1]", item.Index + 1, item.Name);
    }
}