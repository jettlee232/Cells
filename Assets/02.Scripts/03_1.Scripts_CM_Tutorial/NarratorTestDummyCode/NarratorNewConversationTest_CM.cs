using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class NarratorNewConversationTest_CM : MonoBehaviour
{
    public DialogueSystemTrigger dialogueSystemTrigger;
    public List<Conversation> conv;


    private void Start()
    {
        Debug.Log("Current DB : " + dialogueSystemTrigger.selectedDatabase);
        Debug.Log("Current Conv : " + dialogueSystemTrigger.selectedDatabase.);
        dialogueSystemTrigger.selectedDatabase.conversations.Clear();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {           
            ChangeCurrentConversation();
        }        
    }

    public void ChangeCurrentConversation()
    {
        //dialogueSystemTrigger.selectedDatabase.conversations = dialogueSystemTrigger.selectedDatabase.GetConversation("Text Convers 2");

        dialogueSystemTrigger.conversation = "Text Convers 2";

        /*
        if (dialogueSystemTrigger != null)
        {
            dialogueSystemTrigger.conversation = "Text Convers 2";
        }
        else
        {
            Debug.LogError("DialogueSystemTrigger 참조가 설정되지 않았습니다.");
        }
        */
    }

}
