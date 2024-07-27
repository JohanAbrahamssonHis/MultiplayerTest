using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class NetworkLobby : MonoBehaviour
{
    public static NetworkLobby Instance { get; private set; }

    public event EventHandler<OnLobbyListChagnedEventArgs> OnLobbyListChanged; 
    public class OnLobbyListChagnedEventArgs : EventArgs
    {
        public List<Lobby> lobbyList;
    }
    
    private Lobby joinedLobby;

    private float heartbeatTimer;
    private float listLobbiesTimer;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        
        DontDestroyOnLoad(gameObject);
        
        InitializeUnityAuthentication();
    }

    private void Update()
    {
        HandleHeatbeat();
        HandlePeriodicListLobbies();
    }

    private void HandlePeriodicListLobbies()
    {
        if(joinedLobby != null && 
           !AuthenticationService.Instance.IsSignedIn &&
           SceneManager.GetActiveScene().name != "Lobby Scene") return;
        
        
        listLobbiesTimer -= Time.deltaTime;
        if (listLobbiesTimer <= 0f)
        {
            float listLobbiesTimerMax = 3f;
            listLobbiesTimer = listLobbiesTimerMax;
            ListLobbies();
        }
            
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

    private  async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Filters = new List<QueryFilter>
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
                }
            };
            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            OnLobbyListChanged?.Invoke(this, new OnLobbyListChagnedEventArgs()
            {
                lobbyList = queryResponse.Results
            });
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

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
            
            GeneralManager.Instance.StartHost();
            TestingLobbyUI.LoadNetWork("Character Select Scene");
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }

    public async void DestroyLobby()
    {
        if (joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);

                joinedLobby = null;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
        }
    }

    public async void LeaveLobby()
    {
        if (joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);

                joinedLobby = null;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
        }
    }
    
    public async void KickPlayer(string playerId)
    {
        if (IsLobbyHost())
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);

                joinedLobby = null;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
        }
    }

    public async void QuickJoin()
    {
        try
        {
            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            GeneralManager.Instance.StartClient();
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }

    public async void JoinWithId(string lobbyId)
    {
        try
        {
            joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
            GeneralManager.Instance.StartClient();
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
            GeneralManager.Instance.StartClient();
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
