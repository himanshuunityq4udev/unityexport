using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Car/CarHolder")]
public class CarHoler : ScriptableObject
{
    [Header("Cars Price")]
    public List<int> carsPrice = new List<int>();

    [Header("Unlocked Cars")]
    public bool[] unlockedCard = new bool[10];

}


