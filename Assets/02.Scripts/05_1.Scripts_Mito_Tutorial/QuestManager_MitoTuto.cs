using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class QuestManager_MitoTuto : MonoBehaviour
{
    public static QuestManager_MitoTuto Instance { get; private set; }

    UnityEngine.XR.InputDevice right;
    PlayerMoving_Mito playerMoving_Mito;

    public TextMeshProUGUI questText;

    public bool playerInRange = false; // �÷��̾ ���� ���� �ִ��� Ȯ���ϴ� �÷���
    public bool dialogueActive = false; // ��ȭ�� Ȱ��ȭ�Ǿ����� Ȯ���ϴ� �÷���

    public bool isABtnPressed = false;
    public bool wasABtnPressed = false;

    public GameObject npcToolTip;

    private void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // �ν��Ͻ��� �ߺ� �������� �ʵ��� ���� �ν��Ͻ��� �ı�
        }
    }

    void Start()
    {
        playerMoving_Mito = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoving_Mito>();

        StartCoroutine(StartDelayQuestTextReset(5.0f));
    }

    void Update()
    {
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        right.TryGetFeatureValue(CommonUsages.primaryButton, out isABtnPressed);

        if (npcToolTip.activeSelf)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }

        if (playerInRange)
        {
            if (isABtnPressed && !wasABtnPressed && !dialogueActive)
            {
                if (!playerMoving_Mito.flyable)
                    DialogueController_MitoTuto.Instance.ActivateDST(3);
                else
                    DialogueController_MitoTuto.Instance.ActivateDST(7);
                dialogueActive = true;
                npcToolTip.SetActive(false);
            }
        }

        wasABtnPressed = isABtnPressed;
    }

    IEnumerator StartDelayQuestTextReset(float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeQuestText("NPC���� ���� �ɾ��!");
        yield return new WaitForSeconds(delay / 2.0f);
        ResetQuestText();
    }

    public void ResetQuestText()
    {
        questText.text = string.Empty;
    }

    public void ChangeQuestText(string text)
    {
        questText.text = text;
    }
}
