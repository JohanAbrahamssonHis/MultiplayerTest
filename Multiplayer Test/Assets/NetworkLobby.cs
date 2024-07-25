using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetworkLobby : MonoBehaviour
{
    public static NetworkLobby Instance { get; private set; }

    private Lobby joinedLobby;
    private void Awake()
    {
        Instance = this;
        
        DontDestroyOnLoad(gameObject);
        
        InitializeUnityAuthentication();
    }

    private async void InitializeUnityAuthentication()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            InitializationOptions initializationOptions = new InitializationOptions();
            initializationOptions.SetProfile(Random.Range(0, 10000).ToString());
            
            await UnityServices.InitializeAsync(initializationOptions);

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    public async void CreateLobby(string lobbyName, bool isPrivate)
    {
        try
        {
            joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, 5, new CreateLobbyOptions
            {
                IsPrivate = isPrivate,
            });
            
            NetworkManager.Singleton.StartHost();
            TestingLobbyUI.LoadNetWork("Character Select Scene");
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }

    public async void QuickJoin()
    {
        try
        {
            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            NetworkManager.Singleton.StartClient();
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }
}
