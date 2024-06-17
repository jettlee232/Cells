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

    public bool firstEnd = false;
    public bool secondEnd = false;
    public bool secondCon = false;
    public bool movable = true;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
    }

    #region �÷��̾�, NPC
    public GameObject GetPlayer() { return player; }
    public GameObject GetPlayerCam() { return playerCam; }
    public void StopPlayer() { player.GetComponent<PlayerMoving_StageMap>().StopPlayer(); }
    public GameObject GetNPC() { return NPC; }
    public void FirstEnd() { firstEnd = true; }
    public bool GetFirstEnd() { return firstEnd; }
    public bool GetMovable() { return movable; }
    public void EnableMove() { movable = true; }
    public void DisableMove() { movable = false; }
    public void RemoveSelect() { uiPointer.GetComponent<LaserPointer_StageMap>().DestroyDescription(); }
    #endregion

    public void MoveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
