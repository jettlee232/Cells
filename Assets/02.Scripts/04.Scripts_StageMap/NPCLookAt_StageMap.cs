using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLookAt_StageMap : MonoBehaviour
{
    private Transform player;
    void Start()
    {
        player = GameManager_StageMap.instance.GetPlayer().transform;
    }

    void Update()
    {
        transform.LookAt(player);
    }
}
