using UnityEngine;
using UnityEngine.InputSystem;

namespace RDC
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputManager : Singleton<InputManager>
    {
        #region Variables

        private Vector2 _cyclic;
        private float _pedals;
        private float _throttle;

        public Vector2 Cyclic => _cyclic; // Cyclic control - Movement (i.e., pitch and roll)
        public float Pedals => _pedals; // Pedal control - Rotation (i.e., yaw)
        public float Throttle => _throttle; // Throttle control - (i.e., altitude)

        private DroneInputs droneInputs;

        #endregion

        #region Main Methods

        protected override void AwakeSingleton()
        {
            Debug.Log("Inputs are initialized");
            InitializeInputs();
        }

        private void InitializeInputs()
        {

            droneInputs = new DroneInputs();

            // Cyclic (Pitch and Roll)
            droneInputs.Drone.Cyclic.performed += ctx => _cyclic = ctx.ReadValue<Vector2>();
            droneInputs.Drone.Cyclic.canceled += ctx => _cyclic = Vector2.zero;

            // Pedals (Yaw)
            droneInputs.Drone.Pedals.performed += ctx => _pedals = ctx.ReadValue<float>();
            droneInputs.Drone.Pedals.canceled += ctx => _pedals = 0f;

            // Throttle (Altitude)
            droneInputs.Drone.Throttle.performed += ctx => _throttle = ctx.ReadValue<float>();
            droneInputs.Drone.Throttle.canceled += ctx => _throttle = 0f;

        }

        private void OnEnable()
        {
            droneInputs.Enable();
        }


        private void OnDisable()
        {
            droneInputs.Disable();
        }
        #endregion

    }
}