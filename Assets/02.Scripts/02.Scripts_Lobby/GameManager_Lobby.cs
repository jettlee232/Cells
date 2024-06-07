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
    public float moveSpeed = 3f;
    private GameObject lastInteract = null;

    //public bool firstNow { get; set; }
    //public bool secondNow { get; set; }
    //public bool thirdNow { get; set; }

    public bool secondCon { get; set; }

    public bool firstEnd { get; set; }
    public bool secondEnd { get; set; }
    //public bool secondEnd { get; set; }
    //public bool thirdEnd { get; set; }

    void Awake()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else if (instance != this) { Destroy(gameObject); }
    }

    void Start()
    {
        //firstNow = false; secondNow = false; thirdNow = false;
        firstEnd = false;
        secondCon = false;
        //firstEnd = false; secondEnd = false; thirdEnd = false;
    }

    public void SetLastInteract(GameObject obj) { lastInteract = obj; }
    public void RemoveLastInteract() { lastInteract = null; }

    public void MoveScene(string sceneName)
    {
        RemoveLastInteract();
        SceneManager.LoadScene(sceneName);
    }

    public float GetMoveSpeed() { return moveSpeed; }
    public GameObject GetPlayerCam() { return playerCam; }
    public GameObject GetPlayer() { return player; }
    public GameObject GetNPC() { return NPC; }
}
