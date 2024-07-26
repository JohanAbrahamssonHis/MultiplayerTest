using Steamworks;
using UnityEngine;
using Unity.Netcode;

public class NetworkManagerSetup : MonoBehaviour
{
    void Awake()
    {
        if (NetworkManager.Singleton != null)
        {
            Destroy(gameObject);
        }
    }
}