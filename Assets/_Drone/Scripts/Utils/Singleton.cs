using UnityEngine;

namespace RDC
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        [SerializeField] private bool dontDestroyOnLoad = false;

        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();

                    if (instance == null)
                    {
                        GameObject singletonObject = new GameObject(typeof(T).Name + " (Singleton)");
                        instance = singletonObject.AddComponent<T>();
                    }
                }
                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;

                if (dontDestroyOnLoad)
                {
                    DontDestroyOnLoad(gameObject);
                }

                AwakeSingleton();  // This will now call the virtual method
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void AwakeSingleton()
        {
            // Derived classes can override this method to provide custom initialization
        }
    }
}