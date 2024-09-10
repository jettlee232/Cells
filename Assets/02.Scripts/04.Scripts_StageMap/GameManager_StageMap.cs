using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.UI;

public class GameManager_StageMap : MonoBehaviour
{
    public static GameManager_StageMap instance;
    UnityEngine.XR.InputDevice right;
    UnityEngine.XR.InputDevice left;

    [Header("Settings")]
    public GameObject player;
    public GameObject playerCam;
    public GameObject uiPointer;
    public GameObject NPC;
    public GameObject TutorialManager;

    [Header("Timers")]
    public float minSelectTime = 0.5f;         // 한번 UI를 끈 후 다시 UI를 키도록 인식할 때까지 지연할 시간 (뭔소린지 해보면 앎)

    private bool firstEnd = false;
    private bool secondEnd = false;
    private bool secondCon = false;
    private bool movable = true;
    private bool selectable = true;

    // SYS Code
    [Header("ETC Variable")]
    public PlayerMoving_StageMap playerMove;    
    public AudioClip alertClip;
    public GameObject speechBubble;
    public SpeechBubblePanel_CM npcSpeechBubble;
    private bool tutoEnd = false;

    // SYS Code
    [Header("Player & NPC")]
    public SelectDialogue_StageMap npcDialogueSystem;
    public Transform[] spawnPos;
    public BNG.MyFader_CM scrFader;
    public LaserPointer_StageMap laserPointer;

    [Header("Buttons")]
    public Button[] interactableBtns;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
    }

    #region 플레이어, NPC
    public GameObject GetPlayer() { return player; }
    public GameObject GetPlayerCam() { return playerCam; }
    public GameObject GetNPC() { return NPC; }
    public void FirstEnd() { firstEnd = true; }
    public bool GetFirstEnd() { return firstEnd; }

    // SYS Code
    public void ClearTutorial() 
    {
        //UIManager_StageMap.instance.SetQuest("NPC에게 돌아가자!");
        secondCon = true;

        // SYS Code
        //AudioMgr_CM.Instance.audioSrc.PlayOneShot(alertClip);
        AudioMgr_CM.Instance.InCreaseSFXandDecreaseBGMfewSec(2f, 0.1f, 3.25f);
        AudioMgr_CM.Instance.PlaySFXByInt(19);
        speechBubble.SetActive(true);
        speechBubble.GetComponent<SpeechBubblePanel_CM>().PanelOpen("여기로 돌아와 봐!");
        tutoEnd = true;
    }

    public void SecondEnd() { secondEnd = true; }
    public bool GetSecondCon() { return secondCon; }
    public bool GetMovable() { return movable; }
    public void EnableMove() { movable = true; player.GetComponent<PlayerMoving_StageMap>().EnableFly(); }
    public void DisableMove() { movable = false; player.GetComponent<PlayerMoving_StageMap>().DisableFly(); }
    public void RemoveSelect() { uiPointer.GetComponent<LaserPointer_StageMap>().DestroyDescription(); }
    public GameObject GetUIPointer() { return uiPointer; }

    // SYS Code
    public void ChangeTutorialStatus(int status) { playerMove.tutorialStatus = status; }
    public void StartAdventure() { npcSpeechBubble.PanelClose(); }
    public bool ReturnTutoEnd() { return tutoEnd; }

    #endregion

    #region UI 끝 -> 새로운 UI 까지

    public void WaitForNewUI() { StartCoroutine(WaitUITimer()); }
    public bool GetSelectable() { return selectable; }
    IEnumerator WaitUITimer()
    {
        selectable = false;
        yield return new WaitForSeconds(minSelectTime);
        selectable = true;
    }
    public void SetSelectable(bool select) { selectable = select; }

    #endregion

    public GameObject GetTutorialManager() { return TutorialManager; }

    public void MoveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // SYS Code
#region LoadMech
    public void LoadFromCM() { npcDialogueSystem.ActivateDST1(); }

    public void LoadFromCM_ForPresentation() 
    {
        npcDialogueSystem.enabled = false;
        laserPointer.enabled = true;
        LoadInteractable();
    }

    public void LoadFromOtherScene(int spawnPosNum)
    {        
        Transform playerPos = player.transform;       
        playerPos.position = spawnPos[spawnPosNum].position;
        playerPos.rotation = spawnPos[spawnPosNum].localRotation;

        LoadFromCM_ForPresentation();
    }
    #endregion

    public void LoadScene(string cutSceneName)
    {
        AudioMgr_CM.Instance.AudioFade();
        StartCoroutine(SceneLoading(cutSceneName));        
    }

    IEnumerator SceneLoading(string cutSceneName)
    {
        scrFader.ChangeFadeImageColor(Color.black, 2f, 1f);
        scrFader.DoFadeIn();

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(cutSceneName);
    }
    public void LoadInteractable() { for (int i = 0; i < interactableBtns.Length; i++) interactableBtns[i].interactable = true; }
}
