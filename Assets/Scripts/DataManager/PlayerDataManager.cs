using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : Singleton<PlayerDataManager>
{
    [SerializeField] PlayerDataHolder playerDataHolder;
    [SerializeField] GameDataHolder gameDataHolder;

  
    public PlayerDataHolder PlayerDataHolder { get => playerDataHolder; set => playerDataHolder = value; }


    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        LoadData();
    }

  
    [ContextMenu("SaveData")]
    public async void SaveData()
    {
        try
        {

            bool saveSuccess = await DataSaver.SaveDataAsync(playerDataHolder.playerInfo, "players");
            if (saveSuccess)
            {
                // Debug.Log("SaveCall");
                LoadData();
                // UpdateCloudData();
            }
            else
            {
                Debug.Log("No data to be saved");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving data: {e.Message}");
        }
    }


    [ContextMenu("Load Data")]
    public async void LoadData()
    {
        List<int> carUnlockedIndices = new List<int>();

        for (int i = 0; i < gameDataHolder.unlockedCars.Length; i++)
        {
            if (gameDataHolder.unlockedCars[i] == true)
            {
                Debug.Log("Himanshu " + i);
                carUnlockedIndices.Add(i);  // Store the index of the unlocked car
            }
        }
        try
        {
            // Debug.Log("Load call");

            playerDataHolder.playerInfo = await DataSaver.LoadDataAsync<PlayerInfo>("players");

            if (playerDataHolder.playerInfo == null)
            {
                playerDataHolder.playerInfo = new PlayerInfo
                {
                    Name = "Himanshu",
                    DimandPoints = 0,
                    HighScore = 0,
                    Coins = 500,
                    carUnlocked = carUnlockedIndices // Initialize with a list and add car 5
                };
                SaveData();
            }
            else
            {
                for (int i = 0; i < playerDataHolder.playerInfo.carUnlocked.Count; i++)
                {
                    gameDataHolder.unlockedCars[playerDataHolder.playerInfo.carUnlocked[i]] = true;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading data: {e.Message}");
        }
        EventBus.Publish(new UpdateCoinTextEvent(playerDataHolder.playerInfo.Coins.ToString()));
    }
}

public class UpdateCoinTextEvent
{
    public string coinText;

    public UpdateCoinTextEvent(string _coinText)
    {
        coinText = _coinText;
    }
}
