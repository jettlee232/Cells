using BNG;
using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATP_MitoTuto : MonoBehaviour
{
    //public bool dialogueActive = false; // ��ȭ�� Ȱ��ȭ�Ǿ����� Ȯ���ϴ� �÷���

    public bool isATPGrabbed = false; // ATP�� �ѹ��̶� �������� üũ
    public bool isATPComponentGrabbed = false; // �Ƶ��� ������ �λ꿰�� �ѹ��̶� �������� üũ
    //public bool isComponentGrabbed = false;

    public Tooltip_Mito[] tooltips;

    void Update()
    {
        if (GetComponent<Grabbable>().SelectedHandPose) // ���� ���� ���� ������
        {
            if (!isATPGrabbed)
            {
                isATPGrabbed = true;
                PlayerTooltipOff();
                // ���⿡ ����ó�� ���̾�α� �ѱ�� �Ƹ��ٿ��?
            }
            //GetComponent<HighlightEffect>().highlighted = true;

        }

        if (CheckChildIsGrabbed())
        {
            if (!isATPComponentGrabbed)
            {
                isATPComponentGrabbed = true;
                PlayerTooltipOff();
                // ���⿡ ����ó�� ���̾�α� �ѱ�� �Ƹ��ٿ��?
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
        // �ڽ� ������Ʈ�鿡�� ItemExplain_MitoTuto ��ũ��Ʈ�� ã��
        ItemExplain_MitoTuto[] childItems = GetComponentsInChildren<ItemExplain_MitoTuto>();
        foreach (ItemExplain_MitoTuto item in childItems)
        {
            if (item != this.GetComponent<ItemExplain_MitoTuto>() && item.isGrab)
            {
                return true; // �ϳ��� isGrab�� true�̸� true ��ȯ
            }
        }
        return false; // ��� �ڽ� ������Ʈ�� isGrab�� false�̸� false ��ȯ
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
