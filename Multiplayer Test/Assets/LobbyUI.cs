using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button quickJoinButton;
    [SerializeField] private Button joinCodeButton;
    [SerializeField] private TMP_InputField joinCodeInputField;
    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private LobbyCreateUI _lobbyCreateUI;
    [SerializeField] private Transform _lobbyContainter;
    [SerializeField] private Transform _lobbyTemplate;
    

    private void Awake()
    {
        mainMenuButton.onClick.AddListener((() =>
        {
            NetworkLobby.Instance.LeaveLobby();
            SceneManager.LoadScene("MainMenu");
        }));
        createLobbyButton.onClick.AddListener((() =>
        {
            _lobbyCreateUI.Show();
        }));
        quickJoinButton.onClick.AddListener((() =>
        {
            NetworkLobby.Instance.QuickJoin();
        }));
        joinCodeButton.onClick.AddListener((() =>
        {
            NetworkLobby.Instance.JoinWithCode(joinCodeInputField.text);
        }));
        
        _lobbyTemplate.gameObject.SetActive(false);
    }
    
    public static void LoadNetWork(string targetScene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene, LoadSceneMode.Single);
    }

    private void Start()
    {
        playerNameInputField.text = GeneralManager.Instance.GetPlayername();
        playerNameInputField.onValueChanged.AddListener((string newText) =>
        {
            GeneralManager.Instance.SetPlayername(newText);
        });
        
        NetworkLobby.Instance.OnLobbyListChanged += InstanceOnOnLobbyListChanged;
        UpdateLobbyList(new List<Lobby>());
    }

    private void InstanceOnOnLobbyListChanged(object sender, NetworkLobby.OnLobbyListChagnedEventArgs e)
    {
        UpdateLobbyList(e.lobbyList);
    }

    private void UpdateLobbyList(List<Lobby> lobbyList)
    {
        foreach (Transform child in _lobbyContainter)
        {
            if(child == _lobbyTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (Lobby lobby in lobbyList)
        {
            Transform lobbyTransform = Instantiate(_lobbyTemplate, _lobbyContainter);
            lobbyTransform.gameObject.SetActive(true);
            lobbyTransform.GetComponent<LobbyListSingleUI>().SetLobby(lobby);
        }
    }

    private void OnDestroy()
    {
        NetworkLobby.Instance.OnLobbyListChanged -= InstanceOnOnLobbyListChanged;
    }
}
