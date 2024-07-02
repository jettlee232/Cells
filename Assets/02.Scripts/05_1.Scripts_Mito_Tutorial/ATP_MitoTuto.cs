using BNG;
using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATP_MitoTuto : MonoBehaviour
{
    //public bool dialogueActive = false; // 대화가 활성화되었는지 확인하는 플래그

    public bool isATPGrabbed = false;
    public bool isComponentGrabbed = false;

    void Update()
    {
        if (GetComponent<Grabbable>().SelectedHandPose)
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
