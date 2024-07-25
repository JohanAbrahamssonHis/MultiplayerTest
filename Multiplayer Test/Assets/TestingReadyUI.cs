using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.UI;

public class TestingReadyUI : MonoBehaviour
{
    [SerializeField] private Button readyButton;
    [SerializeField] private List<GameObject> players;
    [SerializeField] private List<TextMeshProUGUI> readyTexts;

    private void Awake()
    {
        readyButton.onClick.AddListener((() =>
        {
            CharacterSelectReady.Instance.SetPlayerReady();
        }));
    }

    private void Update()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].SetActive(i<NetworkManager.Singleton.ConnectedClients.Count);
        }
        List<bool> playersReady = CharacterSelectReady.Instance.GetPlayersReady();
        Debug.Log(playersReady.Count);
        for (int i = 0; i < players.Count(x => x.activeSelf); i++)
        {
            readyTexts[i].text = playersReady[i] ? "Ready" : "UnReady";
        }
    }
}
