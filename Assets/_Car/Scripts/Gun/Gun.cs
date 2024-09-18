using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Gun : MonoBehaviour
{
    public GameObject projectile;
    public float speed = 100;

    private void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            Fire();
        }
    }
    void Fire()
    {
        Rigidbody clone;
        GameObject obj = Instantiate(projectile, transform.position, transform.rotation);
        clone = obj.GetComponent<Rigidbody>();
        clone.velocity = transform.TransformDirection(Vector3.forward * speed);

    }
}
