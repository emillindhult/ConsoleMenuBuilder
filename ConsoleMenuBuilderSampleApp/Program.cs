using ConsoleMenuBuilder;

namespace ConsoleMenuBuilderSampleApp;

internal class Program
{
    static void Main(string[] args)
    {
        var menu = new Menu()
            .Add("Add Book", () =>
            {
                Console.WriteLine("Testing");
                Console.ReadKey();
            })
            .Add("Remove Book", () => Console.WriteLine())
            .Add("List Books", () => Console.WriteLine())
            .Add("Sub menu", () =>
            {
                var subMenu = new Menu()
                    .Add("Submenu Item 1", () => Console.WriteLine())
                    .Add("Submenu Item 2", () => Console.WriteLine())
                    .Add("Back", Menu.Close)
                    .Configure(config =>
                    {
                        config.Title = "Submenu";
                    });

                subMenu.Show();
            })
            .Add("Exit", Menu.Close)
            .Configure(config =>
            {
                config.Title = "Library";
            });

        menu.Show();
    }
}
