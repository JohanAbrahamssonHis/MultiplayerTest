using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class SameNumberTest : NetworkBehaviour
{
    private int number;

    public TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        number = Random.Range(0, 100);
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsServer) return;
        SetTextClientRpc(number.ToString());
    }

    [ClientRpc]
    private void SetTextClientRpc(string newText)
    {
        text.text = newText;
    }
}
