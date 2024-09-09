using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController_Home : MonoBehaviour
{
    public GameObject fixedTeleport;
    public GameObject offSwitch;
    public GameObject onSwitch;

    public PlayerMoving_Lobby lobby;
    public PlayerMoving_StageMap stageMap;
    public PlayerMoving_Mito mito;
    public PlayerMoving_Lys lys;
    public PlayerMoving_MT multi;

    void Start()
    {
        if (offSwitch == null) offSwitch = transform.Find("Switch_Off").gameObject;
        if (onSwitch == null) onSwitch = transform.Find("Switch_On").gameObject;

        lobby = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerMoving_Lobby>();
        stageMap = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerMoving_StageMap>();
        mito = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerMoving_Mito>();
        lys = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerMoving_Lys>();
        multi = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerMoving_MT>();
    }

    void Update()
    {
        if (onSwitch.activeSelf)
        {
            fixedTeleport.SetActive(true);
            fixedTeleport.GetComponent<Teleport_Home>().canTeleport = true;

            SetMoving(true);
        }
        else
        {
            fixedTeleport.SetActive(false);
            fixedTeleport.GetComponent<Teleport_Home>().canTeleport = false;

            SetMoving(false);
        }
    }

    public void SetMoving(bool flag)
    {
        if (flag)
        {
            if (lobby != null)
                GameManager_Lobby.instance.StopPlayer();

            if (stageMap != null)
                GameManager_StageMap.instance.DisableMove();

            if (mito != null)
                mito.isMoving = false;

            if (lys != null)
                GameManager_Lys.instance.DisableMove();

            if (multi != null)
                multi.isMoving = false;
        }
        else
        {
            if (lobby != null)
                GameManager_Lobby.instance.EnableMovePlayer();

            if (stageMap != null)
                GameManager_StageMap.instance.EnableMove();

            if (mito != null)
                mito.isMoving = true;

            if (lys != null)
                GameManager_Lys.instance.EnableMove();

            if (multi != null)
                multi.isMoving = true;
        }
        
    }
}