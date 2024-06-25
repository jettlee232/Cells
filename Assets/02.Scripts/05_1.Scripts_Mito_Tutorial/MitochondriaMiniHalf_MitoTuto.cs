using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MitochondriaMiniHalf_MitoTuto : MonoBehaviour
{
    public bool dialogueActive = false; // 대화가 활성화되었는지 확인하는 플래그

    void Update()
    {
        if (GetComponent<Grabbable>().SelectedHandPose && !dialogueActive)
        {
            FinishExplainMito();
            dialogueActive = true;
        }
    }

    // 대충 내막 외막 막사이공간 설명 3개 다 보면 실행
    public void FinishExplainMito()
    {
        // 대충 조건
        DialogueController_MitoTuto.Instance.ActivateDST(5);
    }
}
