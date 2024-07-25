using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyDetailsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI lobbyCodeText;

    private void Start()
    {
        Lobby lobby = NetworkLobby.Instance.GetLobby();

        lobbyNameText.text = "Lobby Name: " + lobby.Name;
        lobbyCodeText.text = "Lobby Code: " + lobby.LobbyCode;
    }
}
