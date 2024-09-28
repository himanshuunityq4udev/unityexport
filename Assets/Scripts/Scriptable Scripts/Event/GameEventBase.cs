using UnityEngine;
public class GameEventBase<T> : ScriptableObject
{
    public delegate void EventAction(T arg);
    private event EventAction _EventRaised;

    public void AddListener(EventAction listener)
    {
        _EventRaised += listener;
    }

    public void RemoveListener(EventAction listener)
    {
        _EventRaised -= listener;
    }

    public void Invoke(T arg)
    {
        Debug.Log("With parameter");
        _EventRaised?.Invoke(arg);
    }
    // Overloaded method to invoke without an argument
    public void Invoke()
    {
        Debug.Log("Without parameter");

        Invoke(default(T)); // Pass default value of T (e.g., null for reference types, 0 for ints, etc.)
    }


    void OnDisable()
    {
        _EventRaised = null;
    }
}


[CreateAssetMenu(menuName = "Events/BoolEvent")]
public class BoolGameEvent : GameEventBase<bool> { }

[CreateAssetMenu(menuName = "Events/IntEvent")]
public class IntGameEvent : GameEventBase<int> { }

[CreateAssetMenu(menuName = "Events/FloadEvent")]
public class FloatGameEvent : GameEventBase<float> { }

[CreateAssetMenu(menuName = "Events/StringEvent")]
public class StringGameEvent : GameEventBase<string> { }