namespace ConsoleMenuBuilder;

internal interface IConsole
{
    void Clear();
    void WriteLine();
    ConsoleKeyInfo ReadKey(bool v);

    ConsoleColor ForegroundColor { get; set; }

    ConsoleColor BackgroundColor { get; set; }
}
