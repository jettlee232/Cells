using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public bool secondCon { get; set; }

    public bool firstEnd { get; set; }
    public bool secondEnd { get; set; }

    void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
    }

    void Start()
    {
        firstEnd = false;
        secondCon = false;
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
}
