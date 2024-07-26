using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using Unity.Netcode;
using UnityEngine;

public class GeneralManager : NetworkBehaviour
{
    public static GeneralManager Instance { get; private set; }
    private string playerName;
    public const string PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER = "PlayerNameMultiplayer";

    public EventHandler OnPlayerDataNetworkListChanged;
    
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        playerDataNetworkList = new NetworkList<PlayerData>();
        playerDataNetworkList.OnListChanged += PlayerDataNetworkListOnOnListChanged;
        playerName = SteamManager.Initialized ? SteamFriends.GetPersonaName() : PlayerPrefs.GetString(PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER, "PlayerName" + UnityEngine.Random.Range(100,10000));
    }

    private void PlayerDataNetworkListOnOnListChanged(NetworkListEvent<PlayerData> changeEvent)
    {
       OnPlayerDataNetworkListChanged?.Invoke(this,EventArgs.Empty);
    }

    private NetworkList<PlayerData> playerDataNetworkList;

    public void StartHost()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManagerOnClientConnectedCallback;
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    private void NetworkManagerOnClientConnectedCallback(ulong clientId)
    {
        playerDataNetworkList.Add(new PlayerData
        {
            clientId = clientId,
        });
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

    public bool IsPlayerIndexConnected(int playerIndex)
    {
        return playerIndex < playerDataNetworkList.Count;
    }
}
