using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapTurnController_Home : MonoBehaviour
{
    public GameObject smoothTurnBtn;
    public GameObject snapTurnBtn;

    public PlayerMoving_Lobby lobby;
    public PlayerSnapTurnCtrl_CM cm;
    public PlayerMoving_StageMap stageMap;
    public PlayerMoving_Mito mito;
    public PlayerMoving_Lys lys;
    public PlayerMoving_MT multi;

    void Start()
    {
        if (smoothTurnBtn == null) smoothTurnBtn = transform.Find("SmoothTurnBtn").gameObject;
        if (snapTurnBtn == null) snapTurnBtn = transform.Find("SnapTurnBtn").gameObject;

        lobby = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerMoving_Lobby>();
        cm = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerSnapTurnCtrl_CM>();
        stageMap = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerMoving_StageMap>();
        mito = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerMoving_Mito>();
        lys = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerMoving_Lys>();
        multi = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerMoving_MT>();
    }

    void Update()
    {
        if (snapTurnBtn.activeSelf)
        {
            SetTurn(true);
        }
        else
        {
            SetTurn(false);
        }
    }

    public void SetTurn(bool flag)
    {
        if (flag)
        {
            if (lobby != null)
                lobby.snapturn = true;

            if (cm != null)
                cm.OnSnapTurn();

            if (stageMap != null)
                stageMap.snapturn = true;

            if (mito != null)
                mito.snapturn = true;

            if (lys != null)
                lys.snapturn = true;

            if (multi != null)
                multi.snapturn = true;
        }
        else
        {
            if (lobby != null)
                lobby.snapturn = false;

            if (cm != null)
                cm.OnSmoothTurn();

            if (stageMap != null)
                stageMap.snapturn = false;

            if (mito != null)
                mito.snapturn = false;

            if (lys != null)
                lys.snapturn = false;

            if (multi != null)
                multi.snapturn = false;
        }

    }
}
