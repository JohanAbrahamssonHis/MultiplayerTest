using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;

public class GeneralManager : NetworkBehaviour
{
    public static GeneralManager Instance { get; private set; }
    private string playerName;
    public const string PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER = "PlayerNameMultiplayer";

    public EventHandler OnPlayerDataNetworkListChanged;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
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
        NetworkManager.Singleton.OnClientDisconnectCallback += SingletonOnOnClientDisconnectCallback;
        NetworkManager.Singleton.StartHost();
    }

    private void SingletonOnOnClientDisconnectCallback(ulong clientId)
    {
        for (int i = 0; i < playerDataNetworkList.Count; i++)
        {
            PlayerData playerData = playerDataNetworkList[i];
            if (playerData.clientId == clientId)
            {
                 //Disconnected
                 playerDataNetworkList.RemoveAt(i);
            }
        }
    }

    public void StartClient()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += SingletonOnOnClientConnectedCallback;
        NetworkManager.Singleton.StartClient();
    }

    private void SingletonOnOnClientConnectedCallback(ulong clientId)
    {
        SetPlayerNameServerRpc(GetPlayername());
        SetPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerNameServerRpc(string playerName, ServerRpcParams serverRpcParams = default)
    {
        int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

        PlayerData playerData = playerDataNetworkList[playerDataIndex];

        playerData.playerName = playerName;

        playerDataNetworkList[playerDataIndex] = playerData;
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerIdServerRpc(string playerId, ServerRpcParams serverRpcParams = default)
    {
        int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

        PlayerData playerData = playerDataNetworkList[playerDataIndex];

        playerData.playerId = playerId;

        playerDataNetworkList[playerDataIndex] = playerData;
    }

    private void NetworkManagerOnClientConnectedCallback(ulong clientId)
    {
        playerDataNetworkList.Add(new PlayerData
        {
            clientId = clientId,
        });
        SetPlayerNameServerRpc(GetPlayername());
        SetPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);
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

    public int GetPlayerDataIndexFromClientId(ulong clientId)
    {
        for (int i = 0; i < playerDataNetworkList.Count; i++)
        {
            if (playerDataNetworkList[i].clientId == clientId)
            {
                return i;
            }
        }

        return -1;
    }
    
    public PlayerData GetPlayerDataFromClientId(ulong clientId)
    {
        foreach (var playerData in playerDataNetworkList)
        {
            if (playerData.clientId == clientId)
            {
                return playerData;
            }
        }

        return default;
    }

    public PlayerData GetPlayerData()
    {
        return GetPlayerDataFromClientId(NetworkManager.Singleton.LocalClientId);
    }
    
    public PlayerData GetPlayerDataFromPlayerIndex(int playerIndex)
    {
        return playerDataNetworkList[playerIndex];
    }
}
