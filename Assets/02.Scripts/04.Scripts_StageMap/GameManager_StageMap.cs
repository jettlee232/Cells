using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class GameManager_StageMap : MonoBehaviour
{
    public static GameManager_StageMap instance;
    UnityEngine.XR.InputDevice right;
    UnityEngine.XR.InputDevice left;

    public GameObject player;
    public GameObject playerCam;
    public GameObject uiPointer;
    public GameObject NPC;
    public GameObject TutorialManager;

    public bool firstEnd = false;
    public bool secondEnd = false;
    public bool secondCon = false;
    public bool movable = true;

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
    public void ClearTutorial() { UIManager_StageMap.instance.SetUpsideSubtitle("Talk to NPC!!"); secondCon = true; }
    public bool GetSecondCon() { return secondCon; }
    public bool GetMovable() { return movable; }
    public void EnableMove() { movable = true; player.GetComponent<PlayerMoving_StageMap>().EnableFly(); }
    public void DisableMove() { movable = false; player.GetComponent<PlayerMoving_StageMap>().DisableFly(); }
    public void RemoveSelect() { uiPointer.GetComponent<LaserPointer_StageMap>().DestroyDescription(); }
    #endregion

    public GameObject GetTutorialManager() { return TutorialManager; }

    public void MoveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
