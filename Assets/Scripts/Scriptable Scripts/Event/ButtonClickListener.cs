using System;
using UnityEngine;
using UnityEngine.Events;

public class ButtonClickListener : MonoBehaviour
{

    [SerializeField] ClickListener[] _responce2;
    [SerializeField] ListenerGameEvent[] _responce;
    // Start is called before the first frame update


    private void OnEnable()
    {
        foreach (ListenerGameEvent e in _responce)
        {
            foreach (ClickListener e2 in e.clickListeners)
            {
                e2.listenerEvent.AddListener(e2.responceEvent.Invoke);
            }
        }
        foreach (ClickListener e in _responce2)
        {
            e.listenerEvent.AddListener(e.responceEvent.Invoke);
        }

    }

    private void OnDisable()
    {
        foreach (ListenerGameEvent e in _responce)
        {
            foreach (ClickListener e2 in e.clickListeners)
            {
                e2.listenerEvent.RemoveListener(e2.responceEvent.Invoke);
            }
        }
        foreach (ClickListener e in _responce2)
        {
            e.listenerEvent.RemoveListener(e.responceEvent.Invoke);
        }
    }

}

[Serializable]
public class ListenerGameEvent
{
    public string Title;
    public ClickListener[] clickListeners;
   /* public GameEvent listenerEvent;
    public UnityEvent responceEvent;*/
}

[Serializable]
public class ClickListener
{
    public string buttonName;
    public GameEvent listenerEvent;
    public UnityEvent responceEvent;

}