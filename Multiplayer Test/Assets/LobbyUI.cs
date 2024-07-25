using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
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
    [SerializeField] private LobbyCreateUI _lobbyCreateUI;
    

    private void Awake()
    {
        mainMenuButton.onClick.AddListener((() =>
        {
            LoadNetWork("Character Select Scene");
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
    }
    
    public static void LoadNetWork(string targetScene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene, LoadSceneMode.Single);
    }
}
