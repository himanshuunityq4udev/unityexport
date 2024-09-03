using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RDC
{
    public class ResetDronePosition :Singleton<ResetDronePosition>
    {
        private Rigidbody _droneRigidbody;
        //Store Initial spawn position and rotation
        private Vector3 initialPosition;
        private Quaternion initialRotation;
   
        private void Start()
        {
            _droneRigidbody = GetComponent<Rigidbody>();
            // Store the initial position and rotation
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }

        public void ResetPosition()
        {

            // Reset Rigidbody velocity and angular velocity
            _droneRigidbody.velocity = Vector3.zero;
            _droneRigidbody.angularVelocity = Vector3.zero;

            // Calculate the new position to be in front of the camera
            Transform cameraTransform = Camera.main.transform;
            Vector3 offset = initialPosition - cameraTransform.position;
            Vector3 newPosition = cameraTransform.position + cameraTransform.forward * offset.magnitude;

            // Set the new position
            transform.position = newPosition + new Vector3(0, -1, 2f);

            // Keep the initial rotation
            transform.rotation = initialRotation;
        }
    }
}