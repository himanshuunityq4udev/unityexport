using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarSelection : MonoBehaviour
{
    [Header("Navigation Buttons")]
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button SelectButton;
    [SerializeField] private Button UnlockButton;
    [SerializeField] private TMP_Text carPriceText;
    [SerializeField]SoundManager soundManager;
    [SerializeField] GameDataHolder carHolder;
    [SerializeField] Transform carHolderParent;


    [Header("Car Attributes")]
    private int currentCar = 0;

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

    private void SelectCar(int _index)
    {
        previousButton.interactable = (_index != 0);
        nextButton.interactable = (_index != carHolderParent.childCount - 1);
        carPriceText.text = carHolder.carsPrice[currentCar].ToString();

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

    public void ChangeCar(int _change)
    {
        currentCar += _change;
        SelectCar(currentCar);
        soundManager.EnterPlayClip(true);
    }
}
