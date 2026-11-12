namespace ConsoleMenuBuilder.Tests;

public class MenuTests
{
    private static Menu CreateMenu(FakeConsole console) =>
        new Menu(console).Configure(config =>
        {
            // Keep the default renderers from writing to the real console.
            config.WriteItemAction = _ => { };
            config.WriteTitleAction = _ => { };
        });

    [Fact]
    public void Enter_RunsFirstItem()
    {
        var console = new FakeConsole(FakeConsole.Enter);
        var ran = new List<string>();
        var menu = CreateMenu(console);
        menu.Add("A", m => { ran.Add("A"); m.CloseMenu(); })
            .Add("B", m => { ran.Add("B"); m.CloseMenu(); });

        menu.Show();

        Assert.Equal(["A"], ran);
    }

    [Fact]
    public void DownThenEnter_RunsSecondItem()
    {
        var console = new FakeConsole(FakeConsole.Down, FakeConsole.Enter);
        var ran = new List<string>();
        var menu = CreateMenu(console);
        menu.Add("A", m => { ran.Add("A"); m.CloseMenu(); })
            .Add("B", m => { ran.Add("B"); m.CloseMenu(); });

        menu.Show();

        Assert.Equal(["B"], ran);
    }

    [Fact]
    public void UpFromFirstItem_WrapsToLast()
    {
        var console = new FakeConsole(FakeConsole.Up, FakeConsole.Enter);
        var ran = new List<string>();
        var menu = CreateMenu(console);
        menu.Add("A", m => { ran.Add("A"); m.CloseMenu(); })
            .Add("B", m => { ran.Add("B"); m.CloseMenu(); })
            .Add("C", m => { ran.Add("C"); m.CloseMenu(); });

        menu.Show();

        Assert.Equal(["C"], ran);
    }

    [Fact]
    public void DigitKey_RunsThatItemImmediately()
    {
        var console = new FakeConsole(FakeConsole.Digit(1));
        var ran = new List<string>();
        var menu = CreateMenu(console);
        menu.Add("A", m => { ran.Add("A"); m.CloseMenu(); })
            .Add("B", m => { ran.Add("B"); m.CloseMenu(); });

        menu.Show();

        Assert.Equal(["B"], ran);
    }

    [Fact]
    public void CloseItem_ClosesMenuWithoutRunningAnything()
    {
        var console = new FakeConsole(FakeConsole.Down, FakeConsole.Enter);
        var ran = false;
        var menu = CreateMenu(console);
        menu.Add("Run", () => ran = true)
            .Add("Back", Menu.Close);

        menu.Show();

        Assert.False(ran);
        Assert.Equal(0, console.RemainingKeys);
    }

    [Fact]
    public void SubmenuBackItem_ReturnsToParent()
    {
        var console = new FakeConsole(
            FakeConsole.Enter,                    // open "Sub"
            FakeConsole.Enter,                    // "Back" in the submenu
            FakeConsole.Down, FakeConsole.Enter); // "Quit" in the parent
        var child = CreateMenu(console).Add("Back", Menu.Close);
        var parent = CreateMenu(console)
            .Add("Sub", child.Show)
            .Add("Quit", Menu.Close);

        parent.Show();

        Assert.Equal(0, console.RemainingKeys);
    }

    [Fact]
    public void ReopeningMenu_StartsCursorOnFirstItem()
    {
        var console = new FakeConsole(FakeConsole.Down, FakeConsole.Enter);
        var ran = new List<string>();
        var menu = CreateMenu(console);
        menu.Add("A", m => { ran.Add("A"); m.CloseMenu(); })
            .Add("B", m => { ran.Add("B"); m.CloseMenu(); });

        menu.Show();
        console.Enqueue(FakeConsole.Enter);
        menu.Show();

        Assert.Equal(["B", "A"], ran);
    }

    [Fact]
    public async Task AsyncItem_IsAwaitedAndReceivesToken()
    {
        var console = new FakeConsole(FakeConsole.Enter);
        using var cts = new CancellationTokenSource();
        CancellationToken received = default;
        var completed = false;
        var menu = CreateMenu(console);
        menu.Add("Load", async ct =>
        {
            await Task.Yield();
            received = ct;
            completed = true;
            menu.CloseMenu();
        });

        await menu.ShowAsync(cts.Token);

        Assert.True(completed);
        Assert.Equal(cts.Token, received);
    }

    [Fact]
    public void ConfigureWithInstance_CopiesAllSettings()
    {
        Action<MenuItem> writeItem = _ => { };
        Action<string> writeTitle = _ => { };
        var original = new MenuConfiguration
        {
            Title = "Custom",
            Selector = "=> ",
            ClearConsole = false,
            EnableWriteTitle = false,
            ItemBackgroundColor = ConsoleColor.DarkBlue,
            ItemForegroundColor = ConsoleColor.Yellow,
            SelectedItemBackgroundColor = ConsoleColor.Red,
            SelectedItemForegroundColor = ConsoleColor.Green,
            WriteItemAction = writeItem,
            WriteTitleAction = writeTitle,
        };

        var copy = new MenuConfiguration(original);

        Assert.Equal("Custom", copy.Title);
        Assert.Equal("=> ", copy.Selector);
        Assert.False(copy.ClearConsole);
        Assert.False(copy.EnableWriteTitle);
        Assert.Equal(ConsoleColor.DarkBlue, copy.ItemBackgroundColor);
        Assert.Equal(ConsoleColor.Yellow, copy.ItemForegroundColor);
        Assert.Equal(ConsoleColor.Red, copy.SelectedItemBackgroundColor);
        Assert.Equal(ConsoleColor.Green, copy.SelectedItemForegroundColor);
        Assert.Same(writeItem, copy.WriteItemAction);
        Assert.Same(writeTitle, copy.WriteTitleAction);
    }
}
