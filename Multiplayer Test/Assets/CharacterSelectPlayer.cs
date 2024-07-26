using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterSelectPlayer : MonoBehaviour
{
    [SerializeField] private int index;
    [SerializeField] private TextMeshProUGUI ready;
    
    private void Start()
    {
        GeneralManager.Instance.OnPlayerDataNetworkListChanged += OnPlayerDataNetworkListChanged;
        
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
        }
        else
        {
            Hide();
        }
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
