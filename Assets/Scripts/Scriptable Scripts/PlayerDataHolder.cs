using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/PlayerData", fileName = "PlayerDataHolder")]

public class PlayerDataHolder : ScriptableObject
{
    public PlayerInfo playerInfo;
   
}

[System.Serializable]
public class PlayerInfo
{
    public string Name;
    public int HighScore;
    public int Coins;
    public int DimandPoints;
    public List<int> carUnlocked;

}
