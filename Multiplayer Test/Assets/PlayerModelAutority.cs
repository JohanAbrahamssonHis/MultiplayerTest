using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerModelAutority : NetworkBehaviour
{
    public GameObject OnlySelf;
    public GameObject OnlyOther;
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        OnlySelf.SetActive(IsOwner);
        OnlyOther.SetActive(!IsOwner);
    }
}
