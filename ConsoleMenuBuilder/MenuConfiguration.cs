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
    public string Selector = "-> ";
    public bool ClearConsole = true;
    public bool EnableWriteTitle = true;
    public Encoding InputEncoding = Console.InputEncoding;
    public Encoding OutputEncoding = Console.OutputEncoding;
    public ConsoleColor ItemBackgroundColor = Console.BackgroundColor;
    public ConsoleColor ItemForegroundColor = Console.ForegroundColor;
    public ConsoleColor SelectedItemBackgroundColor = Console.ForegroundColor;
    public ConsoleColor SelectedItemForegroundColor = Console.BackgroundColor;
    public Action<string> WriteTitleAction = title => TitleAction(title);

    private static void ItemAction(MenuItem item)
    {
        Console.WriteLine("{0}", item.Name);
    }

    private static void TitleAction(string title)
    {
        Console.WriteLine(" -------------------- ");
        Console.WriteLine($"|      {title}      |");
        Console.WriteLine(" -------------------- ");
    }
}