using UnityEngine;

namespace RDC
{
    [RequireComponent(typeof(Rigidbody))]

    public class RigidBodyManager : MonoBehaviour
    {
        #region Fields
        [Header("Rigidbody Settings")]
        [SerializeField] private bool freezeRotationY = true;

        private Rigidbody _droneRigidbody;
        #endregion

        #region Public Methods

        public Rigidbody DroneRigidbody => _droneRigidbody;

        #endregion
        #region Unity Methods

        protected virtual void Awake()
        {
            _droneRigidbody = GetComponent<Rigidbody>();

            InitializeRigidbody();
        }
        protected virtual void FixedUpdate()
        {
            if (_droneRigidbody == null) return;

            HandlePhysics();
            ConstrainMovement();
        }
        #endregion
        #region Protected Methods
        protected virtual void InitializeRigidbody() 
        {
            if (_droneRigidbody == null)
            {
                Debug.LogError($"{nameof(RigidBodyManager)} requires a Rigidbody component.");
                enabled = false; // Disable script to prevent further errors
                return;
            }

            if (freezeRotationY)
            {
                _droneRigidbody.constraints |= RigidbodyConstraints.FreezeRotationY;
            }
        }


        protected virtual void HandlePhysics() { }
        protected virtual void ConstrainMovement() { }
        #endregion

    }
}