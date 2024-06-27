using BNG;
using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATP_MitoTuto : MonoBehaviour
{
    public bool dialogueActive = false; // ��ȭ�� Ȱ��ȭ�Ǿ����� Ȯ���ϴ� �÷���

    public bool isATPGrabbed = false;
    public bool isComponentGrabbed = false;

    void Update()
    {
        if (GetComponent<Grabbable>().SelectedHandPose)
        {
            isATPGrabbed = true;
            GetComponent<HighlightEffect>().highlighted = true;
        }
        else
            GetComponent<HighlightEffect>().highlighted = false;

        if (GetComponentInChildren<Grabbable>().SelectedHandPose) // �ӽ�
        {
            isComponentGrabbed = true;
        }

        if (isATPGrabbed && !dialogueActive)
        {
            FinishGrabATP();
            dialogueActive = true;
        }

        if (isComponentGrabbed && !dialogueActive)
        {
            FinishGrabATPComponent();
            dialogueActive = true;
        }
    }

    // ���� atp ���� ���� ����
    public void FinishGrabATP()
    {
        // ���� ����
        DialogueController_MitoTuto.Instance.ActivateDST(10);
    }

    // ���� �Ƶ��� ������ �λ� �� �������� ����
    public void FinishGrabATPComponent()
    {
        DialogueController_MitoTuto.Instance.ActivateDST(11);
    }
}
