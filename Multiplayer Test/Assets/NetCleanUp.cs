using Steamworks;
using UnityEngine;
using Unity.Netcode;

public class NetCleanUp : MonoBehaviour
{
    void Awake()
    {
        if (NetworkManager.Singleton != null)
        {
            Destroy(NetworkManager.Singleton.gameObject);
        }

        if (GeneralManager.Instance != null)
        {
            Destroy(GeneralManager.Instance.gameObject);
        }
        if (NetworkLobby.Instance != null)
        {
            Destroy(NetworkLobby.Instance.gameObject);
        }
    }
}