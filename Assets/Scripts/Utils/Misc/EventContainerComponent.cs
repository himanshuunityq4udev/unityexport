using UnityEngine;

namespace ModifiedObject.Scripts.Utils
{
    /// <summary>
    /// The event container component.
    /// </summary>
    public abstract class EventContainerComponent : MonoBehaviour
    {
        protected void Start()
        {
            OnStart();
            HockEvents();
        }
        protected virtual void OnStart() { }

        protected void OnEnable()
        {
            OnEnabled();
            HockEvents();
        }

        protected virtual void OnEnabled() { }

        protected void OnDisable()
        {
            OnDisabled();
            UnHookEvents();
        }
        protected virtual void OnDisabled() { }


        abstract protected void HockEvents();
        abstract protected void UnHookEvents();
    }
}