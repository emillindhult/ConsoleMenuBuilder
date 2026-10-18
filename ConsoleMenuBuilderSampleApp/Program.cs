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
            .Add("Exit", () => Environment.Exit(0))
            .Configure(config =>
            {
                config.Title = "Library";
            });

        menu.Show();
    }
}
