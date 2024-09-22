using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePropaller : MonoBehaviour
{
    [SerializeField] private float speed = -1000f;
    [SerializeField] bool xRotatation;
    [SerializeField] bool yRotatation;
    [SerializeField] bool zRotatation;
    void Update()
    {
        if (xRotatation)
        {
            transform.Rotate(new Vector3(speed * Time.fixedDeltaTime,0, 0));
        }
        else if (yRotatation)
        {

            transform.Rotate(new Vector3(0, speed * Time.fixedDeltaTime, 0));
        }
        else
        {

            transform.Rotate(new Vector3(0,0, speed * Time.fixedDeltaTime));
        }
    }
}
