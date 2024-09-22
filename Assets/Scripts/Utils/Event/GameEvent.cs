using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/GameEvents")]
public class GameEvent :ScriptableObject
{
    UnityEvent _unityEvent = new UnityEvent();

    public void AddListener(UnityAction listner)
    {
        _unityEvent.AddListener(listner);
    }
   
    public void RemoveListener(UnityAction listner)
    {
        _unityEvent.RemoveListener(listner);
    }

    public void Invoke()
    {
        _unityEvent.Invoke();
    }

    private void OnDisable()
    {
        _unityEvent.RemoveAllListeners();
    }
}
