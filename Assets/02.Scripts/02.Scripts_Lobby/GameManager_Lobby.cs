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
        SceneManager.LoadScene(sceneName);
    }

    public float GetMoveSpeed() { return moveSpeed; }
    public GameObject GetPlayerCam() { return playerCam; }
    public GameObject GetPlayer() { return player; }
    public GameObject GetNPC() { return NPC; }
}
