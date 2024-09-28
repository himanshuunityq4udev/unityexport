using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/GameEvent")]
public class GameEvent : ScriptableObject
{
    private readonly UnityEvent unityEvent = new UnityEvent();

    // Add a listener to this event
    public void AddListener(UnityAction listener)
    {
        unityEvent.AddListener(listener);
    }

    // Remove a listener from this event
    public void RemoveListener(UnityAction listener)
    {
        unityEvent.RemoveListener(listener);
    }

    // Invoke the event
    public void Invoke()
    {
        unityEvent.Invoke();
    }

    private void OnDisable()
    {
        unityEvent.RemoveAllListeners(); // Clean up when disabled
    }
}
