using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class LobbyCreateUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button createPublicButton;
    [SerializeField] private Button createPrivateButton;
    [SerializeField] private TMP_InputField lobbyNameInputField;

    private void Awake()
    {
        createPrivateButton.onClick.AddListener((() =>
        {
            NetworkLobby.Instance.CreateLobby(lobbyNameInputField.text, true);
        }));
        createPublicButton.onClick.AddListener((() =>
        {
            NetworkLobby.Instance.CreateLobby(lobbyNameInputField.text, false);
        }));
        closeButton.onClick.AddListener(Hide);
    }

    private void Start()
    {
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
