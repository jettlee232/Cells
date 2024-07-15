using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    // SYS Code
    public ButtonDoorController_Lobby buttonDoor;

    // SYS Code
    public Tooltip[] toolTips;

    // SYS Code
    public BNG.Lever[] levers;

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

        // SYS Code
        if (PlayerPrefs.GetInt("Lobby") == 1)
        {
            buttonDoor.DoorOpen();
            NewTooltip(1, "트리거 키를 눌러 NPC에게 말을 걸어보세요!");
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
        toolTips[index].gameObject.SetActive(true);
        toolTips[index].TooltipOn(content);
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
}
