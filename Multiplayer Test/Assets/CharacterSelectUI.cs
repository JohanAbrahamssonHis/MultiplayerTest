using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button readyButton;

    private void Awake()
    {
        backButton.onClick.AddListener((() =>
        {
            //NOTE MUST BE ADDED TO WORK WITH A MAIN MENU (AKA LEAVING THE MULTIPLAYER PART)
            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.SceneManager.LoadScene("Lobby Scene", LoadSceneMode.Single);
            
        }));
        readyButton.onClick.AddListener((() =>
        {
            CharacterSelectReady.Instance.SetPlayerReady();
        }));
    }
}
