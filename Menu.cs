using System.Collections;
namespace ConsoleMenuBuilder;

public class Menu : IEnumerable
{
    private readonly ItemsCollection menuItems;
    public IReadOnlyList<MenuItem> Items => this.menuItems.Items;
    public IEnumerator GetEnumerator() => this.Items.GetEnumerator();
}