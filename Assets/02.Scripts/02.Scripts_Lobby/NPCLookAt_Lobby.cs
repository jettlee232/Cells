using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLookAt_Lobby : MonoBehaviour
{
    private Transform player;
    void Start()
    {
        player = GameManager_Lobby.instance.GetPlayer().transform;
    }

    void Update()
    {
        transform.LookAt(player);
    }
}
