using UnityEngine;

[CreateAssetMenu( menuName = "DroneConfig / Config", fileName ="Config")]
public class DroneConfig : ScriptableObject
{
    [Header("RigidBody Properties")]
    public float weight = 1f;

    [Header("Drone Rotation")]
    // The speed of rotation
    public float rotationSpeed = 1.0f;

    // The threshold to consider the rotation complete
    public float rotationThreshold = 0.01f;

    [Header("Engine Properties")]
    public float maxPower = 4;
    public int enginCount = 4;

    [Header("Propeller Properties")]
    public float maxpropRotSpeed = 300;

    [Header("Raycast Properties")]
    public float raycastLength = 0.1f;


    [Header("Control Property")]
    public float minMaxPitch = 30;
    public float minMaxRoll = 30;
    public float yawPower = 4;
    public float lerpSpeed = 2;

    [Header("Fuel Consumption Rate")]

    public float fuelConsumptionRateWhenNotMoving = 0.25f;
    public float fuelConsumptionRateWhenMoving = 1.5f;

   

}
