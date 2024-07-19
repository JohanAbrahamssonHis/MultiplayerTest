using Unity.Netcode;
using UnityEngine;

public class NetworkTest : NetworkBehaviour
{
    /*
    void Start()
    {
        if (IsServer)
        {
            InvokeRepeating(nameof(SendTestMessage), 2.0f, 2.0f); // Send a test message every 2 seconds
        }
    }

    [ServerRpc]
    void SendTestMessage()
    {
        Debug.Log("Server sending test message to all clients.");
        TestMessageClientRpc();
    }

    [ClientRpc]
    void TestMessageClientRpc()
    {
        Debug.Log("Client received test message from server.");
    }
    */
}