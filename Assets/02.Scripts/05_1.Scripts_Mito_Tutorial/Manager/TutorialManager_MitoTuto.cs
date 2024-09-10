using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.SceneManagement;
using BNG;
using HighlightPlus;
using DG.Tweening;

public class TutorialManager_MitoTuto : MonoBehaviour
{
    PlayerMoving_Mito playerMoving_Mito;
    public Transform trackingSpace;
    public RayDescription_MitoTuto rayDescription_MitoTuto;

    // Global UI
    public QuestPanel_Mito questPanelMito;
    public LocationPanel_CM locationPanelMito;

    public GameObject laserExplainPanel; // 레이저로 설명창 띄우는법 패널
    public GameObject mixExplainPanel; // 조합법 패널
    public Transform ExplainPanelPos; // 설명패널 위치

    public Transform playerDialoguePos; // NPC와 대화할때 플레이어 위치

    public Tooltip_Mito playerLeftHandToolTip; // 플레이어 왼손 툴팁
    public Tooltip_Mito playerRightHandToolTip; // 플레이어 오른손 툴팁

    public Tooltip_Mito[] mitoToolTips; // 미토콘드리아 툴팁 배열, 외막 - 내막 - 주름 - 기질 - 막사이공간

    public GameObject playerWall; // 플레이어 근처 투명벽
    public GameObject mapWall; // 전체맵 투명벽
    public GameObject miniHalfMito; // 작은 미토콘드리아
    public GameObject mitoText; // 미토콘드리아 3D 텍스트
    public GameObject explainMiniHalfMito; // 설명용 작은 미토콘드리아
    public Transform mitoPos; // 미토콘드리아 초기 위치
    public GameObject atp; // ATP 모형
    public GameObject atpText; // 아데노신 삼인산 3D 텍스트
    public Transform atpPos; // ATP 초기 위치

    // 반으로 잘라봤어 다이얼로그용 모델링
    public GameObject mitoMap1;
    public GameObject mitoMap2;
    public GameObject mitoMap3;

    // MyATP 조합용 아이템
    public GameObject adeinine;
    public GameObject ribose;
    public GameObject phosphate;

    void Start()
    {
        playerMoving_Mito = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoving_Mito>();

        if (playerMoving_Mito != null)
        {
            SetPlayerSpeed(3.0f, 15.0f, 15.0f);
        }

        playerMoving_Mito.StartPlayer(1.0f);

        DOTween.Init();
    }

    public void SetPlayerPosition()
    {
        //playerMoving_Mito.transform.position = playerDialoguePos.position;
        //playerMoving_Mito.transform.eulerAngles = playerDialoguePos.eulerAngles;

        Sequence sequence = DOTween.Sequence();

        // 위치를 자연스럽게 이동
        sequence.Append(playerMoving_Mito.transform.DOMove(playerDialoguePos.position, 1.5f).SetEase(Ease.InOutQuad));

        // 회전을 자연스럽게 변경
        sequence.Join(playerMoving_Mito.transform.DORotate(playerDialoguePos.eulerAngles, 1.5f).SetEase(Ease.InOutQuad));
    }

    public void ToggleNpcTooltip()
    {
        GameObject npcToolTip = QuestManager_MitoTuto.Instance.npcToolTip.gameObject;

        //npcToolTip.SetActive(!npcToolTip.activeSelf);
        npcToolTip.GetComponent<Tooltip_Mito>().TooltipOn("천천히 구경해봐~!");
    }

    public void ToggleRayDescription()
    {
        rayDescription_MitoTuto.canMakeRayDescription = !rayDescription_MitoTuto.canMakeRayDescription;
    }

    public void ReadyDialogue()
    {
        QuestManager_MitoTuto.Instance.dialogueActive = false;
    }

    public void ToggleIsMoving()
    {
        playerMoving_Mito.isMoving = !playerMoving_Mito.isMoving;
        playerMoving_Mito.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void ToggleIsRotateX()
    {
        playerMoving_Mito.isRotateX = !playerMoving_Mito.isRotateX;
    }

    public void ResetPlayerRotate()
    {
        trackingSpace.DORotate(playerDialoguePos.eulerAngles, 1.5f, RotateMode.Fast);
    }

    public void SetPlayerSpeed(float move, float up, float down)
    {
        playerMoving_Mito.SetPlayerSpeed(move, up, down);
    }

    public void TogglePlayerWall()
    {
        playerWall.SetActive(!playerWall.activeSelf);
    }

    public void ToggleMapWall()
    {
        mapWall.SetActive(!mapWall.activeSelf);
    }

    public void ToggleFlyable()
    {
        playerMoving_Mito.flyable = !playerMoving_Mito.flyable;
    }

    public void ShowLaserPanel()
    {
        GameObject go = Instantiate(laserExplainPanel);
        go.transform.SetParent(ExplainPanelPos);
        go.transform.GetComponent<RectTransform>().localPosition = Vector3.zero;
    }

    public void ToggleMiniHalfMito()
    {
        miniHalfMito.SetActive(!miniHalfMito.activeSelf);
    }

    public void ToggleMitoText()
    {
        mitoText.SetActive(!mitoText.activeSelf);
    }

    public void EnableGrabMiniHalfMito()
    {
        //miniHalfMito.GetComponent<CapsuleCollider>().enabled = true;

        Collider[] colliders = miniHalfMito.GetComponents<BoxCollider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
    }

    public void ToggleExplainMiniHalfMito()
    {
        if (explainMiniHalfMito.transform.parent)
            explainMiniHalfMito.transform.SetParent(null);

        explainMiniHalfMito.SetActive(!explainMiniHalfMito.activeSelf);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(explainMiniHalfMito.transform.DOMove(mitoPos.position, 2.5f).SetEase(Ease.InOutQuad));
        sequence.Join(explainMiniHalfMito.transform.DORotateQuaternion(mitoPos.rotation, 2.5f).SetEase(Ease.InOutQuad));
    }

    public void SliceMito()
    {
        //mitoMap1.SetActive(!mitoMap1.activeSelf);
        //mitoMap2.SetActive(!mitoMap2.activeSelf);

        mitoMap1.transform.DOMove(mitoMap1.transform.position + Vector3.up * -250.0f, 3.0f)
                .OnComplete(() => mitoMap1.SetActive(false));
        mitoMap2.transform.DOMove(mitoMap2.transform.position + Vector3.forward * -260.0f, 3.0f)
                .OnComplete(() => mitoMap2.SetActive(false));

        mitoMap3.SetActive(true);
    }

    public void LookAtMito()
    {
        Vector3 targetPosition = Vector3.zero;
        Vector3 targetRotation = new Vector3(0, 180.0f, 0);

        playerMoving_Mito.transform.DOLocalMove(targetPosition, 1.5f).SetEase(Ease.InOutQuad); // 1초 동안 위치 변경
        playerMoving_Mito.transform.DOLocalRotate(targetRotation, 1.5f).SetEase(Ease.InOutQuad); // 1초 동안 회전 변경

        //playerMoving_Mito.transform.localPosition = Vector3.zero;
        //playerMoving_Mito.transform.localEulerAngles = new Vector3(0, 270.0f, 0);
    }

    public void ToggleATP()
    {
        atp.transform.position = atpPos.position;
        atp.transform.eulerAngles = atpPos.eulerAngles;
        atp.SetActive(!atp.activeSelf);
    }

    public void ToggleATPText()
    {
        atpText.SetActive(!atpText.activeSelf);
    }

    public void ToggleGrabATP()
    {
        SphereCollider sphereCollider = atp.GetComponent<SphereCollider>();
        Grabbable grabbable = atp.GetComponent<Grabbable>();

        if (sphereCollider != null)
        {
            sphereCollider.enabled = !sphereCollider.enabled;
            grabbable.enabled = !grabbable.enabled;
        }

        atp.transform.GetChild(0).gameObject.SetActive(!atp.transform.GetChild(0).gameObject.activeSelf);
    }

    public void EnableGrabATPComponent()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(atp.transform.DOMove(atpPos.position, 2.5f).SetEase(Ease.InOutQuad));
        sequence.Join(atp.transform.DORotateQuaternion(atpPos.rotation, 2.5f).SetEase(Ease.InOutQuad));

        Collider[] childColliders = atp.GetComponentsInChildren<Collider>(true);
        foreach (Collider collider in childColliders)
        {
            collider.enabled = true;
        }

        Grabbable[] grabbableComponents = atp.GetComponentsInChildren<Grabbable>(true);
        foreach (Grabbable grabbable in grabbableComponents)
        {
            grabbable.enabled = true;
        }
    }

    public void SetActiveATPMixComponents()
    {
        StartCoroutine(ATPMixComponents(1.0f));
    }

    IEnumerator ATPMixComponents(float delay)
    {
        adeinine.SetActive(true);
        yield return new WaitForSeconds(delay);
        ribose.SetActive(true);
        yield return new WaitForSeconds(delay);
        phosphate.SetActive(true);
    }

    public void ShowATPMixRulePanel()
    {
        GameObject go = Instantiate(mixExplainPanel);
        go.transform.SetParent(ExplainPanelPos);
        go.transform.GetComponent<RectTransform>().localPosition = Vector3.zero;
    }

    public void CloseQuest()
    {
        questPanelMito.PanelClose();
    }

    public void ChangeQuestText(string text)
    {
        questPanelMito.PanelOpen(text);
    }

    public void DelayCloseQuestText(string text, float delay)
    {
        questPanelMito.PanelOpen(text, delay);
    }

    public void VibrateHand()
    {
        VibrateManager_Mito.Instance.VibrateBothHands();
    }

    public void ShortVibrateHand()
    {
        VibrateManager_Mito.Instance.ShortVibrateBothHands();
    }

    public void PlayerHandToolTipOn(float index, string text)
    {
        switch (index)
        {
            case 0:
                playerLeftHandToolTip.TooltipOn(text);
                break;
            case 1:
                playerRightHandToolTip.TooltipOn(text);
                break;
        }
    }

    public void PlayerHandToolTipTextChange(float index, string text)
    {
        switch (index)
        {
            case 0:
                playerLeftHandToolTip.TooltipTextChange(text);
                break;
            case 1:
                playerRightHandToolTip.TooltipTextChange(text);
                break;
        }
    }

    public void PlayerHandToolTipOffAfterDelay(float index, float delay)
    {
        switch (index)
        {
            case 0:
                StartCoroutine(DelayToolTipOff(playerLeftHandToolTip, delay, false));
                break;
            case 1:
                StartCoroutine(DelayToolTipOff(playerRightHandToolTip, delay, false));
                break;
        }
    }

    public void MitoToolTipOnAfterDelay(float index, string text, float delay)
    {
        switch (index)
        {
            case 0:
                StartCoroutine(DelayToolTipOn(mitoToolTips[0], text, delay, false));
                break;
            case 1:
                StartCoroutine(DelayToolTipOn(mitoToolTips[1], text, delay, false));
                break;
            case 2:
                StartCoroutine(DelayToolTipOn(mitoToolTips[2], text, delay, false));
                break;
            case 3:
                StartCoroutine(DelayToolTipOn(mitoToolTips[3], text, delay, false));
                break;
            case 4:
                StartCoroutine(DelayToolTipOn(mitoToolTips[4], text, delay, false));
                break;
        }
    }

    public void MitoToolTipOff()
    {
        foreach (Tooltip_Mito mitoToolTip in mitoToolTips)
        {
            StartCoroutine(DelayToolTipOff(mitoToolTip, 0, false));
        }

        /*
        switch (index)
        {
            case 0:
                mitoToolTips[0].TooltipOff();
                break;
            case 1:
                mitoToolTips[1].TooltipOff();
                break;
            case 2:
                mitoToolTips[2].TooltipOff();
                break;
            case 3:
                mitoToolTips[3].TooltipOff();
                break;
            case 4:
                mitoToolTips[4].TooltipOff();
                break;
        }
        */
    }

    IEnumerator DelayToolTipOn(Tooltip_Mito tooltip, string text, float delay, bool isHighlight)
    {
        yield return new WaitForSeconds(delay);
        if (isHighlight)
        {
            tooltip.transform.parent.GetComponentInParent<HighlightEffect>().highlighted = true;
            tooltip.transform.parent.GetComponentInParent<HighLightColorchange_MitoTuto>().GlowStart();
        }
        if (tooltip.GetComponentInChildren<BoxCollider>())
            tooltip.GetComponentInChildren<BoxCollider>().enabled = true;
        tooltip.TooltipOn(text);
    }

    IEnumerator DelayToolTipOff(Tooltip_Mito tooltip, float delay, bool isHighlight)
    {
        yield return new WaitForSeconds(delay);
        if (isHighlight)
        {
            tooltip.transform.parent.GetComponentInParent<HighlightEffect>().highlighted = false;
            tooltip.transform.parent.GetComponentInParent<HighLightColorchange_MitoTuto>().GlowEnd();
        }
        if (tooltip.GetComponentInChildren<BoxCollider>())
            tooltip.GetComponentInChildren<BoxCollider>().enabled = false;
        tooltip.TooltipOff();
    }

    public void EndTutorial()
    {
        Scene scene = SceneManager.GetActiveScene();

        int curScene = scene.buildIndex;
        int nextScene = curScene + 1;

        StartCoroutine(LoadIndexScene(nextScene));
    }

    public void BackToTheStageMap()
    {
        StartCoroutine(LoadIndexScene("04_StageMap"));
    }

    IEnumerator LoadIndexScene(int index)
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(index);
    }

    IEnumerator LoadIndexScene(string sceneName)
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(sceneName);
    }

    #region Register with Lua
    private void OnEnable()
    {
        Lua.RegisterFunction("SetPlayerPosition", this, SymbolExtensions.GetMethodInfo(() => SetPlayerPosition()));
        Lua.RegisterFunction("ToggleNpcTooltip", this, SymbolExtensions.GetMethodInfo(() => ToggleNpcTooltip()));
        Lua.RegisterFunction("ToggleRayDescription", this, SymbolExtensions.GetMethodInfo(() => ToggleRayDescription()));
        Lua.RegisterFunction("ReadyDialogue", this, SymbolExtensions.GetMethodInfo(() => ReadyDialogue()));
        Lua.RegisterFunction("ToggleIsMoving", this, SymbolExtensions.GetMethodInfo(() => ToggleIsMoving()));
        Lua.RegisterFunction("ToggleIsRotateX", this, SymbolExtensions.GetMethodInfo(() => ToggleIsRotateX()));
        Lua.RegisterFunction("ResetPlayerRotate", this, SymbolExtensions.GetMethodInfo(() => ResetPlayerRotate()));
        Lua.RegisterFunction("SetPlayerSpeed", this, SymbolExtensions.GetMethodInfo(() => SetPlayerSpeed((float)0, (float)0, (float)0)));
        Lua.RegisterFunction("TogglePlayerWall", this, SymbolExtensions.GetMethodInfo(() => TogglePlayerWall()));
        Lua.RegisterFunction("ToggleMapWall", this, SymbolExtensions.GetMethodInfo(() => ToggleMapWall()));
        Lua.RegisterFunction("ToggleFlyable", this, SymbolExtensions.GetMethodInfo(() => ToggleFlyable()));
        Lua.RegisterFunction("ShowLaserPanel", this, SymbolExtensions.GetMethodInfo(() => ShowLaserPanel()));
        Lua.RegisterFunction("ToggleMiniHalfMito", this, SymbolExtensions.GetMethodInfo(() => ToggleMiniHalfMito()));
        Lua.RegisterFunction("ToggleMitoText", this, SymbolExtensions.GetMethodInfo(() => ToggleMitoText()));
        Lua.RegisterFunction("EnableGrabMiniHalfMito", this, SymbolExtensions.GetMethodInfo(() => EnableGrabMiniHalfMito()));
        Lua.RegisterFunction("ToggleExplainMiniHalfMito", this, SymbolExtensions.GetMethodInfo(() => ToggleExplainMiniHalfMito()));
        Lua.RegisterFunction("SliceMito", this, SymbolExtensions.GetMethodInfo(() => SliceMito()));
        Lua.RegisterFunction("LookAtMito", this, SymbolExtensions.GetMethodInfo(() => LookAtMito()));
        Lua.RegisterFunction("ToggleATP", this, SymbolExtensions.GetMethodInfo(() => ToggleATP()));
        Lua.RegisterFunction("ToggleATPText", this, SymbolExtensions.GetMethodInfo(() => ToggleATPText()));
        Lua.RegisterFunction("ToggleGrabATP", this, SymbolExtensions.GetMethodInfo(() => ToggleGrabATP()));
        Lua.RegisterFunction("EnableGrabATPComponent", this, SymbolExtensions.GetMethodInfo(() => EnableGrabATPComponent()));
        Lua.RegisterFunction("SetActiveATPMixComponents", this, SymbolExtensions.GetMethodInfo(() => SetActiveATPMixComponents()));
        Lua.RegisterFunction("ShowATPMixRulePanel", this, SymbolExtensions.GetMethodInfo(() => ShowATPMixRulePanel()));
        Lua.RegisterFunction("CloseQuest", this, SymbolExtensions.GetMethodInfo(() => CloseQuest()));
        Lua.RegisterFunction("ChangeQuestText", this, SymbolExtensions.GetMethodInfo(() => ChangeQuestText(string.Empty)));
        Lua.RegisterFunction("DelayCloseQuestText", this, SymbolExtensions.GetMethodInfo(() => DelayCloseQuestText(string.Empty, (float)0)));
        Lua.RegisterFunction("VibrateHand", this, SymbolExtensions.GetMethodInfo(() => VibrateHand()));
        Lua.RegisterFunction("PlayerHandToolTipOn", this, SymbolExtensions.GetMethodInfo(() => PlayerHandToolTipOn((float)0, string.Empty)));
        Lua.RegisterFunction("PlayerHandToolTipTextChange", this, SymbolExtensions.GetMethodInfo(() => PlayerHandToolTipTextChange((float)0, string.Empty)));
        Lua.RegisterFunction("PlayerHandToolTipOffAfterDelay", this, SymbolExtensions.GetMethodInfo(() => PlayerHandToolTipOffAfterDelay((float)0, (float)0)));
        Lua.RegisterFunction("MitoToolTipOnAfterDelay", this, SymbolExtensions.GetMethodInfo(() => MitoToolTipOnAfterDelay((float)0, string.Empty, (float)0)));
        Lua.RegisterFunction("MitoToolTipOff", this, SymbolExtensions.GetMethodInfo(() => MitoToolTipOff()));
        Lua.RegisterFunction("EndTutorial", this, SymbolExtensions.GetMethodInfo(() => EndTutorial()));
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction("SetPlayerPosition");
        Lua.UnregisterFunction("ToggleNpcTooltip");
        Lua.UnregisterFunction("ToggleRayDescription");
        Lua.UnregisterFunction("ReadyDialogue");
        Lua.UnregisterFunction("ToggleIsMoving");
        Lua.UnregisterFunction("ToggleIsRotateX");
        Lua.UnregisterFunction("ResetPlayerRotate");
        Lua.UnregisterFunction("SetPlayerSpeed");
        Lua.UnregisterFunction("TogglePlayerWall");
        Lua.UnregisterFunction("ToggleMapWall");
        Lua.UnregisterFunction("ToggleFlyable");
        Lua.UnregisterFunction("ShowLaserPanel");
        Lua.UnregisterFunction("ToggleMiniHalfMito");
        Lua.UnregisterFunction("ToggleMitoText");
        Lua.UnregisterFunction("EnableGrabMiniHalfMito");
        Lua.UnregisterFunction("ToggleExplainMiniHalfMito");
        Lua.UnregisterFunction("SliceMito");
        Lua.UnregisterFunction("LookAtMito");
        Lua.UnregisterFunction("ToggleATP");
        Lua.UnregisterFunction("ToggleATPText");
        Lua.UnregisterFunction("ToggleGrabATP");
        Lua.UnregisterFunction("EnableGrabATPComponent");
        Lua.UnregisterFunction("SetActiveATPMixComponents");
        Lua.UnregisterFunction("ShowATPMixRulePanel");
        Lua.UnregisterFunction("CloseQuest");
        Lua.UnregisterFunction("ChangeQuestText");
        Lua.UnregisterFunction("DelayCloseQuestText");
        Lua.UnregisterFunction("VibrateHand");
        Lua.UnregisterFunction("PlayerHandToolTipOn");
        Lua.UnregisterFunction("PlayerHandToolTipTextChange");
        Lua.UnregisterFunction("PlayerHandToolTipOffAfterDelay");
        Lua.UnregisterFunction("MitoToolTipOnAfterDelay");
        Lua.UnregisterFunction("MitoToolTipOff");
        Lua.UnregisterFunction("EndTutorial");
    }
    #endregion
}
