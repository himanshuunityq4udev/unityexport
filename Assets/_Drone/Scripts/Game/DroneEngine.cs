using UnityEngine;


namespace RDC
{
    [RequireComponent(typeof(BoxCollider))]
    public class DroneEngine : MonoBehaviour, IEngine
    {

        #region Variables
        [Header("Drone Config")]
        [SerializeField] DroneConfig droneConfig;

        [Header("Propeller Properties")]
        [SerializeField] private Transform propeller;

        private Vector3 lastRayOrigin;
        private Vector3 lastRayDirection;
        private bool hitDetected;
        private RaycastHit lastHit;

        private float Zero = 0;
        private float One = 1;

        private float timeWhenGrounded = 4;
        private float timeWhenFlying = 10;

        private DroneController droneController;

        // Add an index to identify each engine
        [SerializeField] private int engineIndex;

        #endregion

        #region Interface Methods
        public void InitEngine()
        {
            // throw new System.NotImplementedException();
        }

        public void UpdateEngine(Rigidbody rb, InputManager inputs)
        {
            Vector3 upVector = transform.up;
            upVector.x = Zero;
            upVector.z = Zero;
            float diff = One - upVector.magnitude;

            float finalDiff = diff * Physics.gravity.magnitude;

            Vector3 engineForce = Vector3.zero;

            engineForce = transform.up * ((rb.mass * Physics.gravity.magnitude + finalDiff) + (inputs.Throttle * droneConfig.maxPower)) / droneConfig.enginCount;

            rb.AddForce(engineForce, ForceMode.Acceleration);

            HandlePropellers(inputs);


            // Store ray information for Gizmos
            lastRayOrigin = transform.position;
            lastRayDirection = Vector3.down;

            hitDetected = Physics.Raycast(lastRayOrigin, lastRayDirection, out lastHit, droneConfig.raycastLength, LayerMask.GetMask("Ground"));



            // Update grounded status in DroneController
            if (droneController == null)
            {
                droneController = GetComponentInParent<DroneController>();
            }

            if (droneController != null)
            {
                droneController.SetEngineGroundedStatus(engineIndex, hitDetected);
            }

        }

        private void HandlePropellers(InputManager inputs)
        {
            float propRotSpeed = Zero;
            if (!propeller)
            {
                return;
            }
            // Adjust propeller speed based on input
            float inputMagnitude = new Vector3(inputs.Cyclic.x, inputs.Cyclic.y, inputs.Throttle).magnitude;

            if (hitDetected)
            {
                propRotSpeed = Mathf.Lerp(propRotSpeed, Zero, timeWhenGrounded * Time.deltaTime);
            }
            else
            {
                propRotSpeed = Mathf.Lerp(propRotSpeed, droneConfig.maxpropRotSpeed, timeWhenFlying * Time.deltaTime);
            }
            propeller.Rotate(Vector3.forward, propRotSpeed);
        }

        private void OnDrawGizmos()
        {
            if (hitDetected)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(lastRayOrigin, lastHit.point);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(lastRayOrigin, lastRayOrigin + lastRayDirection * droneConfig.raycastLength);
            }
        }


        #endregion

    }
}