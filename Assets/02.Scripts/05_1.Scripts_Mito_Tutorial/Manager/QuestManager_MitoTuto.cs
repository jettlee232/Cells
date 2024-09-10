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

    public bool playerInRange = false; // 플레이어가 범위 내에 있는지 확인하는 플래그
    public bool dialogueActive = false; // 대화가 활성화되었는지 확인하는 플래그
    public bool isDesc = false; // 중복 호출 방지용 플래그
    public bool isDesc2 = false; // 중복 호출 방지용 플래그(임시)

    // A버튼 중복 입력 방지용 변수인데 굳이 필요?
    public bool isABtnPressed = false;
    public bool wasABtnPressed = false;

    public bool isATP = false; // ATP 모형 설명확인 + 그랩확인 체크
    public bool isAdenine = false; // 아데닌 설명확인 + 그랩확인 체크
    public bool isRibose = false; // 리보스 설명확인 + 그랩확인 체크
    public bool isPhosphate = false; // 인산염 설명확인 + 그랩확인 체크

    public UIButtonKeyTrigger uiButtonKeyTrigger;

    public GameObject mixEffect; // 조합효과

    public GameObject npcToolTip; // NPC에 있는 툴팁

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

        StartCoroutine(FindUiButtonKeyTrigger());
        StartCoroutine(StartQuest(4.0f));
    }

    void Update()
    {
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        right.TryGetFeatureValue(CommonUsages.primaryButton, out isABtnPressed);

        // NPC 툴팁의 활성화 여부에 따라 변수 토글
        if (npcToolTip.GetComponent<NPCToolTip_MitoTuto>().checkActive)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }

        if (playerInRange) // 플레이어가 범위 안에 있으면
        {
            if (isABtnPressed && !wasABtnPressed && !dialogueActive) // 버튼은 빼도 될듯?
            {
                if (!playerMoving_Mito.flyable) // 맨 처음에 NPC와 마주한 상황일때 호출
                {
                    //npcToolTip.SetActive(false);
                    npcToolTip.GetComponent<Tooltip_Mito>().TooltipOff();
                    StartCoroutine(PlayDialogueAfterDelay(3, 1.0f));
                }
                else // 비행으로 맵 구경하고 돌아왔을때 호출
                {
                    //npcToolTip.SetActive(false);
                    npcToolTip.GetComponent<Tooltip_Mito>().TooltipOff();
                    StartCoroutine(PlayDialogueAfterDelay(7, 1.0f));
                }
                dialogueActive = true; // 중복 호출 방지
            }
        }

        CheckGrabATP();
        CheckInteractionATPComponent();

        wasABtnPressed = isABtnPressed;
    }

    IEnumerator StartQuest(float delay)
    {
        //locationPanelMito.PanelOpen("미토콘드리아");
        questPanelMito.PanelClose();
        //questPanelMito.ChangeText("");

        yield return new WaitForSeconds(delay);
        VibrateManager_Mito.Instance.VibrateBothHands();
        questPanelMito.PanelOpen("NPC에게 말을 걸어보자!");
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
        Debug.Log("MyATP 완성");
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
