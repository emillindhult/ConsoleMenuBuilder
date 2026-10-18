using System.Collections;
namespace ConsoleMenuBuilder;

public class Menu : IEnumerable
{
    private IConsole Console = new SystemConsole();
    private readonly ItemsCollection menuItems;
    private readonly CloseTrigger closeTrigger;
    private MenuConfiguration config = new MenuConfiguration();
    private Menu? parent = null;
    private bool isShown = false;
    private MenuConfiguration config = new MenuConfiguration();
    private Menu? parent = null;

    public Menu()
    {
        this.menuItems = new ItemsCollection();
        this.closeTrigger = new CloseTrigger();
    }

    public Menu(string[] args, int level)
    {
        if (args == null)
        {
            throw new ArgumentNullException(nameof(args));
        }

        if (level < 0)
        {
            throw new ArgumentException("Cannot be less than 0.", nameof(level));
        }

        this.menuItems = new ItemsCollection(args, level);
        this.closeTrigger = new CloseTrigger();
    }

    public IReadOnlyList<MenuItem> Items => this.menuItems.Items;

    public MenuItem CurrentItem
    {
        get => this.menuItems.CurrentItem;
        set => this.menuItems.CurrentItem = value;
    }

    private IReadOnlyList<string> Titles
    {
        get
        {
            Menu? current = this;
            List<string> titles = new();
            while (current != null)
            {
                titles.Add(current.config.Title ?? "");
                current = current.parent;
            }

            titles.Reverse();
            return titles;
        }
    }

    public static Task Close(CancellationToken cancellationToken) => throw new InvalidOperationException("Don't run this method directly. Just passa a reference to this method.");

    public Menu Add(string name, Action action)
    {
        if (name == null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        if (action.Target is Menu child && action == child.Show)
        {
            child.parent = this;
        }

        this.menuItems.Add(name, (_) =>
        {
            action();
            return Task.CompletedTask;
        });
        return this;
    }

    public void Show()
    {
        ShowAsync(CancellationToken.None).GetAwaiter().GetResult();
    }

    public async Task ShowAsync(CancellationToken cancellationToken = default)
    {
        if (isShown)
        {
            this.menuItems.UnsetSelectedIndex();
        }

        isShown = true;

        await new MenuDisplay(
            this.menuItems,
            this.Console,
            new List<string>(this.Titles),
            this.config,
            this.closeTrigger).ShowAsync(cancellationToken);
    }

    public IEnumerator GetEnumerator() => this.Items.GetEnumerator();
}