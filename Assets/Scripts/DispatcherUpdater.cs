using UnityEngine;

public class DispatcherUpdater : MonoBehaviour
{
    void Update()
    {
        MainThreadDispatcher.Update();
    }
}