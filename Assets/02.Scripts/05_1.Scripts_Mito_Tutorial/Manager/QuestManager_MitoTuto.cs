using PixelCrushers.Wrappers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class QuestManager_MitoTuto : MonoBehaviour
{
    public static QuestManager_MitoTuto Instance { get; private set; }

    // Global UI
    public QuestPanel_Mito questPanelMito;
    public LocationPanel_CM locationPanelMito;

    UnityEngine.XR.InputDevice right;
    PlayerMoving_Mito playerMoving_Mito;

    //public TextMeshProUGUI questText;

    public bool playerInRange = false; // �÷��̾ ���� ���� �ִ��� Ȯ���ϴ� �÷���
    public bool dialogueActive = false; // ��ȭ�� Ȱ��ȭ�Ǿ����� Ȯ���ϴ� �÷���
    public bool isDesc = false; // �ߺ� ȣ�� ������ �÷���
    public bool isDesc2 = false; // �ߺ� ȣ�� ������ �÷���(�ӽ�)

    // A��ư �ߺ� �Է� ������ �����ε� ���� �ʿ�?
    public bool isABtnPressed = false;
    public bool wasABtnPressed = false;

    public bool isATP = false; // ATP ���� ����Ȯ�� + �׷�Ȯ�� üũ
    public bool isAdenine = false; // �Ƶ��� ����Ȯ�� + �׷�Ȯ�� üũ
    public bool isRibose = false; // ������ ����Ȯ�� + �׷�Ȯ�� üũ
    public bool isPhosphate = false; // �λ꿰 ����Ȯ�� + �׷�Ȯ�� üũ

    public UIButtonKeyTrigger uiButtonKeyTrigger;

    public GameObject mixEffect; // ����ȿ��

    public GameObject npcToolTip; // NPC�� �ִ� ����

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

        StartCoroutine(FindUiButtonKeyTrigger());
        StartCoroutine(StartQuest(4.0f));
    }

    void Update()
    {
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        right.TryGetFeatureValue(CommonUsages.primaryButton, out isABtnPressed);

        // NPC ������ Ȱ��ȭ ���ο� ���� ���� ���
        if (npcToolTip.GetComponent<NPCToolTip_MitoTuto>().checkActive)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }

        if (playerInRange) // �÷��̾ ���� �ȿ� ������
        {
            if (isABtnPressed && !wasABtnPressed && !dialogueActive) // ��ư�� ���� �ɵ�?
            {
                if (!playerMoving_Mito.flyable) // �� ó���� NPC�� ������ ��Ȳ�϶� ȣ��
                {
                    //npcToolTip.SetActive(false);
                    npcToolTip.GetComponent<Tooltip_Mito>().TooltipOff();
                    StartCoroutine(PlayDialogueAfterDelay(3, 1.0f));
                }
                else // �������� �� �����ϰ� ���ƿ����� ȣ��
                {
                    //npcToolTip.SetActive(false);
                    npcToolTip.GetComponent<Tooltip_Mito>().TooltipOff();
                    StartCoroutine(PlayDialogueAfterDelay(7, 1.0f));
                }
                dialogueActive = true; // �ߺ� ȣ�� ����
            }
        }

        CheckGrabATP();
        CheckInteractionATPComponent();

        wasABtnPressed = isABtnPressed;
    }

    IEnumerator StartQuest(float delay)
    {
        //locationPanelMito.PanelOpen("�����ܵ帮��");
        questPanelMito.PanelClose();
        //questPanelMito.ChangeText("");

        yield return new WaitForSeconds(delay);
        VibrateManager_Mito.Instance.VibrateBothHands();
        questPanelMito.PanelOpen("NPC���� ���� �ɾ��!");
    }

    /*
    public void ResetQuestText()
    {
        questText.text = string.Empty;
    }

    public void ChangeQuestText(string text)
    {
        questText.text = text;
    }
    */

    public void CheckGrabATP()
    {
        if (isATP && !isDesc)
        {
            StartCoroutine(PlayDialogueAfterDelay(10, 3.0f));

            isDesc = true;
        }
    }

    public void CheckInteractionATPComponent()
    {
        if (isAdenine && isRibose && isPhosphate && !isDesc2)
        {
            StartCoroutine(PlayDialogueAfterDelay(11, 3.0f));

            isDesc2 = true;
        }
    }

    IEnumerator PlayDialogueAfterDelay(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        DialogueController_MitoTuto.Instance.ActivateDST(index);
    }

    public void CheckMyATP()
    {
        Debug.Log("MyATP �ϼ�");
        DialogueController_MitoTuto.Instance.ActivateDST(12);
    }

    public void enableUBKT()
    {
        uiButtonKeyTrigger.enabled = true;
    }
    public void DisableUBKT()
    {
        uiButtonKeyTrigger.enabled = false;
    }   

    private IEnumerator FindUiButtonKeyTrigger()
    {
        while (uiButtonKeyTrigger == null)
        {
            UIButtonKeyTrigger[] triggers = Resources.FindObjectsOfTypeAll<UIButtonKeyTrigger>();
            if (triggers.Length > 0)
            {
                uiButtonKeyTrigger = triggers[1];
            }

            yield return null;
        }
    }
}
