using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.SceneManagement;
using BNG;

public class TutorialManager_MitoTuto : MonoBehaviour
{
    PlayerMoving_Mito playerMoving_Mito;
    public RayDescription_MitoTuto rayDescription_MitoTuto;

    // Global UI
    public QuestPanel_Mito questPanelMito;
    public LocationPanel_CM locationPanelMito;

    public GameObject laserExplainPanel; // 레이저로 설명창 띄우는법 패널
    public GameObject mixExplainPanel; // 조합법 패널
    public Transform ExplainPanelPos; // 설명패널 위치

    public Transform playerDialoguePos; // NPC와 대화할때 플레이어 위치

    public GameObject playerWall; // 플레이어 근처 투명벽
    public GameObject mapWall; // 전체맵 투명벽
    public GameObject miniHalfMito; // 작은 미토콘드리아
    public GameObject atp; // ATP 모형

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

        playerMoving_Mito.StartPlayer(5.0f);
    }

    public void SetPlayerPosition()
    {
        playerMoving_Mito.transform.position = playerDialoguePos.position;
        playerMoving_Mito.transform.eulerAngles = playerDialoguePos.eulerAngles;
    }

    public void ToggleNpcTooltip()
    {
        GameObject npcToolTip = QuestManager_MitoTuto.Instance.npcToolTip.transform.parent.gameObject;

        npcToolTip.SetActive(!npcToolTip.activeSelf);
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

    public void EnableGrabMiniHalfMito()
    {
        miniHalfMito.GetComponent<CapsuleCollider>().enabled = true;
    }

    public void SliceMito()
    {
        mitoMap1.SetActive(!mitoMap1.activeSelf);
        mitoMap2.SetActive(!mitoMap2.activeSelf);
        mitoMap3.SetActive(!mitoMap3.activeSelf);
    }

    public void LookAtMito()
    {
        playerMoving_Mito.transform.localPosition = Vector3.zero;
        playerMoving_Mito.transform.localEulerAngles = new Vector3(0, 270.0f, 0);
    }

    public void ToggleATP()
    {
        atp.transform.position = new Vector3(68.25f, 76.75f, 5.0f);
        atp.transform.eulerAngles = new Vector3(0, 330.0f, 0);
        atp.SetActive(!atp.activeSelf);
    }

    public void ToggleGrabATP()
    {
        SphereCollider sphereCollider = atp.GetComponent<SphereCollider>();
        if (sphereCollider != null)
        {
            sphereCollider.enabled = !sphereCollider.enabled;
        }
        atp.transform.GetChild(0).gameObject.SetActive(!atp.transform.GetChild(0).gameObject.activeSelf);
    }

    public void EnableGrabATPComponent()
    {
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
        adeinine.SetActive(true);
        ribose.SetActive(true);
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

    public void EndTutorial()
    {
        Scene scene = SceneManager.GetActiveScene();

        int curScene = scene.buildIndex;
        int nextScene = curScene + 1;

        StartCoroutine(LoadNextScene(nextScene));
    }

    IEnumerator LoadNextScene(int index)
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(index);
    }

    #region Register with Lua
    private void OnEnable()
    {
        Lua.RegisterFunction("SetPlayerPosition", this, SymbolExtensions.GetMethodInfo(() => SetPlayerPosition()));
        Lua.RegisterFunction("ToggleNpcTooltip", this, SymbolExtensions.GetMethodInfo(() => ToggleNpcTooltip()));
        Lua.RegisterFunction("ToggleRayDescription", this, SymbolExtensions.GetMethodInfo(() => ToggleRayDescription()));
        Lua.RegisterFunction("ReadyDialogue", this, SymbolExtensions.GetMethodInfo(() => ReadyDialogue()));
        Lua.RegisterFunction("ToggleIsMoving", this, SymbolExtensions.GetMethodInfo(() => ToggleIsMoving()));
        Lua.RegisterFunction("SetPlayerSpeed", this, SymbolExtensions.GetMethodInfo(() => SetPlayerSpeed((float)0, (float)0, (float)0)));
        Lua.RegisterFunction("TogglePlayerWall", this, SymbolExtensions.GetMethodInfo(() => TogglePlayerWall()));
        Lua.RegisterFunction("ToggleMapWall", this, SymbolExtensions.GetMethodInfo(() => ToggleMapWall()));
        Lua.RegisterFunction("ToggleFlyable", this, SymbolExtensions.GetMethodInfo(() => ToggleFlyable()));
        Lua.RegisterFunction("ShowLaserPanel", this, SymbolExtensions.GetMethodInfo(() => ShowLaserPanel()));
        Lua.RegisterFunction("ToggleMiniHalfMito", this, SymbolExtensions.GetMethodInfo(() => ToggleMiniHalfMito()));
        Lua.RegisterFunction("EnableGrabMiniHalfMito", this, SymbolExtensions.GetMethodInfo(() => EnableGrabMiniHalfMito()));
        Lua.RegisterFunction("SliceMito", this, SymbolExtensions.GetMethodInfo(() => SliceMito()));
        Lua.RegisterFunction("LookAtMito", this, SymbolExtensions.GetMethodInfo(() => LookAtMito()));
        Lua.RegisterFunction("ToggleATP", this, SymbolExtensions.GetMethodInfo(() => ToggleATP()));
        Lua.RegisterFunction("ToggleGrabATP", this, SymbolExtensions.GetMethodInfo(() => ToggleGrabATP()));
        Lua.RegisterFunction("EnableGrabATPComponent", this, SymbolExtensions.GetMethodInfo(() => EnableGrabATPComponent()));
        Lua.RegisterFunction("SetActiveATPMixComponents", this, SymbolExtensions.GetMethodInfo(() => SetActiveATPMixComponents()));
        Lua.RegisterFunction("ShowATPMixRulePanel", this, SymbolExtensions.GetMethodInfo(() => ShowATPMixRulePanel()));
        Lua.RegisterFunction("CloseQuest", this, SymbolExtensions.GetMethodInfo(() => CloseQuest()));
        Lua.RegisterFunction("ChangeQuestText", this, SymbolExtensions.GetMethodInfo(() => ChangeQuestText(string.Empty)));
        Lua.RegisterFunction("DelayCloseQuestText", this, SymbolExtensions.GetMethodInfo(() => DelayCloseQuestText(string.Empty, (float)0)));
        Lua.RegisterFunction("VibrateHand", this, SymbolExtensions.GetMethodInfo(() => VibrateHand()));
        Lua.RegisterFunction("EndTutorial", this, SymbolExtensions.GetMethodInfo(() => EndTutorial()));
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction("SetPlayerPosition");
        Lua.UnregisterFunction("ToggleNpcTooltip");
        Lua.UnregisterFunction("ToggleRayDescription");
        Lua.UnregisterFunction("ReadyDialogue");
        Lua.UnregisterFunction("ToggleIsMoving");
        Lua.UnregisterFunction("SetPlayerSpeed");
        Lua.UnregisterFunction("TogglePlayerWall");
        Lua.UnregisterFunction("ToggleMapWall");
        Lua.UnregisterFunction("ToggleFlyable");
        Lua.UnregisterFunction("ShowLaserPanel");
        Lua.UnregisterFunction("ToggleMiniHalfMito");
        Lua.UnregisterFunction("EnableGrabMiniHalfMito");
        Lua.UnregisterFunction("SliceMito");
        Lua.UnregisterFunction("LookAtMito");
        Lua.UnregisterFunction("ToggleATP");
        Lua.UnregisterFunction("ToggleGrabATP");
        Lua.UnregisterFunction("EnableGrabATPComponent");
        Lua.UnregisterFunction("SetActiveATPMixComponents");
        Lua.UnregisterFunction("ShowATPMixRulePanel");
        Lua.UnregisterFunction("CloseQuest");
        Lua.UnregisterFunction("ChangeQuestText");
        Lua.UnregisterFunction("DelayCloseQuestText");
        Lua.UnregisterFunction("VibrateHand");
        Lua.UnregisterFunction("EndTutorial");
    }
    #endregion
}
