using TMPro;
using UnityEngine;

public class UiView : MonoBehaviour
{
    [Header("Component Refrence")]
    [SerializeField] private TMP_Text _titleTxt;


    private void OnEnable()
    {
        EventBus.Subscribe<UpdateTitleTextEvent>(UpdateTitleText);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<UpdateTitleTextEvent>(UpdateTitleText);
    }

    private void UpdateTitleText(UpdateTitleTextEvent eventData)
    {
        // Update the UI through EventBus event
        _titleTxt.text = eventData.titleText;
    }
}
