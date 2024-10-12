using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{

    [Header("Button Click Events Listener")]
    [SerializeField] GameEvent playButton;
    [SerializeField] GameEvent backButton;
    [SerializeField] GameEvent selectButton;
    [SerializeField] GameEvent settingsButton;
    [SerializeField] GameEvent discardButton;
    [SerializeField] GameEvent saveButton;
    [SerializeField] GameEvent needCoinSPanel;
    [SerializeField] GameEvent confirmBuyPanel;



    UiRefrenceProvider uiRefrenceProvider;

    [SerializeField] GameObject[] popupPanels;
    [SerializeField] GameObject closeButton;
    [SerializeField] TMP_Text titleText;
    List<string> titles = new List<string> {"SETTINGS", "NEED COINS", "UNLOCK CAR", "PURCHASED", "REWARDS", "CAMERA PERMISSION", "AR NOT SUPPORTED" };


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
        needCoinSPanel.AddListener(NeedCoinPanel);
        confirmBuyPanel.AddListener(ConfirmBuyPanel);
    }

    private void OnDisable()
    {
        playButton.RemoveListener(PlayButton);
        backButton.RemoveListener(BackButton);
        selectButton.RemoveListener(SelectButton);
        settingsButton.RemoveListener(SettingsButton);
        discardButton.RemoveListener(DiscardButton);
        saveButton.RemoveListener(SaveButton);
        needCoinSPanel.RemoveListener(NeedCoinPanel);
        confirmBuyPanel.RemoveListener(ConfirmBuyPanel);

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
        ActivateDeactivatePanels();
        closeButton.SetActive(false);
        titleText.text = titles[0];
        popupPanels[0].gameObject.SetActive(true);
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


    public void NeedCoinPanel()
    {
        ActivateDeactivatePanels();
        titleText.text = titles[1];
        popupPanels[1].gameObject.SetActive(true);
        uiRefrenceProvider._MenuController.PushPage(uiRefrenceProvider.GetPageByName(PanelName.Popup.ToString()));
    }

    public void ConfirmBuyPanel()
    {
        ActivateDeactivatePanels();
        titleText.text = titles[2];
        popupPanels[2].gameObject.SetActive(true);
        uiRefrenceProvider._MenuController.PushPage(uiRefrenceProvider.GetPageByName(PanelName.Popup.ToString()));
    }

    public void ActivateDeactivatePanels()
    {
        closeButton.SetActive(true);
        foreach (GameObject panel in popupPanels)
        {
            panel.SetActive(false);
        }
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