using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SCC
{
    public class CarController : MonoBehaviour
    {
        public enum Axel
        {
            front, Rear
        }

        [Serializable]
        public struct Wheel
        {
            public GameObject wheelModel;
            public WheelCollider wheelCollider;
            public GameObject wheelEffectObj;
            public ParticleSystem smokeParticle;
            public Axel axel;
        }

        public float maxAcceleration = 30.0f;
        public float breakAcceleration = 50.0f;

        public float turnSensitivity = 1.0f;
        public float maxSteerAngle = 30.0f;

        public Vector3 _centerOfMass;

        public List<Wheel> wheels;

        float moveInput;
        float steerInput;
        private Rigidbody carRb;

        private void Start()
        {
            carRb = GetComponent<Rigidbody>();
            carRb.centerOfMass = _centerOfMass;
        }

        private void Update()
        {
            GetInput();
            AnimateWheel();
            WheelEffects();
        }


        private void LateUpdate()
        {
            Move();
            Steer();
            Break();
        }


        void GetInput()
        {
            moveInput = Input.GetAxis("Vertical");
            steerInput = Input.GetAxis("Horizontal");
        }

        void Move()
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.motorTorque = moveInput * 600 * maxAcceleration * Time.deltaTime;
            }
        }

        void Steer()
        {
            foreach (var wheel in wheels)
            {
                if (wheel.axel == Axel.front)
                {
                    var _steerAngle = steerInput * turnSensitivity * maxSteerAngle;
                    wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);

                }
            }
        }

        void Break()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                foreach (var wheel in wheels)
                {
                    wheel.wheelCollider.brakeTorque = 300 * breakAcceleration * Time.deltaTime;
                }
            }
            else
            {
                foreach (var wheel in wheels)
                {
                    wheel.wheelCollider.brakeTorque = 0;
                }
            }
        }


        void AnimateWheel()
        {
            foreach (var wheel in wheels)
            {
                Quaternion rot;
                Vector3 pos;
                wheel.wheelCollider.GetWorldPose(out pos, out rot);
                wheel.wheelModel.transform.position = pos;
                wheel.wheelModel.transform.rotation = rot;
            }
        }

        void WheelEffects()
        {
            foreach (var wheel in wheels)
            {
                if (Input.GetKey(KeyCode.Space) && wheel.axel == Axel.Rear && wheel.wheelCollider.isGrounded == true && carRb.velocity.magnitude >= 2.0f)
                {
                    wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = true;
                    wheel.smokeParticle.Emit(1);
                }
                else
                {
                    wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = false;
                
                }
            }

        }
    }

}