using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/GameData" , fileName ="GameDataHolder")]
public class GameDataHolder : ScriptableObject
{
    [Header("Cars Price")]
    public List<int> carsPrice = new List<int>();

    [Header("Unlocked Cars")]
    public bool[] unlockedCars = new bool[10];

}


