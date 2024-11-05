using System.Collections.Generic;

namespace Template.Script.Utils
{
    public class EventDelegate
    {
        private List<System.Action> _callbacks = new List<System.Action>();

        public void AddCallback(System.Action func)
        {
            if (!_callbacks.Contains(func))
            {
                _callbacks.Add(func);
            }
        }
        public void RemoveCallback(System.Action func)
        {
            if (_callbacks.Contains(func))
            {
                _callbacks.Remove(func);
            }
        }



        public void Invoke()
        {
            int callbackLength = _callbacks.Count;
            for (int i = callbackLength - 1; i >= 0; i--)
            {
                var fun = _callbacks[i];
                fun?.Invoke();
            }
        }

        public static EventDelegate operator +(EventDelegate @deligate, System.Action func)
        {
            @deligate.AddCallback(func);
            return @deligate;
        }

        public static EventDelegate operator -(EventDelegate @deligate, System.Action @func)
        {
            @deligate.RemoveCallback(func);
            return @deligate;
        }
    }

    /// <summary>
    /// The event delegate - the type of func.
    /// </summary>
    /// <typeparam name="T">The type of the delegate.</typeparam>
    public class EventDelegate<T>
    {
        private List<System.Action<T>> _callbacks = new List<System.Action<T>>();

        public void AddCallback(System.Action<T> func)
        {
            if (!_callbacks.Contains(func))
            {
                _callbacks.Add(func);
            }
        }
        public void RemoveCallback(System.Action<T> func)
        {
            if (_callbacks.Contains(func))
            {
                _callbacks.Remove(func);
            }
        }

        public void Invoke(T param)
        {
            for (int i = _callbacks.Count - 1; i >= 0; i--)
            {
                var func = _callbacks[i];
                func?.Invoke(param);
            }
        }
        public static EventDelegate<T> operator +(EventDelegate<T> @delegate, System.Action<T> func)
        {
            @delegate.AddCallback(func);
            return @delegate;
        }

        public static EventDelegate<T> operator -(EventDelegate<T> @delegate, System.Action<T> @func)
        {
            @delegate.RemoveCallback(func);
            return @delegate;
        }
    }

    /// <summary>
    /// The event delegate - the type of func.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    public class EventDelegate<T1, T2>
    {
        private List<System.Action<T1, T2>> _callbacks
            = new List<System.Action<T1, T2>>();

        public void AddCallback(System.Action<T1, T2> func)
        {
            if (!this._callbacks.Contains(func))
            {
                this._callbacks.Add(func);
            }
        }

        public void RemoveCallback(System.Action<T1, T2> func)
        {
            if (this._callbacks.Contains(func))
            {
                this._callbacks.Remove(func);
            }
        }

        public void Invoke(T1 param1, T2 param2)
        {
            foreach (var func in this._callbacks)
            {
                func.Invoke(param1, param2);
            }
        }

        public static EventDelegate<T1, T2> operator +(EventDelegate<T1, T2> @delegate, System.Action<T1, T2> func)
        {
            @delegate.AddCallback(func);
            return @delegate;
        }

        public static EventDelegate<T1, T2> operator -(EventDelegate<T1, T2> @delegate, System.Action<T1, T2> @func)
        {
            @delegate.RemoveCallback(func);
            return @delegate;
        }
    }
}