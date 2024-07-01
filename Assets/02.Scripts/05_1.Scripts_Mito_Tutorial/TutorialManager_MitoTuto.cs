using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.SceneManagement;

public class TutorialManager_MitoTuto : MonoBehaviour
{
    PlayerMoving_Mito playerMoving_Mito;

    public QuestPanel_CM questPanelMito;
    public LocationPanel_CM locationPanelMito;

    public Transform playerDialoguePos;

    public GameObject playerWall;
    public GameObject mapWall;
    public GameObject miniHalfMito;
    public GameObject atp;

    public GameObject mitoMap1;
    public GameObject mitoMap2;
    public GameObject mitoMap3;

    void Start()
    {
        playerMoving_Mito = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoving_Mito>();

        if (playerMoving_Mito != null )
        {
            SetPlayerSpeed(3.0f, 15.0f, 15.0f);
        }
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
        playerMoving_Mito.moveSpeed = move;
        playerMoving_Mito.upSpeed = up;
        playerMoving_Mito.downSpeed = down;
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
        atp.GetComponent<SphereCollider>().enabled = (!atp.GetComponent<SphereCollider>().enabled);
        atp.transform.GetChild(0).gameObject.SetActive(!atp.transform.GetChild(0).gameObject.activeSelf);
    }

    public void EnableGrabATPComponent()
    {

    }

    public void CloseQuest()
    {
        questPanelMito.PanelClose();
    }

    public void ChangeQuestText(string text)
    {
        questPanelMito.PanelOpen(text);
    }

    public void EndTutorial()
    {
        Scene scene = SceneManager.GetActiveScene();

        int curScene = scene.buildIndex;
        int nextScene = curScene + 1;

        SceneManager.LoadScene(nextScene);
    }

    #region Register with Lua
    private void OnEnable()
    {
        Lua.RegisterFunction("SetPlayerPosition", this, SymbolExtensions.GetMethodInfo(() => SetPlayerPosition()));
        Lua.RegisterFunction("ToggleNpcTooltip", this, SymbolExtensions.GetMethodInfo(() => ToggleNpcTooltip()));
        Lua.RegisterFunction("ReadyDialogue", this, SymbolExtensions.GetMethodInfo(() => ReadyDialogue()));
        Lua.RegisterFunction("ToggleIsMoving", this, SymbolExtensions.GetMethodInfo(() => ToggleIsMoving()));
        Lua.RegisterFunction("SetPlayerSpeed", this, SymbolExtensions.GetMethodInfo(() => SetPlayerSpeed((float)0, (float)0, (float)0)));
        Lua.RegisterFunction("TogglePlayerWall", this, SymbolExtensions.GetMethodInfo(() => TogglePlayerWall()));
        Lua.RegisterFunction("ToggleMapWall", this, SymbolExtensions.GetMethodInfo(() => ToggleMapWall()));
        Lua.RegisterFunction("ToggleFlyable", this, SymbolExtensions.GetMethodInfo(() => ToggleFlyable()));
        Lua.RegisterFunction("ToggleMiniHalfMito", this, SymbolExtensions.GetMethodInfo(() => ToggleMiniHalfMito()));
        Lua.RegisterFunction("EnableGrabMiniHalfMito", this, SymbolExtensions.GetMethodInfo(() => EnableGrabMiniHalfMito()));
        Lua.RegisterFunction("SliceMito", this, SymbolExtensions.GetMethodInfo(() => SliceMito()));
        Lua.RegisterFunction("LookAtMito", this, SymbolExtensions.GetMethodInfo(() => LookAtMito()));
        Lua.RegisterFunction("ToggleATP", this, SymbolExtensions.GetMethodInfo(() => ToggleATP()));
        Lua.RegisterFunction("ToggleGrabATP", this, SymbolExtensions.GetMethodInfo(() => ToggleGrabATP()));
        Lua.RegisterFunction("CloseQuest", this, SymbolExtensions.GetMethodInfo(() => CloseQuest()));
        Lua.RegisterFunction("ChangeQuestText", this, SymbolExtensions.GetMethodInfo(() => ChangeQuestText(string.Empty)));
        Lua.RegisterFunction("EndTutorial", this, SymbolExtensions.GetMethodInfo(() => EndTutorial()));
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction("SetPlayerPosition");
        Lua.UnregisterFunction("ToggleNpcTooltip");
        Lua.UnregisterFunction("ReadyDialogue");
        Lua.UnregisterFunction("ToggleIsMoving");
        Lua.UnregisterFunction("SetPlayerSpeed");
        Lua.UnregisterFunction("TogglePlayerWall");
        Lua.UnregisterFunction("ToggleMapWall");
        Lua.UnregisterFunction("ToggleFlyable");
        Lua.UnregisterFunction("ToggleMiniHalfMito");
        Lua.UnregisterFunction("EnableGrabMiniHalfMito");
        Lua.UnregisterFunction("SliceMito");
        Lua.UnregisterFunction("LookAtMito");
        Lua.UnregisterFunction("ToggleATP");
        Lua.UnregisterFunction("ToggleGrabATP");
        Lua.UnregisterFunction("CloseQuest");
        Lua.UnregisterFunction("ChangeQuestText");
        Lua.UnregisterFunction("EndTutorial");
    }
    #endregion
}
