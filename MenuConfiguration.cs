using System.Text;

namespace ConsoleMenuBuilder;

public class MenuConfiguration
{
    public MenuConfiguration()
    {
        this.WriteItemAction = item => ItemAction(item);
    }
    internal MenuConfiguration(MenuConfiguration config) : this()
    {
        this.ArgsPreselectedItemsKey = config.ArgsPreselectedItemsKey;
        this.ArgsPrecelectedItemsValueSeperator = config.ArgsPrecelectedItemsValueSeperator;

        this.ClearConsole = config.ClearConsole;
        this.EnableWriteTitle = config.EnableWriteTitle;
        this.InputEncoding = config.InputEncoding;
        this.OutputEncoding = config.OutputEncoding;
        this.ItemBackgroundColor = config.ItemBackgroundColor;
        this.ItemForegroundColor = config.ItemForegroundColor;
        this.WriteItemAction = config.WriteItemAction;
        this.WriteTitleAction = config.WriteTitleAction;
    }

    public string ArgsPreselectedItemsKey = "--menu-select=";
    public char ArgsPrecelectedItemsValueSeperator = '.';

    public Action<MenuItem> WriteItemAction;
    public string Title = "My Menu";
    public bool ClearConsole = true;
    public bool EnableWriteTitle = false;
    public Encoding InputEncoding = Console.InputEncoding;
    public Encoding OutputEncoding = Console.OutputEncoding;
    public ConsoleColor ItemBackgroundColor = Console.BackgroundColor;
    public ConsoleColor ItemForegroundColor = Console.ForegroundColor;
    public ConsoleColor SelectedItemBackgroundColor = Console.BackgroundColor;
    public ConsoleColor SelectedItemForegroundColor = Console.ForegroundColor;
    public Action<string> WriteTitleAction = title => Console.WriteLine(title);

    private static void ItemAction(MenuItem item)
    {
        Console.WriteLine("[{0}] [1]", item.Index + 1, item.Name);
    }
}