using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class MainThreadDispatcher
{
    private static readonly Queue<System.Action> actions = new Queue<System.Action>();

    public static void Update()
    {
        while (actions.Count > 0)
        {
            var action = actions.Dequeue();
            action();
        }
    }

    public static void RunOnMainThread(System.Action action)
    {
        if (action == null) { return; }
        lock (actions)
        {
            actions.Enqueue(action);
        }
    }

    public static Task RunOnMainThreadAsync(System.Action action)
    {
        var taskCompletionSource = new TaskCompletionSource<bool>();

        RunOnMainThread(() =>
        {
            action();
            taskCompletionSource.SetResult(true);
        });

        return taskCompletionSource.Task;
    }
}
