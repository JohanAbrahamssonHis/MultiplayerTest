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

    private float heartbeatTimer;
    
    private void Awake()
    {
        Instance = this;
        
        DontDestroyOnLoad(gameObject);
        
        InitializeUnityAuthentication();
    }

    private void Update()
    {
        HandleHeatbeat();
    }

    private void HandleHeatbeat()
    {
        if (IsLobbyHost())
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer <= 0f)
            {
                float heartBeatTimerMax = 15f;
                heartbeatTimer = heartBeatTimerMax;

                LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }
        }
    }

    private bool IsLobbyHost()
    {
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
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

    public async void JoinWithCode(string lobbyCode)
    {
        try
        {
            joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
            NetworkManager.Singleton.StartClient();
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }

    public Lobby GetLobby()
    {
        return joinedLobby;
    }
}
