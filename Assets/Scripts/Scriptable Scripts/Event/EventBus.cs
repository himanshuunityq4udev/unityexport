using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus
{
    // Dictionary to store events by type
    private static Dictionary<Type, List<Action<object>>> eventListeners = new Dictionary<Type, List<Action<object>>>();

    // Subscribe to an event
    public static void Subscribe<T>(Action<T> listener)
    {
        Type eventType = typeof(T);

        if (!eventListeners.ContainsKey(eventType))
        {
            eventListeners[eventType] = new List<Action<object>>();
        }

        // Wrap listener with an Action<object>
        Action<object> wrapper = (obj) => listener((T)obj);
        eventListeners[eventType].Add(wrapper);
    }

    // Unsubscribe from an event
    public static void Unsubscribe<T>(Action<T> listener)
    {
        Type eventType = typeof(T);

        if (eventListeners.ContainsKey(eventType))
        {
            // Find and remove the listener
            eventListeners[eventType].RemoveAll(wrapper => wrapper.Equals(listener));
        }
    }

    // Publish an event
    public static void Publish<T>(T eventData)
    {
        Type eventType = typeof(T);

        if (eventListeners.ContainsKey(eventType))
        {
            foreach (var listener in eventListeners[eventType])
            {
                listener.Invoke(eventData);
            }
        }
    }
}
