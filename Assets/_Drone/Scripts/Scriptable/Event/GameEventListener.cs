using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] GameEvent _gameEvents;
    [SerializeField] UnityEvent _responce;
    // Start is called before the first frame update
    private void OnEnable()
    {
        _gameEvents.AddListener(_responce.Invoke);
    }

    private void OnDisable()
    {
        _gameEvents.RemoveListener(_responce.Invoke);
    }
}
