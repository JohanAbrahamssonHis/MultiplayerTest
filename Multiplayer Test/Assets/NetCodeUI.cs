using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetCodeUI : MonoBehaviour
{
    public Button startHostButton;
    public Button startClientButton;


    private void Awake()
    {
        startHostButton.onClick.AddListener(() =>
        {
            Debug.Log("Host");
            NetworkManager.Singleton.StartHost();
            Hide();
        });
        startClientButton.onClick.AddListener(() =>
        {
            Debug.Log("Client");
            NetworkManager.Singleton.StartClient();
            Hide();
        });
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
