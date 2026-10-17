using System.Collections;
namespace ConsoleMenuBuilder;

public class Menu : IEnumerable
{
    private readonly ItemsCollection menuItems;
    private MenuConfiguration config = new MenuConfiguration();
    private Menu? parent = null;

    public Menu()
    {
        this.menuItems = new ItemsCollection();
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
    }

    public IReadOnlyList<MenuItem> Items => this.menuItems.Items;
    public IEnumerator GetEnumerator() => this.Items.GetEnumerator();
}