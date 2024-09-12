using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RDC
{
    [RequireComponent(typeof(InputManager))]
    public class DroneController : RigidBodyManager
    {
        #region Variables
        private InputManager input;
        private List<IEngine> _engines = new List<IEngine>();

        //Drone Configation 
        [SerializeField] private DroneConfig droneConfig;
        //Input value
        private float _rotationFinalPitch;
        private float _rotationFinalRoll;

        private float _finalYaw;
        private float yaw;

        //Store Initial spawn position and rotation
        private Vector3 initialPosition;
        private Quaternion initialRotation;

        private float Zero = 0;

        //Boolen check for grounded or not;
        private bool isGrounded;
        private bool[] engineGroundedStatuses;

        // FuelSystem fuelSystem;

        public bool canRotate;

        #endregion

        #region Main Methods
        private void Start()
        {
            InitializeDrone();
        }
        #endregion
        #region Initialization

        private void InitializeDrone()
        {
            //  droneConfig = RefrenceManager.Instance.DroneConfig;
            if (droneConfig == null)
            {
                Debug.LogError("Drone configuration is missing!");
                return;
            }

            // Assume there are 4 engines
            engineGroundedStatuses = new bool[droneConfig.enginCount];
            input = GetComponent<InputManager>();
            _engines = GetComponentsInChildren<IEngine>().ToList();

            // Store the initial position and rotation
            initialPosition = transform.position;
            initialRotation = transform.rotation;

        }

        #endregion
        #region Physics Handling

        protected override void HandlePhysics()
        {
            HandleEngines();
            HandleControls();
            UpdateDroneSound();
            // RefrenceManager.Instance.speedText.text = (_droneRigidbody.velocity.magnitude * 2.23693629f).ToString("0") + " KM/H";
        }

        #endregion
        #region CustomMethods

        private void UpdateDroneSound()
        {
            if (isGrounded)
            {
                SoundManager.Instance.DroneAudioSource.volume = 0;
            }
            else
            {
                SoundManager.Instance.DroneSound();
            }
        }

        protected virtual void HandleControls()
        {
            if (GetGroundedStatus()) // If the drone is grounded
            {
                StopDroneMovement();
            }
            else
            {
                UpdateDroneMovement();
            }

            FuelSystem.Instance.ConsumeFuel(droneConfig.fuelConsumptionRateWhenNotMoving, droneConfig.fuelConsumptionRateWhenMoving);
        }
        private void UpdateDroneMovement()
        {
            // Calculate pitch, roll, and yaw based on input
            float pitch = input.Cyclic.y * droneConfig.minMaxPitch;
            float roll = -input.Cyclic.x * droneConfig.minMaxRoll;

            yaw += input.Pedals * droneConfig.yawPower;

            // Smooth transitions for pitch, roll, and yaw
            _rotationFinalPitch = Mathf.Lerp(_rotationFinalPitch, pitch, droneConfig.lerpSpeed * Time.deltaTime);
            _rotationFinalRoll = Mathf.Lerp(_rotationFinalRoll, roll, droneConfig.lerpSpeed * Time.deltaTime);
            _finalYaw = Mathf.Lerp(_finalYaw, yaw, droneConfig.lerpSpeed * Time.deltaTime);


     
            if (canRotate)
            {
                RotateDrone(_rotationFinalPitch, _finalYaw, _rotationFinalRoll);
            }
            else
            {
                AlignDroneWithCamera(_rotationFinalPitch, _rotationFinalRoll);
            }
        }

        private void AlignDroneWithCamera(float pitch, float roll )
        {
            Quaternion currentRotation = DroneRigidbody.rotation;
            Vector3 currentEulerAngles = currentRotation.eulerAngles;

            float targetYaw = Mathf.LerpAngle(currentEulerAngles.y, Camera.main.transform.eulerAngles.y, droneConfig.rotationSpeed * Time.deltaTime);
            Quaternion targetRotation = Quaternion.Euler(pitch, targetYaw, roll);

            if (Mathf.Abs(currentEulerAngles.y) < droneConfig.rotationThreshold)
            {
                targetRotation = Quaternion.Euler(pitch, Camera.main.transform.eulerAngles.y, roll);
            }

            DroneRigidbody.MoveRotation(targetRotation);
       
        }

        private void RotateDrone(float pitch, float yaw, float roll)
        {
            Quaternion rotation = Quaternion.Euler(pitch, yaw, roll);
            DroneRigidbody.MoveRotation(rotation);
           
        }
        private void StopDroneMovement()
        {
            _rotationFinalPitch = Zero;
            _rotationFinalRoll = Zero;
            yaw = Zero;

            Quaternion stopRotation = Quaternion.Euler(Zero, Zero, Zero);
            DroneRigidbody.MoveRotation(stopRotation);
        }



        #endregion
        #region Engine Handling
        protected virtual void HandleEngines()
        {
            foreach (IEngine engine in _engines)
            {
                engine.UpdateEngine(DroneRigidbody, input);
            }
        }
        #endregion

        #region Grounded Status Handling
        public void SetEngineGroundedStatus(int engineIndex, bool grounded)
        {
            engineGroundedStatuses[engineIndex] = grounded;
            CheckGroundedStatus();
        }

        private void CheckGroundedStatus()
        {
            isGrounded = isGrounded = engineGroundedStatuses.All(status => status);
        }

        public bool GetGroundedStatus()
        {
            return isGrounded;
        }

        #endregion
        #region Reset Functionality
        public void ResetPosition()
        {
            // Reset pitch, roll, and yaw
            _rotationFinalPitch = Zero;
            _rotationFinalRoll = Zero;
            _finalYaw = Zero;
            yaw = Zero;

            // Reset Rigidbody velocity and angular velocity
            DroneRigidbody.velocity = Vector3.zero;
            DroneRigidbody.angularVelocity = Vector3.zero;

            // Set new position in front of the camera
            Transform cameraTransform = Camera.main.transform;
            Vector3 offset = initialPosition - cameraTransform.position;
            Vector3 newPosition = cameraTransform.position + cameraTransform.forward * offset.magnitude;
            transform.position = newPosition + new Vector3(0, -1, 2f);

            // Reset rotation to initial values
            transform.rotation = initialRotation;
        }
        #endregion
    }
}
