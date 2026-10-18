namespace ConsoleMenuBuilder;

internal class SystemConsole : IConsole
{
    public ConsoleColor ForegroundColor { get => Console.ForegroundColor; set => Console.ForegroundColor = value; }
    public ConsoleColor BackgroundColor { get => Console.BackgroundColor; set => Console.BackgroundColor = value; }
    public bool CursorVisible { get => Console.CursorVisible; set => Console.CursorVisible = value; }

    public void Clear() => Console.Clear();

    public ConsoleKeyInfo ReadKey(bool v) => Console.ReadKey(v);

    public void Write(string value) => Console.Write(value);

    public void WriteLine() => Console.WriteLine();
}