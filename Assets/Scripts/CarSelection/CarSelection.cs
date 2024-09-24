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
    [SerializeField] private Transform carHolder;
  
    [SerializeField]SoundManager soundManager;

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
        nextButton.interactable = (_index != carHolder.childCount - 1);

        for (int i = 0; i < carHolder.childCount; i++)
        {
            carHolder.GetChild(i).gameObject.SetActive(i == _index);
        }
    }

    public void ChangeCar(int _change)
    {
        Debug.Log(_change + currentCar);
        currentCar += _change;
        SelectCar(currentCar);
        soundManager.EnterPlayClip(true);
       
        /*if (currentCar > carHolder.childCount - 1)
        {
            currentCar = 0;
            
        }
        else if (currentCar < 0)
        {
            currentCar = carHolder.childCount - 1;
        }*/
    }
}
