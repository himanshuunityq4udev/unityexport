using UnityEngine;

namespace Template.Script.Utils.Variables
{
    /// <summary>
    /// The generic variable definition.
    /// </summary>
    /// <typeparam name="T">The generic variable</typeparam>
    public abstract class GenericVariable<T> : ScriptableObject
    {
        [SerializeField] private T value;
        [SerializeField] private T originalValue;

        private EventDelegate<T> _changedValueEvent = new EventDelegate<T>();

        public T Value
        {
            get => value;
            set
            {
                if (!this.value.Equals(value))
                {
                    _changedValueEvent.Invoke(value);
                }
                this.value = value;
            }
        }

        public void AddChangedValueEventCallback(System.Action<T> func)
        {
            _changedValueEvent.AddCallback(func);
        }

        public void RemoveChangedValueEventCallback(System.Action<T> func)
        {
            _changedValueEvent.RemoveCallback(func);
        }

        public void Reset()
        {
            value = originalValue;
        }
    }
}