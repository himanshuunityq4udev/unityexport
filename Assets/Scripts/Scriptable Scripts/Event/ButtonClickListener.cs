using System;
using UnityEngine;
using UnityEngine.Events;

public class ButtonClickListener : MonoBehaviour
{
    [SerializeField] ListenerGameEvent[] eventListiner;

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        foreach (ListenerGameEvent e in eventListiner)
        {
            e.listenerEvent.AddListener((value) =>
            {
                e.methodsToCallWithInt.Invoke(value);  // Calls methods that need an int
            });
        }
    }

    private void UnsubscribeFromEvents()
    {
        foreach (ListenerGameEvent e in eventListiner)
        {
            e.listenerEvent.RemoveListener((value) =>
            {
                e.methodsToCallWithInt.Invoke(value);  // Calls methods that need an int
            });
        }
    }
}

[Serializable]
public class ListenerGameEvent
{
    public string Title;
    public IntGameEvent listenerEvent;  // Assuming IntGameEvent is a custom class that derives from UnityEvent<int>

    // Here we store the methods we want to call, using UnityAction<int> or Action<int>
    public UnityEvent<int> methodsToCallWithInt;

}
