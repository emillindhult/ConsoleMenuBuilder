using System.Diagnostics;

namespace ConsoleMenuBuilder;

public sealed class MenuItem
{
    internal MenuItem(string name, Func<CancellationToken, Task> action, int index)
    {
        Debug.Assert(index >= 0);

        this.Name = name ?? throw new ArgumentNullException(nameof(name));
        this.AsyncAction = action ?? throw new ArgumentNullException(nameof(action));
        this.Index = index;
    }

    public string Name { get; set; }
    public Action Action
    {
        get => () => this.AsyncAction(CancellationToken.None).GetAwaiter().GetResult();
        set => this.AsyncAction = (_) =>
        {
            value();
            return Task.CompletedTask;
        }
    }

    public Func<CancellationToken, Task> AsyncAction { get; set; }
    public int Index { get; }
}
