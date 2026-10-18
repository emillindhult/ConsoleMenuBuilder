namespace ConsoleMenuBuilder;

internal interface IConsole
{
    void Clear();
    void WriteLine();
    void Write(string value);
    ConsoleKeyInfo ReadKey(bool v);

    ConsoleColor ForegroundColor { get; set; }

    ConsoleColor BackgroundColor { get; set; }
    bool CursorVisible { get; set; }
}
