using TMPro;
using UnityEngine;

public class UiView : MonoBehaviour
{
    [Header("Component Refrence")]
    [SerializeField] private TMP_Text _titleTxt;
    [SerializeField] private TMP_Text _coinText;


    private void OnEnable()
    {
        EventBus.Subscribe<UpdateTitleTextEvent>(UpdateTitleText);
        EventBus.Subscribe<UpdateCoinTextEvent>(UpdateCoinText);

    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<UpdateTitleTextEvent>(UpdateTitleText);
        EventBus.Unsubscribe<UpdateCoinTextEvent>(UpdateCoinText);
    }


    private void UpdateCoinText(UpdateCoinTextEvent eventData)
    {
        _coinText.text = "\u20B9 " + eventData.coinText;
    }

    private void UpdateTitleText(UpdateTitleTextEvent eventData)
    {
        // Update the UI through EventBus event
        _titleTxt.text = eventData.titleText;
    }


    private void Update()
    {
       // UpdateNumberText(MoneyManager.Instance.playerInfo.money);
    }


    public void UpdateNumberText(int number)
    {
       // AssetRefrence.Instance.CoinTxt.text = FormatNumber(number);
        //AssetRefrence.Instance.CoinTxt2.text = FormatNumber(number);
    }

    private string FormatNumber(int number)
    {
        if (number >= 1000)
        {
            float shortenedNumber = number / 1000f;
            return shortenedNumber.ToString("0.##") + "K";
        }
        else
        {
            return number.ToString();
        }
    }


}
