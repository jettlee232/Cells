using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenAIController : MonoBehaviour
{
    public TMP_Text textField;
    public TMP_InputField inputField;
    public Button okBtn;

    private OpenAIAPI api;
    private List<ChatMessage> messages;

    void Start()
    {
        api = new OpenAIAPI(""); // 취급주의
        StartConversation();
        okBtn.onClick.AddListener(() => GetResponse());
    }

    private void StartConversation()
    {
        messages = new List<ChatMessage>
        {
            new ChatMessage(ChatMessageRole.System, "너는 미토콘드리아에 대해 설명하는 시스템이야. 초등학생과 중학생이 이해할 수 있도록 친근한 말투로 설명해줘, 만약에 미토콘드리아와 관련된 질문이 아니라면 관련된 질문을 해달라고 하면서 답변을 거부해야해")
        };

        inputField.text = "";
        string startString = "당신의 질문을 기다리고 있습니다!";
        textField.text = startString;
        Debug.Log(startString);
    }
    private async void GetResponse()
    {
        if (inputField.text.Length < 1)
        {
            return;
        }
        //버튼 Disable
        okBtn.enabled = false;

        //유저 메세지에 inputField를
        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = ChatMessageRole.User;
        userMessage.TextContent = inputField.text;
        if (userMessage.TextContent.Length > 100)
        {
            userMessage.TextContent = userMessage.TextContent.Substring(0, 100);
        }
        Debug.Log(string.Format("{0} : {1}", userMessage.Role, userMessage.TextContent));

        //list에 메세지 추가
        messages.Add(userMessage);

        //textField에 userMessage표시 
        textField.text = string.Format("You: {0}", userMessage.TextContent);

        //inputField 초기화
        inputField.text = "";

        // 전체 채팅을 openAI 서버에전송하여 다음 메시지(응답)를 가져오도록
        var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.ChatGPTTurbo,
            Temperature = 0.1,
            MaxTokens = 200,
            Messages = messages
        });

        //응답 가져오기
        ChatMessage responseMessage = new ChatMessage();
        responseMessage.Role = chatResult.Choices[0].Message.Role;
        responseMessage.TextContent = chatResult.Choices[0].Message.TextContent;
        Debug.Log(string.Format("{0}: {1}", responseMessage.rawRole, responseMessage.TextContent));

        //응답을 message리스트에 추가
        messages.Add(responseMessage);

        //textField를 응답에 따라 Update
        textField.text = string.Format("You: {0}\n\nChatGPT:\n{1}", userMessage.TextContent, responseMessage.TextContent);

        //Okbtn다시 활성화
        okBtn.enabled = true;
    }
}