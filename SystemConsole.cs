namespace ConsoleMenuBuilder;

internal class SystemConsole : IConsole
{
    public ConsoleColor ForegroundColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public ConsoleColor BackgroundColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void Clear() => Console.Clear();

    public ConsoleKeyInfo ReadKey(bool v) => Console.ReadKey(v);

    public void WriteLine() => Console.WriteLine();
}