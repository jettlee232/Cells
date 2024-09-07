using BNG;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class GameManager_Lobby : MonoBehaviour
{
    public static GameManager_Lobby instance;
    public GameObject player;
    public GameObject playerCam;
    public GameObject NPC;
    public GameObject UIPointer;
    public GameObject interactableManager;
    public float moveSpeed = 3f;
    private bool movable = true;
    private bool warpable = false;

    // SYS Code
    public ButtonDoorController_Lobby buttonDoor;

    // SYS Code
    public Tooltip[] toolTips;

    // SYS Code
    public BNG.Lever[] levers;

    public bool secondCon { get; set; }

    public bool firstEnd { get; set; }
    public bool secondEnd { get; set; }

    // SYS Code
    public int tutoStatus = 0;

    // SYS Code    
    [Header("Warp Effects & ETCs")]
    public MyFader_CM scrFader;
    /*
    public Transform playerWarpPos;
    public GameObject warpVFX;
    public Material skyboxMat;
    public GameObject map;
    public GameObject[] disableThings;
    */

    void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
    }

    void Start()
    {
        firstEnd = false;
        secondCon = false;

        // SYS Code
        //warpVFX.SetActive(false);
        if (PlayerPrefs.GetInt("Lobby") == 0)
        {
            NewTooltip(0, "좌측 컨트롤러 조이스틱을 이용하여 움직여보세요!");
            ShowingTooltipAnim(0, 3);            
        }
        else if (PlayerPrefs.GetInt("Lobby") == 1)
        {
            buttonDoor.DoorOpen();
            NewTooltip(1, "A 버튼을 눌러 NPC에게 말을 걸어보세요!");
            ShowingTooltipAnim(1, 0);
            PortalShaderControllerEnable(false);
        }                       
    }

    public void MoveScene(string sceneName)
    {
        UIManager_Lobby.instance.FadeOut();
        StartCoroutine(cMoveScene(sceneName));
    }
    IEnumerator cMoveScene(string sceneName)
    {
        yield return new WaitForSeconds(0.75f);
        SceneManager.LoadScene(sceneName);
    }

    // SYS Code
    public void MoveToCMScnene()
    {
        StartCoroutine(ScreenFadeInAndWarp());
        //for (int i = 0; i < disableThings.Length; i++) { disableThings[i].SetActive(false); }
    }

    // SYS Code    
    IEnumerator ScreenFadeInAndWarp()
    {
        scrFader.ChangeFadeImageColor(Color.white, 6f, 1f);
        scrFader.DoFadeIn();
        //RenderSettings.skybox = skyboxMat;

        yield return new WaitForSeconds(1.5f);

        AudioMgr_CM.Instance.AudioFade();
        MoveScene("03_0_CM_Cutscenes");
        /*
        player.transform.position = playerWarpPos.position;
        warpVFX.SetActive(true);
        map.SetActive(false);

        yield return new WaitForSeconds(0.1f);        

        scrFader.DoFadeOut();

        yield return new WaitForSeconds(5f);

        scrFader.ChangeFadeImageColor(Color.white, 6f, 1f);
        scrFader.DoFadeIn();

        AudioMgr_CM.Instance.AudioFade();

        yield return new WaitForSeconds(2f);

        MoveScene("03_0_CM_Cutscenes");
        */
    }


    public float GetMoveSpeed() { return moveSpeed; }
    public GameObject GetPlayerCam() { return playerCam; }
    public GameObject GetPlayer() { return player; }
    public void StopPlayer() { movable = false; }
    public void EnableMovePlayer() { movable = true; }
    public bool PlayerMove() { return movable; }
    public void SetPlayerPos(Vector3 pos) { player.gameObject.transform.position = pos; }
    public GameObject GetNPC() { return NPC; }
    public GameObject GetUIPointer() { return UIPointer; }
    public void GlowAllStart() { interactableManager.GetComponent<InteractableManager_Lobby>().GlowStart(); }
    public void GlowAllEnd() { interactableManager.GetComponent<InteractableManager_Lobby>().GlowEnd(); }
    public GameObject GetInteractable() { return interactableManager; }

    public bool GetWarpable() { return warpable; }
    public void SetWarpable() { warpable = true; }

    public void SetLobby() { PlayerPrefs.SetInt("Lobby", 1); }
    public int GetLobby() { return PlayerPrefs.GetInt("Lobby"); }

    // SYS Code
    public void GlowStartOnlySelected(double start, double end)
    {
        interactableManager.GetComponent<InteractableManager_Lobby>().GlowStartOnlySelected((int)start, (int)end);
    }

    public void GlowEndOnlySelected(double start, double end)
    {
        interactableManager.GetComponent<InteractableManager_Lobby>().GlowEndOnlySelected((int)start, (int)end);
    }

    // SYS Code 
    public void NewTooltip(int index, string content)
    {
        // Update - 20240903
        if (toolTips[index].gameObject.activeSelf == true)
        {
            toolTips[index].TooltipTextChange(content);
        }
        else
        {
            toolTips[index].gameObject.SetActive(true);
            toolTips[index].TooltipOn(content);
        }        
    }

    public void TooltipOver(int index)
    {
        toolTips[index].TooltipOff();
    }

    // SYS Code
    public void PortalShaderControllerEnable(bool flag)
    {       
        for (int i = 0; i < levers.Length; i++)
        {
            if (flag == true) levers[i].enabled = flag; //lever.onLeverChange.AddListener(psc[i].OnLeverChange);
            else levers[i].enabled = flag; //lever.onLeverChange.RemoveAllListeners();
        }
    }

    // SYS Code
    public void ShowingTooltipAnim(int hand, int anim) { toolTips[hand].ShowingTooltipAnims(anim); }
    public void UnShowingTooltipAnim(int hand) { toolTips[hand].UnShowingTooltipAnims(); } 
}
