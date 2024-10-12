using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarSelection :MonoBehaviour
{
    [Header("Navigation Buttons")]
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button SelectButton;
    [SerializeField] private Button UnlockButton;
    [SerializeField] private TMP_Text carPriceText;
    [SerializeField] SoundManager soundManager;
    [SerializeField] GameDataHolder carHolder;
    [SerializeField] Transform carHolderParent;

    [SerializeField] GameEvent unlockButtonEvent;
    [SerializeField] GameEvent needCoinSPanel;
    [SerializeField] GameEvent confirmBuyPanel;
    [SerializeField] GameEvent confirmUnlock;


    [Header("Car Attributes")]
    private int currentCar = 0;

    public int CurrentCar { get => currentCar; set => currentCar = value; }

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
        SelectCar(currentCar);
        
    }

    void Start()
    {
        // Add listeners to the sliders
        previousButton.onClick.AddListener(delegate { ChangeCar(-1); });
        nextButton.onClick.AddListener(delegate { ChangeCar(1); });
    }


    private void OnEnable()
    {
        unlockButtonEvent.AddListener(UnlockCarButton);
        confirmUnlock.AddListener(ConfirmUnlock);
    }

    private void OnDisable()
    {
        unlockButtonEvent.RemoveListener(UnlockCarButton);
        confirmUnlock.RemoveListener(ConfirmUnlock);

    }

    private void SelectCar(int _index)
    {
        previousButton.interactable = (_index != 0);
        nextButton.interactable = (_index != carHolderParent.childCount - 1);
        if (!carHolder.unlockedCars[currentCar])
        {
            carPriceText.transform.parent.gameObject.SetActive(true);
            carPriceText.text = "\u20B9 " + carHolder.carsPrice[currentCar].ToString();
        }
        else
        {
            carPriceText.transform.parent.gameObject.SetActive(false);
        }
        if (carHolder.unlockedCars[currentCar])
        {
            SelectButton.gameObject.SetActive(true);
            UnlockButton.gameObject.SetActive(false);

        }
        else
        {
            UnlockButton.gameObject.SetActive(true);
            SelectButton.gameObject.SetActive(false);
        }

        for (int i = 0; i < carHolderParent.childCount; i++)
        {
            carHolderParent.GetChild(i).gameObject.SetActive(i == _index);
            
        }
    }

    public void UnlockCarButton()
    {
        BuyCar(carHolder.carsPrice[currentCar]);
    }
    public void BuyCar(int carPrice)
    {
        int coins = PlayerDataManager.Instance.PlayerDataHolder.playerInfo.Coins;
        if (coins >= carPrice)
        {
            confirmBuyPanel.Invoke();
        }
        else
        {
            needCoinSPanel.Invoke();
        }
    }

    public void ConfirmUnlock()
    {
        SelectButton.gameObject.SetActive(true);
        UnlockButton.gameObject.SetActive(false);
        PlayerDataManager.Instance.PlayerDataHolder.playerInfo.Coins -= carHolder.carsPrice[currentCar];
        PlayerDataManager.Instance.PlayerDataHolder.playerInfo.carUnlocked.Add(currentCar);
        PlayerDataManager.Instance.SaveData();

    }


    public void ChangeCar(int _change)
    {
        currentCar += _change;
        SelectCar(currentCar);
        soundManager.EnterPlayClip(true);
    }
}
