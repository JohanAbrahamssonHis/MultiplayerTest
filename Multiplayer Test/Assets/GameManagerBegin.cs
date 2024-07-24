using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class GameManagerBegin : NetworkBehaviour
{
    [SerializeField] private Transform playerPrefab;
    
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManagerOnOnLoadEventCompleted;
        }
            
    }

    private void SceneManagerOnOnLoadEventCompleted(string scenename, LoadSceneMode loadscenemode, List<ulong> clientscompleted, List<ulong> clientstimedout)
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            Transform playerTransform = Instantiate(playerPrefab);
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        }
    }
}
