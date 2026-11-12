using System.Collections;
namespace ConsoleMenuBuilder;

public class Menu : IEnumerable
{
    private readonly IConsole console;
    private readonly ItemsCollection menuItems;
    private readonly CloseTrigger closeTrigger;
    private MenuConfiguration config = new MenuConfiguration();
    private Menu? parent = null;
    private bool isShown = false;

    public Menu()
        : this(new SystemConsole())
    {
    }

    internal Menu(IConsole console)
    {
        this.console = console ?? throw new ArgumentNullException(nameof(console));
        this.menuItems = new ItemsCollection();
        this.closeTrigger = new CloseTrigger();
    }

    public IReadOnlyList<MenuItem> Items => this.menuItems.Items;

    public MenuItem CurrentItem
    {
        get => this.menuItems.CurrentItem;
        set => this.menuItems.CurrentItem = value;
    }

    /// <summary>
    /// Marker action that closes the menu when its item is chosen.
    /// Use it as <c>menu.Add("Back", Menu.Close)</c>; don't call it directly.
    /// </summary>
    public static Task Close(CancellationToken cancellationToken) => throw new InvalidOperationException("Don't run this method directly. Just pass a reference to this method.");

    public void CloseMenu()
    {
        this.closeTrigger.SetOn();
    }

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

    public Menu Add(string name, Action<Menu> action)
    {
        if (name == null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        this.menuItems.Add(name, (_) =>
        {
            action(this);
            return Task.CompletedTask;
        });
        return this;
    }

    public Menu Add(string name, Func<CancellationToken, Task> action)
    {
        if (name == null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        // Stored unwrapped so that MenuDisplay can recognise Menu.Close.
        this.menuItems.Add(name, action);
        return this;
    }

    public Menu Configure(Action<MenuConfiguration> configure)
    {
        if (configure is null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        configure.Invoke(this.config);
        return this;
    }

    public Menu Configure(MenuConfiguration config)
    {
        if (config is null)
        {
            throw new ArgumentNullException(nameof(config));
        }

        this.config = new MenuConfiguration(config);
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
            this.console,
            this.config,
            this.closeTrigger).ShowAsync(cancellationToken);
    }

    public IEnumerator GetEnumerator() => this.Items.GetEnumerator();
}