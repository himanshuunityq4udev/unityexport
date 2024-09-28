using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [Header("Button Click Events Listener")]
    [SerializeField] GameEvent playButton;
    [SerializeField] GameEvent backButton;
    [SerializeField] GameEvent selectButton;
    [SerializeField] GameEvent settingsButton;
    [SerializeField] GameEvent discardButton;
    [SerializeField] GameEvent saveButton;



  
    UiRefrenceProvider uiRefrenceProvider;

    private void Start()
    {
        uiRefrenceProvider = GetComponent<UiRefrenceProvider>();
    }

    private void OnEnable()
    {
        playButton.AddListener(PlayButton);
        backButton.AddListener(BackButton);
        selectButton.AddListener(SelectButton);
        settingsButton.AddListener(SettingsButton);
        discardButton.AddListener(DiscardButton);
        saveButton.AddListener(SaveButton);
    }

    private void OnDisable()
    {
        playButton.RemoveListener(PlayButton);
        backButton.RemoveListener(BackButton);
        selectButton.RemoveListener(SelectButton);
        settingsButton.RemoveListener(SettingsButton);
        discardButton.RemoveListener(DiscardButton);
        saveButton.RemoveListener(SaveButton);
    }

    
    public void BackButton() 
    {
        uiRefrenceProvider._MenuController.PopPage();
        EventBus.Publish(new UpdateTitleTextEvent(uiRefrenceProvider._MenuController.GetCurrentPageName()));
    }
    public void PlayButton()
    {
        uiRefrenceProvider._MenuController.PushPage(uiRefrenceProvider.GetPageByName(PanelName.Garage.ToString()));
        EventBus.Publish(new UpdateTitleTextEvent(uiRefrenceProvider._MenuController.GetCurrentPageName()));
    }


    public void SettingsButton()
    {
        uiRefrenceProvider._MenuController.PushPage(uiRefrenceProvider.GetPageByName(PanelName.Popup.ToString()));
    }

    public void DiscardButton()
    {
        BackButton();
        
    }
    public void SaveButton()
    {
        BackButton();

    }

    public void SelectButton()
    {
        uiRefrenceProvider._MenuController.PushPage(uiRefrenceProvider.GetPageByName(PanelName.Mode.ToString()));
        EventBus.Publish(new UpdateTitleTextEvent(uiRefrenceProvider._MenuController.GetCurrentPageName()));

    }

}


public class UpdateTitleTextEvent
{
    public string titleText;

    public UpdateTitleTextEvent(string _titleText)
    {
        titleText = _titleText;
    }
}