namespace ConsoleMenuBuilder;

internal sealed class MenuDisplay
{
    private readonly IConsole console;
    private readonly ItemsCollection menuItems;
    private readonly MenuConfiguration config;
    private readonly VisibilityManager visibility;
    private readonly CloseTrigger closeTrigger;
    //private readonly string noSelectorLine;

    public MenuDisplay(ItemsCollection menuItems,
        IConsole console,
        MenuConfiguration config,
        CloseTrigger closeTrigger)
    {
        this.menuItems = menuItems;
        this.console = console;
        this.config = config;
        this.visibility = new VisibilityManager(menuItems.Items.Count);
        this.closeTrigger = closeTrigger;
    }

    public async Task ShowAsync(CancellationToken cancellationToken)
    {
        var selectedItem = this.menuItems.GetSelectedItem();
        if (selectedItem != null)
        {
            await selectedItem.AsyncAction.Invoke(cancellationToken);
            return;
        }

        ConsoleKeyInfo key;
        this.menuItems.ResetCurrentIndex();
        var currentForegroundColor = this.console.ForegroundColor;
        var currentBackgroundColor = this.console.BackgroundColor;
        bool breakIteration = false;

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            do
            {
                if (this.config.ClearConsole)
                {
                    this.console.Clear();
                }

                if (this.config.EnableWriteTitle)
                {
                    this.config.WriteTitleAction(this.config.Title);
                }

                foreach (var menuItem in this.menuItems.Items)
                {
                    this.WriteLineWithItem(menuItem);
                }

                if (breakIteration)
                {
                    breakIteration = false;
                    break;
                }
            //readKey:
                key = this.console.ReadKey(true);

                if (key.Key == ConsoleKey.DownArrow)
                {
                    this.menuItems.SelectNextVisibleItem(this.visibility);
                }
                else if (key.Key == ConsoleKey.UpArrow)
                {
                    this.menuItems.SelectPreviousVisibleItem(this.visibility);
                }
                else if (this.menuItems.CanSelect(key.KeyChar))
                {
                    this.menuItems.Select(key.KeyChar);
                    breakIteration = true;
                }
            }
            while (key.Key != ConsoleKey.Enter);

            this.console.WriteLine();
            this.console.ForegroundColor = currentForegroundColor;
            this.console.BackgroundColor = currentBackgroundColor;
            var action = this.menuItems.CurrentItem.AsyncAction;
            if (action == Menu.Close)
            {
                this.menuItems.UnsetSelectedIndex();
                return;
            }
            else
            {
                await action(cancellationToken).ConfigureAwait(false);
                if (this.closeTrigger.IsOn())
                {
                    this.menuItems.UnsetSelectedIndex();
                    this.closeTrigger.SetOff();
                    return;
                }
            }
        }
    }

    private void WriteLineWithItem(MenuItem menuItem)
    {
        if (this.menuItems.IsSelected(menuItem))
        {
            this.console.BackgroundColor = this.config.SelectedItemBackgroundColor;
            this.console.ForegroundColor = this.config.SelectedItemForegroundColor;
            this.config.WriteItemAction(menuItem);
            this.console.WriteLine();
            this.console.BackgroundColor = this.config.ItemBackgroundColor;
            this.console.ForegroundColor = this.config.ItemForegroundColor;
        }
        else
        {
            this.console.BackgroundColor = this.config.ItemBackgroundColor;
            this.console.ForegroundColor = this.config.ItemForegroundColor;
            this.config.WriteItemAction(menuItem);
            this.console.WriteLine();
        }
    }
}