using BNG;
using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATP_MitoTuto : MonoBehaviour
{
    //public bool dialogueActive = false; // ��ȭ�� Ȱ��ȭ�Ǿ����� Ȯ���ϴ� �÷���

    public bool isATPGrabbed = false; // �ѹ��̶� �������� üũ
    //public bool isComponentGrabbed = false;

    void Update()
    {
        if (GetComponent<Grabbable>().SelectedHandPose) // ���� ���� ���� ������
        {
            isATPGrabbed = true;
            GetComponent<HighlightEffect>().highlighted = true;
        }
        //else
        //    GetComponent<HighlightEffect>().highlighted = false;

        //if (isATPGrabbed && QuestManager_MitoTuto.Instance.CheckInteractionATP() && !dialogueActive)
        //{
        //    FinishGrabATP();
        //    dialogueActive = true;
        //}

    }
}
