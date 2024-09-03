
using UnityEngine;

public class DroneProperty : MonoBehaviour
{
    [Header("Speed")]
    public float speed;
    public float battery;
    [SerializeField] GameEvent changeSliderProperty;

  /*  [Header("Speed")]
   // [SerializeField] private customSliderScript speed_ref;
   // [SerializeField] private customSliderScript battery_ref;

    private void OnEnable()
    {
        changeSliderProperty.AddListener(ChangeValue);


    }

    private void OnDisable()
    {
        changeSliderProperty.RemoveListener(ChangeValue);
    }
    private void ChangeValue()
    {
        speed_ref.ValueChange(speed);
        battery_ref.ValueChange(battery);

    }*/

}
