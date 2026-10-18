using ConsoleMenuBuilder;

namespace ConsoleMenuBuilderSampleApp;

internal class Program
{
    static void Main(string[] args)
    {
        var menu = new Menu()
            .Add("One", () =>
            {
                Console.WriteLine("Testing");
                Console.ReadKey();
            })
            .Add("Two", () => Console.WriteLine())
            .Add("Three", () => Console.WriteLine())
            .Add("Exit", () => Environment.Exit(0))
            .Configure(config =>
            {
                config.Title = "Library Manager";
            });

        menu.Show();
    }
}
