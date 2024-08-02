using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLookAt_Lys : MonoBehaviour
{
    private Transform player;
    void Start()
    {
        player = GameManager_Lys.instance.GetPlayer().transform;
    }

    void Update()
    {
        transform.LookAt(player);
    }
}
