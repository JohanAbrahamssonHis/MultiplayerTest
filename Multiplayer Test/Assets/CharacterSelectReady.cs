using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectReady : NetworkBehaviour
{
    public static CharacterSelectReady Instance { get; private set; }

    public event EventHandler OnReadyChange;
    private Dictionary<ulong, bool> playerReadyDictionary;

    private void Awake()
    {
        Instance = this;
        playerReadyDictionary = new Dictionary<ulong, bool>();
    }

    public void SetPlayerReady()
    {
        SetPlayerReadyServerRpc();
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        SetPlayerReadyClientRpc(serverRpcParams.Receive.SenderClientId);
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;
        foreach (ulong clientID in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playerReadyDictionary.ContainsKey(clientID) || !playerReadyDictionary[clientID])
            {
                //Player is not ready
                allClientsReady = false;
                break;
            }
        }

        if (allClientsReady)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        }
    }

    [ClientRpc]
    private void SetPlayerReadyClientRpc(ulong clientId)
    {
        playerReadyDictionary[clientId] = true;
        
        OnReadyChange?.Invoke(this,EventArgs.Empty);
    }

    public bool IsPlayerReady(ulong clientId)
    {
        return playerReadyDictionary.ContainsKey(clientId) && playerReadyDictionary[clientId];
    }
}
