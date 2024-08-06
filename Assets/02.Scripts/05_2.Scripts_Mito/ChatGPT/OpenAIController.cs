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

    public GameObject mainImage;

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
            new ChatMessage(ChatMessageRole.System, "너는 생물에 대해 설명하는 시스템이야. 초등학생이 이해할 수 있도록 친근한 말투로 설명해주고 가능하면 비유를 사용해서 200자 이내로 쉽게 설명해, 만약에 생물과 관련된 질문이 아니라면 답변을 거부해")
        };

        inputField.text = "";
        string startString = "";
        textField.text = startString;
        Debug.Log(startString);
    }
    private async void GetResponse()
    {
        if (inputField.text.Length < 1)
        {
            return;
        }

        okBtn.enabled = false;

        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = ChatMessageRole.User;
        userMessage.TextContent = inputField.text;
        if (userMessage.TextContent.Length > 100)
        {
            userMessage.TextContent = userMessage.TextContent.Substring(0, 100);
        }
        Debug.Log(string.Format("{0} : {1}", userMessage.Role, userMessage.TextContent));

        messages.Add(userMessage);

        mainImage.SetActive(false);

        textField.text = string.Format("플레이어: {0}", userMessage.TextContent);

        inputField.text = "";

        var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.GPT4o_Mini,
            Temperature = 0.1,
            MaxTokens = 200,
            Messages = messages
        });

        ChatMessage responseMessage = new ChatMessage();
        responseMessage.Role = chatResult.Choices[0].Message.Role;
        responseMessage.TextContent = chatResult.Choices[0].Message.TextContent;
        Debug.Log(string.Format("{0}: {1}", responseMessage.rawRole, responseMessage.TextContent));

        messages.Add(responseMessage);

        textField.text = string.Format("플레이어: {0}\n\nAI 도우미:\n{1}", userMessage.TextContent, responseMessage.TextContent);

        okBtn.enabled = true;
    }
}