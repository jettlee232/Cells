using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController_MitoTuto : MonoBehaviour
{
    public static DialogueController_MitoTuto Instance { get; private set; }

    public DialogueSystemTrigger[] dialogueSystemTriggers; // 트리거 목록

    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 인스턴스가 중복 생성되지 않도록 기존 인스턴스를 파괴
        }
    }

    public void ActivateDST(int n)
    {
        dialogueSystemTriggers[n - 1].startConversationEntryID = 0; // n번째 트리거의 컨버제이션 진입 번호를 0번으로 변경 (이거 안해도 되기는 한데, 안하면 나중에 컨버제이션 재활용이 불가)
        dialogueSystemTriggers[n - 1].OnUse(); // On Use로 컨버제이션 작동            
    }

}
