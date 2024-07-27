using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterSelectPlayer : MonoBehaviour
{
    [SerializeField] private int index;
    [SerializeField] private TextMeshProUGUI ready;
    [SerializeField] private TextMeshProUGUI name;
    
    private void Start()
    {
        GeneralManager.Instance.OnPlayerDataNetworkListChanged += OnPlayerDataNetworkListChanged;
        CharacterSelectReady.Instance.OnReadyChange += InstanceOnOnReadyChange;
        
        UpdatePlayer();
    }

    private void InstanceOnOnReadyChange(object sender, EventArgs e)
    {
        UpdatePlayer();
    }

    private void OnPlayerDataNetworkListChanged(object sender, EventArgs e)
    {
        UpdatePlayer();
    }

    private void UpdatePlayer()
    {
        if (GeneralManager.Instance.IsPlayerIndexConnected(index))
        {
            Show();

            PlayerData playerData = GeneralManager.Instance.GetPlayerDataFromPlayerIndex(index);
            ready.text = CharacterSelectReady.Instance.IsPlayerReady(playerData.clientId) ? "Ready" : "Unready";

            name.text = playerData.playerName.ToString();

        }
        else
        {
            Hide();
        }
    }

    public void OnDestroy()
    {
        GeneralManager.Instance.OnPlayerDataNetworkListChanged -= OnPlayerDataNetworkListChanged;
        CharacterSelectReady.Instance.OnReadyChange -= InstanceOnOnReadyChange;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
