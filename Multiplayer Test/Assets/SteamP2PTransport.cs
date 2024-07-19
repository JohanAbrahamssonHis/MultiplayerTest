using System;
using Unity.Netcode;
using Steamworks;
using UnityEngine;

public class SteamP2PTransport : NetworkTransport
{
    public override ulong ServerClientId => 0;
    
    public override void Send(ulong clientId, ArraySegment<byte> data, NetworkDelivery delivery)
    {
        var steamID = new CSteamID(clientId);
        bool result = SteamNetworking.SendP2PPacket(steamID, data.Array, (uint)data.Count, EP2PSend.k_EP2PSendReliable);
        Debug.Log($"Send result: {result}, Client ID: {clientId}, Data Length: {data.Count}");
    }

    public override NetworkEvent PollEvent(out ulong clientId, out ArraySegment<byte> payload, out float receiveTime)
    {
        uint msgSize;
        if (SteamNetworking.IsP2PPacketAvailable(out msgSize))
        {
            byte[] msg = new byte[msgSize];
            CSteamID steamIDRemote;
            if (SteamNetworking.ReadP2PPacket(msg, msgSize, out msgSize, out steamIDRemote))
            {
                clientId = (ulong)steamIDRemote.m_SteamID;
                payload = new ArraySegment<byte>(msg, 0, (int)msgSize);
                receiveTime = Time.realtimeSinceStartup;
                Debug.Log($"Received message from {clientId}, Size: {msgSize}");
                return NetworkEvent.Data;
            }
        }

        clientId = 0;
        payload = default;
        receiveTime = 0;
        return NetworkEvent.Nothing;
    }

    public override bool StartClient()
    {
        Debug.Log("Starting Steam P2P client...");
        return true;
    }

    public override bool StartServer()
    {
        Debug.Log("Starting Steam P2P server...");
        return true;
    }

    public override void DisconnectRemoteClient(ulong clientId)
    {
        var steamID = new CSteamID(clientId);
        SteamNetworking.CloseP2PSessionWithUser(steamID);
        Debug.Log($"Disconnected remote client {clientId}");
    }

    public override void DisconnectLocalClient()
    {
        SteamNetworking.CloseP2PSessionWithUser(SteamUser.GetSteamID());
        Debug.Log("Disconnected local client");
    }

    public override ulong GetCurrentRtt(ulong clientId)
    {
        // Implement actual RTT measurement if needed.
        return 50;
    }

    public override void Initialize(NetworkManager networkManager = null)
    {
        Debug.Log("Initializing Steam P2P transport...");
    }

    public override void Shutdown()
    {
        Debug.Log("Shutting down Steam P2P transport...");
    }
}
