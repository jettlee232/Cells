using BNG;
using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATP_MitoTuto : MonoBehaviour
{
    //public bool dialogueActive = false; // 대화가 활성화되었는지 확인하는 플래그

    public bool isATPGrabbed = false; // ATP가 한번이라도 잡혔는지 체크
    public bool isATPComponentGrabbed = false; // 아데닌 리보스 인산염이 한번이라도 잡혔는지 체크
    //public bool isComponentGrabbed = false;

    public Tooltip_Mito[] tooltips;

    void Update()
    {
        if (GetComponent<Grabbable>().SelectedHandPose) // 나를 잡은 손이 있으면
        {
            if (!isATPGrabbed)
            {
                isATPGrabbed = true;
                PlayerTooltipOff();
                // 여기에 미토처럼 다이얼로그 넘기면 아름다울듯?
            }
            //GetComponent<HighlightEffect>().highlighted = true;

        }

        if (CheckChildIsGrabbed())
        {
            if (!isATPComponentGrabbed)
            {
                isATPComponentGrabbed = true;
                PlayerTooltipOff();
                // 여기에 미토처럼 다이얼로그 넘기면 아름다울듯?
            }
        }

        //if (isATPGrabbed && QuestManager_MitoTuto.Instance.CheckInteractionATP() && !dialogueActive)
        //{
        //    FinishGrabATP();
        //    dialogueActive = true;
        //}
    }

    public bool CheckChildIsGrabbed()
    {
        // 자식 오브젝트들에서 ItemExplain_MitoTuto 스크립트를 찾기
        ItemExplain_MitoTuto[] childItems = GetComponentsInChildren<ItemExplain_MitoTuto>();
        foreach (ItemExplain_MitoTuto item in childItems)
        {
            if (item != this.GetComponent<ItemExplain_MitoTuto>() && item.isGrab)
            {
                return true; // 하나라도 isGrab이 true이면 true 반환
            }
        }
        return false; // 모든 자식 오브젝트의 isGrab이 false이면 false 반환
    }

    public void PlayerTooltipOff()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        tooltips = player.GetComponentsInChildren<Tooltip_Mito>();
        foreach (Tooltip_Mito tooltip in tooltips)
        {
            tooltip.TooltipOff();
        }
    }
}
