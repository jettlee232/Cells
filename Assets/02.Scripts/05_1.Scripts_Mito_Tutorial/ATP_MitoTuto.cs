using BNG;
using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATP_MitoTuto : MonoBehaviour
{
    public bool dialogueActive = false; // 대화가 활성화되었는지 확인하는 플래그

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

        if (GetComponentInChildren<Grabbable>().SelectedHandPose) // 임시
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

    // 대충 atp 설명 보면 실행
    public void FinishGrabATP()
    {
        // 대충 조건
        DialogueController_MitoTuto.Instance.ActivateDST(10);
    }

    // 대충 아데닌 리보스 인산 다 만져보면 실행
    public void FinishGrabATPComponent()
    {
        DialogueController_MitoTuto.Instance.ActivateDST(11);
    }
}
