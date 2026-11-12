namespace ConsoleMenuBuilder.Tests;

/// <summary>
/// Scripted console: replays queued key presses and fails loudly if the menu
/// asks for more input than the test provided (instead of hanging).
/// </summary>
internal sealed class FakeConsole : IConsole
{
    private readonly Queue<ConsoleKeyInfo> keys = new();

    public FakeConsole(params ConsoleKeyInfo[] keys)
    {
        foreach (var key in keys)
        {
            this.keys.Enqueue(key);
        }
    }

    public static ConsoleKeyInfo Enter => new('\r', ConsoleKey.Enter, false, false, false);
    public static ConsoleKeyInfo Down => new('\0', ConsoleKey.DownArrow, false, false, false);
    public static ConsoleKeyInfo Up => new('\0', ConsoleKey.UpArrow, false, false, false);
    public static ConsoleKeyInfo Digit(int n) => new((char)('0' + n), ConsoleKey.D0 + n, false, false, false);

    public int RemainingKeys => this.keys.Count;

    public ConsoleColor ForegroundColor { get; set; }
    public ConsoleColor BackgroundColor { get; set; }
    public bool CursorVisible { get; set; }

    public void Enqueue(params ConsoleKeyInfo[] keys)
    {
        foreach (var key in keys)
        {
            this.keys.Enqueue(key);
        }
    }

    public void Clear()
    {
    }

    public void Write(string value)
    {
    }

    public void WriteLine()
    {
    }

    public ConsoleKeyInfo ReadKey(bool v) =>
        this.keys.TryDequeue(out var key)
            ? key
            : throw new InvalidOperationException("The menu asked for more key presses than the test supplied.");
}
