﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBeingHeldOrNot_CM : MonoBehaviour
{
    public TutorialManager_CM tutoMgr;

    public float revortPos = -.5f;

    public BNG.Grabbable grabbable;
    public Rigidbody rb;
    public Transform objRespawnPoint1;
    public Transform objRespawnPoint2;
    public Quaternion objSpawnRotate;

    public bool isHeld = false;
    public int statusFlag = 0;

    [Header("Box Colliders")]
    public bool isthisMainFlag;
    public BoxCollider bc1;
    public BoxCollider bc2;
    public BoxCollider bc3;

    [Header("SaberSS or HeadWF Flag")]
    public bool isThisTail = false;
    public bool isThisHead = false;
    public bool isThisSingle = false;
    public bool isThisDouble = false;
    public bool isThisPhosStick_Philic = false;
    public bool isThisPhosStick_Phos = false;
    private bool firstGrab = false;
    private bool cantDetach = false;

    [Header("Effects")]
    public GameObject[] attachPosLight = new GameObject[4];
    public GameObject attachEffect; // 장착할 때의 이펙트    

    [Header("Desc Panel")]
    private bool descFlag = false;
    public GameObject[] descPanels;
    public GameObject[] toolTipPanels;

    [Header("ToolTip")]
    public Tooltip tooltip;

    void Start()
    {
        tutoMgr = GameObject.FindGameObjectWithTag("GameController").GetComponent<TutorialManager_CM>();

        grabbable = GetComponent<BNG.Grabbable>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = true;

        objRespawnPoint2 = tutoMgr.spawnPos_Single;
        objSpawnRotate = Quaternion.identity;

        if (isThisTail == true) objRespawnPoint1 = tutoMgr.spawnPos_Tail;
        else if (isThisHead == true) objRespawnPoint1 = tutoMgr.spawnPos_Head;
        else if (isThisSingle == true) objRespawnPoint1 = tutoMgr.spawnPos_Single;
        else if (isThisDouble == true) objRespawnPoint1 = tutoMgr.spawnPos_Double;
        else if (isThisPhosStick_Philic) objRespawnPoint1 = tutoMgr.spawnPos_Tail;
        else if (isThisPhosStick_Phos)
        {
            objRespawnPoint1 = tutoMgr.spawnPos_Head;
            objSpawnRotate = Quaternion.Euler(180f, 180f, 0);
        }

        if (isthisMainFlag == true)
        {
            bc1.enabled = false;
            bc2.enabled = false;
            bc3.enabled = false;
        }
        TurnEffect(false);
    }


    private void Update()
    {
        if (grabbable.BeingHeld == true)
        {
            rb.isKinematic = false;

            if (isThisTail == true && firstGrab == false)
            {
                tutoMgr.TooltipOver(0);
                firstGrab = true;
            }
            else if (isThisHead == true && firstGrab == false)
            {
                tutoMgr.TooltipOver(1);
                firstGrab = true;
            }
            else if (isThisDouble == true && firstGrab == false)
            {
                tooltip.TooltipOff();
                firstGrab = true;
            }
            else if (isThisPhosStick_Philic == true && firstGrab == false)
            {
                tutoMgr.tooltips[0].TooltipTextChange("반대쪽 손도 집어보자!");
                tutoMgr.tooltips[0].IncreasingTooltipGap(.03f);
                firstGrab = true;
            }
            else if (isThisPhosStick_Phos == true && firstGrab == false)
            {
                tutoMgr.tooltips[1].TooltipTextChange("반대쪽 손도 집어보자!");
                tutoMgr.tooltips[1].IncreasingTooltipGap(-.03f);
                firstGrab = true;
            }


            //DescPanelONOff(false);
            TurnEffect(0, true);
        }
        else
        {
            rb.isKinematic = true;
        }

        
        /*
        if (transform.position.y < revortPos)
        {
            TurnEffect(false);

            if (transform.childCount > 0)
            {
                if (statusFlag == 1) // single
                {
                    objRespawnPoint1.position = tutoMgr.spawnPos_Tail.position;
                    objSpawnRotate = Quaternion.identity;
                }
                else if (statusFlag == 2) // double
                {
                    objRespawnPoint1.position = tutoMgr.spawnPos_Head.position;
                    objSpawnRotate = Quaternion.identity;
                }
                else if (statusFlag == 3) // double x2
                {
                    objRespawnPoint1.position = tutoMgr.spawnPos_Tail.position;
                    objSpawnRotate = Quaternion.identity;
                }
            }


            rb.isKinematic = true;
            transform.position = objRespawnPoint1.position;
            transform.rotation = objSpawnRotate;
            //isHeld = false;

            DescPanelONOff(true);

            //if (statusFlag == 1) statusFlag = 0;
        }
        */
    }

    public void TurnEffect(int i, bool onoff)
    {
        if (isthisMainFlag == true) attachPosLight[i].SetActive(onoff);
        if (i == 1)
        {
            MadeToolTip(0);            
        }
    }

    public void TurnEffect(bool onoff)
    {
        if (isthisMainFlag == true)
        {
            for (int i = 0; i < attachPosLight.Length; i++)
            {
                attachPosLight[i].SetActive(onoff);
            }
        }
    }

    public void TurnAttachEffect(int i)
    {
        AudioMgr_CM.Instance.PlaySFXByInt(4);
        Instantiate(attachEffect, attachPosLight[i].transform.position, attachPosLight[i].transform.rotation);
    }

    public void TurnLayerToDefault()
    {
        transform.GetComponent<DescObjID_CM>().enabled = false;
    }

    public void DescPanelONOff(bool onOrOff)
    {
        if (descFlag == false)
        {
            for (int i = 0; i < descPanels.Length; i++)
            {
                descPanels[i].SetActive(onOrOff);
            }
        }
    }

    public void DestroyDescPanel()
    {
        for (int i = 0; i < descPanels.Length; i++)
        {
            Destroy(descPanels[i]);
        }

        descFlag = true;
    }

    public void MadeToolTip(int i)
    {
        toolTipPanels[i].SetActive(true);
        toolTipPanels[i].GetComponent<Tooltip>().TooltipOn("이 부분을 서로 연결시켜봐요!");
    }
}