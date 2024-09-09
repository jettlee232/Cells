using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapTurnController_Home : MonoBehaviour
{
    public GameObject smoothTurnBtn;
    public GameObject snapTurnBtn;

    public PlayerMoving_Lobby lobby;
    public PlayerMoving_StageMap stageMap;
    public PlayerMoving_Mito mito;
    public PlayerMoving_Lys lys;

    void Start()
    {
        if (smoothTurnBtn == null) smoothTurnBtn = transform.Find("SmoothTurnBtn").gameObject;
        if (snapTurnBtn == null) snapTurnBtn = transform.Find("SnapTurnBtn").gameObject;

        lobby = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerMoving_Lobby>();
        stageMap = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerMoving_StageMap>();
        mito = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerMoving_Mito>();
        lys = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerMoving_Lys>();
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
            /*
            if (lobby != null)
                lobby.snapturn = true;

            if (stageMap != null)
                stageMap.snapturn = true;


            if (lys != null)
                lys.snapturn = true;
            */
            if (mito != null)
                mito.snapturn = true;
        }
        else
        {
            /*
            if (lobby != null)
                lobby.snapturn = false;

            if (stageMap != null)
                stageMap.snapturn = false;


            if (lys != null)
                lys.snapturn = false;
            */
            if (mito != null)
                mito.snapturn = false;
        }

    }
}
