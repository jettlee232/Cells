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
        api = new OpenAIAPI(""); // �������
        StartConversation();
        okBtn.onClick.AddListener(() => GetResponse());
    }

    private void StartConversation()
    {
        messages = new List<ChatMessage>
        {
            new ChatMessage(ChatMessageRole.System, "�ʴ� ������ ���� �����ϴ� �ý����̾�. �ʵ��л� ������ � ���̵� ���� ������ �� �ֵ��� ģ���� ������ �����ϰ� �����ϸ� ����ְ� ���� ������ �����, ���࿡ ������ ���õ� ������ �ƴ϶�� ���õ� ������ �ش޶�� �ϸ鼭 �亯�� �ź���, ��� �亯�� 200��ū �̳��� �����ϰ� ��Ȯ�ϰ� ��������")
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
        //��ư Disable
        okBtn.enabled = false;

        //���� �޼����� inputField��
        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = ChatMessageRole.User;
        userMessage.TextContent = inputField.text;
        if (userMessage.TextContent.Length > 100)
        {
            userMessage.TextContent = userMessage.TextContent.Substring(0, 100);
        }
        Debug.Log(string.Format("{0} : {1}", userMessage.Role, userMessage.TextContent));

        //list�� �޼��� �߰�
        messages.Add(userMessage);

        mainImage.SetActive(false);

        //textField�� userMessageǥ�� 
        textField.text = string.Format("���: {0}", userMessage.TextContent);

        //inputField �ʱ�ȭ
        inputField.text = "";

        // ��ü ä���� openAI �����������Ͽ� ���� �޽���(����)�� ����������
        var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.GPT4o_Mini,
            Temperature = 0.1,
            MaxTokens = 200,
            Messages = messages
        });

        //���� ��������
        ChatMessage responseMessage = new ChatMessage();
        responseMessage.Role = chatResult.Choices[0].Message.Role;
        responseMessage.TextContent = chatResult.Choices[0].Message.TextContent;
        Debug.Log(string.Format("{0}: {1}", responseMessage.rawRole, responseMessage.TextContent));

        //������ message����Ʈ�� �߰�
        messages.Add(responseMessage);

        //textField�� ���信 ���� Update
        textField.text = string.Format("���: {0}\n\nAI �����:\n{1}", userMessage.TextContent, responseMessage.TextContent);

        //Okbtn�ٽ� Ȱ��ȭ
        okBtn.enabled = true;
    }
}