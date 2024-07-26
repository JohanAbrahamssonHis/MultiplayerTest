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

    private void Start()
    {
        CharacterSelectReady.Instance.OnReadyChange += InstanceOnOnReadyChange;
    }

    private void InstanceOnOnReadyChange(object sender, EventArgs e)
    {
        UpdatePlayerReady();
    }

    private void UpdatePlayerReady()
    {
        playerInformationVisualRpc();
    }
    
    private void playerInformationVisualRpc()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].SetActive(i<NetworkManager.Singleton.ConnectedClients.Count);
            //readyTexts[i].text = CharacterSelectReady.Instance.IsPlayerReady() ? "Ready" : "UnReady";
        }
        
        //5:06
    }
}
