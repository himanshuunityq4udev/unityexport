using UnityEngine;

public class CarAnimation : MonoBehaviour
{
    [SerializeField] private Vector3 finalPosition;
    [SerializeField] private float speed;
    private Vector3 initialPosition;
   

    private void Awake()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, finalPosition, speed);
    }
    
    private void OnDisable()
    {
        transform.position = initialPosition;
    }
}
