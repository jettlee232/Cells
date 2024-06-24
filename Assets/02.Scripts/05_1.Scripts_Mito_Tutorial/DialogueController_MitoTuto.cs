using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController_MitoTuto : MonoBehaviour
{
    public DialogueSystemTrigger[] dialogueSystemTriggers; // 트리거 목록

    public void ActivateDST3() // 3번째 트리거 작동 함수
    {
        dialogueSystemTriggers[2].startConversationEntryID = 0; // 1번째 트리거의 컨버제이션 진입 번호를 0번으로 변경 (이거 안해도 되기는 한데, 안하면 나중에 컨버제이션 재활용이 불가)
        dialogueSystemTriggers[2].OnUse(); // On Use로 컨버제이션 작동           
    }

    public void ActivateDST4()
    {
        dialogueSystemTriggers[3].startConversationEntryID = 0; // 1번째 트리거의 컨버제이션 진입 번호를 0번으로 변경 (이거 안해도 되기는 한데, 안하면 나중에 컨버제이션 재활용이 불가)
        dialogueSystemTriggers[3].OnUse(); // On Use로 컨버제이션 작동              
    }

    public void ActivateDST(int n)
    {
        dialogueSystemTriggers[n - 1].startConversationEntryID = 0; // n번째 트리거의 컨버제이션 진입 번호를 0번으로 변경 (이거 안해도 되기는 한데, 안하면 나중에 컨버제이션 재활용이 불가)
        dialogueSystemTriggers[n - 1].OnUse(); // On Use로 컨버제이션 작동            
    }

    // 이건 함수가 작동하는지 대충 테스트 가능하게 넣은 업데이트문, 나중에 삭제하면 됨
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ActivateDST(3);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            ActivateDST(4);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            ActivateDST(5);
        }
    }
}
