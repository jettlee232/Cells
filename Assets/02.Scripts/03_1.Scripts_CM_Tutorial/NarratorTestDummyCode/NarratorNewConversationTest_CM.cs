using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class NarratorNewConversationTest_CM : MonoBehaviour
{
    public DialogueSystemTrigger dialogueSystemTrigger1;
    public DialogueSystemTrigger dialogueSystemTrigger2;
    public void ActivateDST1() // 1번째 트리거 작동 함수
    {
        dialogueSystemTrigger1.startConversationEntryID = 0; // 1번째 트리거의 컨버제이션 진입 번호를 0번으로 변경 (이거 안해도 되기는 한데, 안하면 나중에 컨버제이션 재활용이 불가)
        dialogueSystemTrigger1.OnUse(); // On Use로 컨버제이션 작동           
    }

    public void ActivateDST2() // 2번째 트리거 작동 함수
    {
        dialogueSystemTrigger2.startConversationEntryID = 0; // 1번째 트리거의 컨버제이션 진입 번호를 0번으로 변경 (이거 안해도 되기는 한데, 안하면 나중에 컨버제이션 재활용이 불가)
        dialogueSystemTrigger2.OnUse(); // On Use로 컨버제이션 작동              
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ActivateDST1();
        }        
        else if (Input.GetKeyDown(KeyCode.W))
        {
            ActivateDST2();
        }
    }
}
