using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class GeneralManager : MonoBehaviour
{
    public static GeneralManager Instance { get; private set; }
    private string playerName;
    public const string PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER = "PlayerNameMultiplayer";

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        playerName = SteamManager.Initialized ? SteamFriends.GetPersonaName() : PlayerPrefs.GetString(PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER, "PlayerName" + UnityEngine.Random.Range(100,10000));
    }

    public string GetPlayername()
    {
        return playerName;
    }
    
    public void SetPlayername(string playerName)
    {
        this.playerName = playerName;
        
        PlayerPrefs.SetString(PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER, playerName);
    }
}
