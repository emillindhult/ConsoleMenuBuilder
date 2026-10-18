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

    public List<MenuItem> Items => this.menuItems;

    public MenuItem CurrentItem
    {
        get => this.menuItems[this.currentItemIndex];
        set => this.menuItems[this.currentItemIndex] = value;
    }

    public void Add(string name, Func<CancellationToken, Task> action)
    {
        this.menuItems.Add(new MenuItem(name, action, this.menuItems.Count));
    }

    public void ResetCurrentIndex()
    {
        this.selectedIndex = 0;
    }

    public MenuItem? GetSelectedItem()
    {
        if (this.selectedIndex < this.menuItems.Count)
        {
            return this.menuItems[this.selectedIndex.Value];
        }

        if (this.selectedName != null)
        {
            return this.menuItems.Find(item => item.Name == this.selectedName);
        }

        return null;
    }

    public void SelectNextVisibleItem(VisibilityManager visibility)
    {
        this.currentItemIndex = visibility.IndexOfNextVisibleItem(this.currentItemIndex);
    }

    public void SelectPreviousVisibleItem(VisibilityManager visibility)
    {
        this.currentItemIndex = visibility.IndexOfPreviousVisibleItem(this.currentItemIndex);
    }

    public bool CanSelect(char ch)
    {
        return ch >= '0' && (ch - '0') < this.menuItems.Count;
    }

    public void Select(char ch)
    {
        this.currentItemIndex = ch - '0';
    }

    public void UnsetSelectedIndex()
    {
        this.selectedIndex = null;
        this.selectedName = null;
    }

    internal bool IsSelected(MenuItem menuItem)
    {
        return this.currentItemIndex == menuItem.Index;
    }
}