using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace AVC {
    public class ArcadeVehicleController : MonoBehaviour {
        public enum GroundCheck { RayCast, Sphere };
        public GroundCheck groundCheck;
        public LayerMask drivableSurface;

        public float maxSpeed;
        public float accelaration;
        public float turn;
        public float gravity = 7f;
        public float downforce = 5f;

        public float decelerationMultiplier = 1.25f;

        public bool airControl = false;
        
        public bool driftMode = false;
        public float driftMultiplier = 1.5f;

        public bool aiMode = false;

        public Rigidbody rb, carBody;

        [HideInInspector]
        public RaycastHit hit;

        public AnimationCurve frictionCurve;
        public AnimationCurve turnCurve;
        public PhysicMaterial frictionMaterial;

        public Transform bodyMesh;
        public Transform[] frontWheels = new Transform[2];
        public Transform[] rearWheels = new Transform[2];

        public bool useEffects = false;
        public TrailRenderer RLSkid;
        public TrailRenderer RRSkid;
        public ParticleSystem RLSmoke;
        public ParticleSystem RRSmoke;

        [HideInInspector]
        public Vector3 carVelocity;

        [Range(0, 10)]
        public float bodyTilt;
        public AudioSource engineSound;
        [Range(0, 1)]
        public float minPitch;
        [Range(1, 3)]
        public float maxPitch;
        public AudioSource skidSound;

        private float radius, horizontalInput, verticalInput;
        private Vector3 origin;

        // AI
        public Transform aiTarget;
        public float aiBreakAngle = 30f;
        [HideInInspector]
        public float aiTurn = 1f;
        [HideInInspector]
        public float aiSpeed = 1f;
        [HideInInspector]
        public float aiBreak = 0f;
        [HideInInspector]
        private float aiDesiredTurning;
        [HideInInspector]
        private CinemachineVirtualCamera virtualCamera;

        private void Start() {
            radius = rb.GetComponent<SphereCollider>().radius;

            virtualCamera = gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
            if(virtualCamera != null) {
                virtualCamera.Priority = aiMode ? 0 : 1;
            }
        }

        private void Update() {
            // Calculate AI stats if enabled or get input
            if(aiMode && aiTarget != null) {
                AIGenerateStats();
            } else {
                horizontalInput = Input.GetAxis("Horizontal");
                verticalInput = Input.GetAxis("Vertical");
            }

            Visuals();
            AudioManager();
        }

        private void FixedUpdate() {
            carVelocity = carBody.transform.InverseTransformDirection(carBody.velocity);

            if (Mathf.Abs(carVelocity.x) > 0) 
                frictionMaterial.dynamicFriction = frictionCurve.Evaluate(Mathf.Abs(carVelocity.x / 100));

            if(aiMode && aiTarget == null) 
                return;

            if (Grounded()) {
                // Turning
                float sign = Mathf.Sign(carVelocity.z);
                float TurnMultiplyer = turnCurve.Evaluate(carVelocity.magnitude / maxSpeed);

                // Drifting Turning
                if (!aiMode && driftMode && Input.GetAxis("Jump") > 0.1f) 
                    TurnMultiplyer *= driftMultiplier;

                if (verticalInput > 0.1f || aiSpeed > 0.1f || carVelocity.z > 1)
                {
                    if(aiMode) 
                        carBody.AddTorque(Vector3.up * aiTurn * sign * turn * 100 * TurnMultiplyer);
                    else 
                        carBody.AddTorque(Vector3.up * horizontalInput * sign * turn * 100 * TurnMultiplyer);
                } else if (verticalInput < -0.1f || aiSpeed < -0.1f || carVelocity.z < -1) {
                    if(aiMode) 
                        carBody.AddTorque(Vector3.up * aiTurn * sign * turn * 100 * TurnMultiplyer);
                    else 
                        carBody.AddTorque(Vector3.up * horizontalInput * sign * turn * 100 * TurnMultiplyer);
                }

                // Brake
                if(aiMode)
                    rb.constraints = aiBreak > 0.1f ? RigidbodyConstraints.FreezeRotationX : RigidbodyConstraints.None;
                else
                    if (!driftMode) 
                        rb.constraints = Input.GetAxis("Jump") > 0.1f ? RigidbodyConstraints.FreezeRotationX : RigidbodyConstraints.None;

                // Accelaration
                if(aiMode) {
                    if (Mathf.Abs(aiSpeed) > 0.1f && aiBreak < 0.1f)
                        rb.velocity = Vector3.Lerp(rb.velocity, carBody.transform.forward * aiSpeed * maxSpeed, accelaration / 10 * Time.deltaTime);
                } else {
                    if (Mathf.Abs(verticalInput) > 0.1f && Input.GetAxis("Jump") < 0.1f && !driftMode)
                        rb.velocity = Vector3.Lerp(rb.velocity, carBody.transform.forward * verticalInput * maxSpeed, accelaration / 10 * Time.deltaTime);
                    else if (Mathf.Abs(verticalInput) > 0.1f && driftMode)
                        rb.velocity = Vector3.Lerp(rb.velocity, carBody.transform.forward * verticalInput * maxSpeed, accelaration / 10 * Time.deltaTime);
               
                    if(Mathf.Abs(verticalInput) < 0.1f && Input.GetAxis("Jump") < 0.1f && decelerationMultiplier > 0f)
                        rb.velocity = rb.velocity * (1f / (1f + (0.025f * decelerationMultiplier)));
                }

                // Down Force
                if(!aiMode) rb.AddForce(-transform.up * downforce * rb.mass);

                // Body Tilt
                carBody.MoveRotation(Quaternion.Slerp(carBody.rotation, Quaternion.FromToRotation(carBody.transform.up, hit.normal) * carBody.transform.rotation, 0.12f));
            } else {
                carBody.MoveRotation(Quaternion.Slerp(carBody.rotation, Quaternion.FromToRotation(carBody.transform.up, Vector3.up) * carBody.transform.rotation, 0.02f));
                
                if(useEffects) {
                    RLSkid.emitting = false;
                    RRSkid.emitting = false;
                    RLSmoke.Stop();
                    RRSmoke.Stop();
                }

                if(!aiMode) {
                    if (airControl) {
                        float TurnMultiplyer = turnCurve.Evaluate(carVelocity.magnitude / maxSpeed);
                        carBody.AddTorque(Vector3.up * horizontalInput * turn * 100 * TurnMultiplyer);
                    }
                    rb.velocity = Vector3.Lerp(rb.velocity, rb.velocity + Vector3.down * gravity, Time.deltaTime * gravity);
                }
            }
        }

        private void Visuals() {
            // Wheels
            foreach (Transform wheel in frontWheels) {
                if(aiMode) 
                    wheel.localRotation = Quaternion.Slerp(wheel.localRotation, Quaternion.Euler(wheel.localRotation.eulerAngles.x, 30 * aiTurn, wheel.localRotation.eulerAngles.z), 0.1f);
                else 
                    wheel.localRotation = Quaternion.Slerp(wheel.localRotation, Quaternion.Euler(wheel.localRotation.eulerAngles.x, 30 * horizontalInput, wheel.localRotation.eulerAngles.z), 0.7f * Time.deltaTime / Time.fixedDeltaTime);
                wheel.GetChild(0).localRotation = rb.transform.localRotation;
            }
            rearWheels[0].localRotation = rb.transform.localRotation;
            rearWheels[1].localRotation = rb.transform.localRotation;

            // Body
            if (carVelocity.z > 1) {
                if(aiMode)
                    bodyMesh.localRotation = Quaternion.Slerp(bodyMesh.localRotation, Quaternion.Euler(Mathf.Lerp(0, -5, carVelocity.z / maxSpeed), bodyMesh.localRotation.eulerAngles.y, Mathf.Clamp(aiDesiredTurning * aiTurn, -bodyTilt, bodyTilt)), 0.05f);
                else
                    bodyMesh.localRotation = Quaternion.Slerp(bodyMesh.localRotation, Quaternion.Euler(Mathf.Lerp(0, -5, carVelocity.z / maxSpeed), bodyMesh.localRotation.eulerAngles.y, bodyTilt * horizontalInput), 0.4f * Time.deltaTime / Time.fixedDeltaTime);
            } else {
                if(aiMode)
                    bodyMesh.localRotation = Quaternion.Slerp(bodyMesh.localRotation, Quaternion.Euler(0, 0, 0), 0.05f);
                else
                    bodyMesh.localRotation = Quaternion.Slerp(bodyMesh.localRotation, Quaternion.Euler(0, 0, 0), 0.4f * Time.deltaTime / Time.fixedDeltaTime);
            }

            if (!aiMode && driftMode) {
                Quaternion quaternion = Quaternion.Euler(0, 0, 0);
                if (Input.GetAxis("Jump") > 0.1f)
                    quaternion = Quaternion.Euler(0, 45 * horizontalInput * Mathf.Sign(carVelocity.z), 0);
                bodyMesh.parent.localRotation = Quaternion.Slerp(bodyMesh.parent.localRotation, quaternion, 0.1f * Time.deltaTime / Time.fixedDeltaTime);
            }

            // Effects
            if(useEffects && Grounded()) {
                if(Mathf.Abs(carVelocity.x) > 10f){
                    RLSkid.emitting = true;
                    RRSkid.emitting = true;
                } else {
                    RLSkid.emitting = false;
                    RRSkid.emitting = false;
                }

                if(Mathf.Abs(carVelocity.x) > 10f && (Input.GetAxis("Jump") > 0.1f || aiMode)){
                    RLSmoke.Play();
                    RRSmoke.Play();
                } else {
                    RLSmoke.Stop();
                    RRSmoke.Stop();
                }
            }
        }

        private void AudioManager() {
            engineSound.pitch = Mathf.Lerp(minPitch, maxPitch, Mathf.Abs(carVelocity.z) / maxSpeed);
            skidSound.mute = Mathf.Abs(carVelocity.x) < 10 && Grounded();
        }

        private void AIGenerateStats() {
            // the new method of calculating turn value
            Vector3 aimedPoint = aiTarget.position;
            aimedPoint.y = transform.position.y;
            Vector3 aimedDir = (aimedPoint - transform.position).normalized;
            Vector3 myDir = transform.forward;
            myDir.Normalize();
            aiDesiredTurning = Mathf.Abs(Vector3.Angle(myDir, Vector3.ProjectOnPlane(aimedDir, transform.up)));

            float reachedTargetDistance = 1f;
            float distanceToTarget = Vector3.Distance(transform.position, aiTarget.position);
            Vector3 dirToMovePosition = (aiTarget.position - transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, dirToMovePosition);
            float angleToMove = Vector3.Angle(transform.forward, dirToMovePosition);
            if (angleToMove > aiBreakAngle)
                aiBreak = carVelocity.z > 15 ? 1 : 0;
            else
                aiBreak = 0;

            if (distanceToTarget > reachedTargetDistance) {
                if (dot > 0) {
                    aiSpeed = 1f;
                    float stoppingDistance = 5f;
                    aiBreak = distanceToTarget < stoppingDistance ? 1f : 0f;
                } else {
                    float reverseDistance = 5f;
                    aiSpeed = distanceToTarget > reverseDistance ? 1f : -1f;
                }

                float angleToDir = Vector3.SignedAngle(transform.forward, dirToMovePosition, Vector3.up);
                aiTurn = (angleToDir > 0 ? 1f : -1f) * turnCurve.Evaluate(aiDesiredTurning / 90);
            } else {
                aiBreak = carVelocity.z > 1f ? -1f : 0f;
                aiTurn = 0f;
            }
        }

        private bool Grounded() {
            origin = rb.position + rb.GetComponent<SphereCollider>().radius * Vector3.up;
            var direction = -transform.up;
            var maxdistance = rb.GetComponent<SphereCollider>().radius + 0.2f;

            if (groundCheck == GroundCheck.RayCast)
                return Physics.Raycast(rb.position, Vector3.down, out hit, maxdistance, drivableSurface);
            else if (groundCheck == GroundCheck.Sphere)
                return Physics.SphereCast(origin, radius + 0.1f, direction, out hit, maxdistance, drivableSurface);
            else 
                return false; 
        }

        private void OnDrawGizmos() {
            float width = 0.02f;
            radius = rb.GetComponent<SphereCollider>().radius;
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(rb.transform.position + ((radius + width) * Vector3.down), new Vector3(2 * radius, 2 * width, 4 * radius));
            BoxCollider boxCollider = GetComponent<BoxCollider>();
            if (boxCollider) {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(transform.position, boxCollider.size);
            }

            if(aiMode && aiTarget != null) {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, aiTarget.position);
                Gizmos.DrawSphere(aiTarget.position, 1f);
            }
        }
    }
}