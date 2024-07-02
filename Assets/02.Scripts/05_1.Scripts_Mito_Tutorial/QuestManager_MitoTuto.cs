using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class QuestManager_MitoTuto : MonoBehaviour
{
    public static QuestManager_MitoTuto Instance { get; private set; }

    public QuestPanel_Mito questPanelMito;
    public LocationPanel_CM locationPanelMito;

    UnityEngine.XR.InputDevice right;
    PlayerMoving_Mito playerMoving_Mito;

    public TextMeshProUGUI questText;

    public bool playerInRange = false; // 플레이어가 범위 내에 있는지 확인하는 플래그
    public bool dialogueActive = false; // 대화가 활성화되었는지 확인하는 플래그
    public bool isDesc = false;
    public bool isDesc2 = false;

    public bool isABtnPressed = false;
    public bool wasABtnPressed = false;

    public bool isATP = false;
    public bool isAdenine = false;
    public bool isRibose = false;
    public bool isPhosphate = false;

    public GameObject npcToolTip;

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

    void Start()
    {
        playerMoving_Mito = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoving_Mito>();

        StartCoroutine(StartQuest(4.0f));
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
                {
                    npcToolTip.SetActive(false);
                    StartCoroutine(PlayDialogueAfterDelay(3));
                }
                else
                {
                    npcToolTip.SetActive(false);
                    StartCoroutine(PlayDialogueAfterDelay(7));
                }
                dialogueActive = true;
            }
        }

        CheckGrabATP();
        CheckInteractionATPComponent();

        wasABtnPressed = isABtnPressed;
    }

    IEnumerator StartQuest(float delay)
    {
        locationPanelMito.PanelOpen("미토콘드리아");
        questPanelMito.PanelClose();
        //questPanelMito.ChangeText("");

        yield return new WaitForSeconds(delay);
        questPanelMito.PanelOpen("NPC에게 말을 걸어보자!");
    }

    public void ResetQuestText()
    {
        questText.text = string.Empty;
    }

    public void ChangeQuestText(string text)
    {
        questText.text = text;
    }

    public void CheckGrabATP()
    {
        if (isATP && !isDesc)
        {
            StartCoroutine(PlayDialogueAfterDelay(10));

            isDesc = true;
        }
    }

    public void CheckInteractionATPComponent()
    {
        if (isAdenine && isRibose && isPhosphate && !isDesc2)
        {
            StartCoroutine(PlayDialogueAfterDelay(11));

            isDesc2 = true;
        }
    }

    IEnumerator PlayDialogueAfterDelay(int index)
    {
        yield return new WaitForSeconds(0.5f);
        DialogueController_MitoTuto.Instance.ActivateDST(index);
    }

    public void CheckMyATP()
    {
        Debug.Log("MyATP 완성");
        DialogueController_MitoTuto.Instance.ActivateDST(12);
    }
}
