using Steamworks;
using UnityEngine;
using Unity.Netcode;

public class NetworkManagerSetup : MonoBehaviour
{
    public NetworkManager networkManager;
    public SteamP2PTransport steamP2PTransport;

    void Start()
    {
        // Assign the SteamP2PTransport to the NetworkManager
        networkManager.NetworkConfig.NetworkTransport = steamP2PTransport;

        // Start the server or client based on your requirement
        //networkManager.StartServer(); // or networkManager.StartClient();
    }
}