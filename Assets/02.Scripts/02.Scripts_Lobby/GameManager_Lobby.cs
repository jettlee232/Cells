using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager_Lobby : MonoBehaviour
{
    public static GameManager_Lobby instance;
    public GameObject player;
    public GameObject playerCam;
    public float moveSpeed = 3f;
    private GameObject lastInteract = null;

    void Awake()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else if (instance != this) { Destroy(gameObject); }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetLastInteract(GameObject obj) { lastInteract = obj; }
    public void RemoveLastInteract() { lastInteract = null; }

    public void MoveScene(string sceneName)
    {
        RemoveLastInteract();
        SceneManager.LoadScene(sceneName);
    }
    //public void SetPlayerAway()
    //{
    //    lastInteract.GetComponent<SelectMenu_Lobby>().GetAway(player);
    //    RemoveLastInteract();
    //}

    public float GetMoveSpeed() { return moveSpeed; }
    public GameObject GetPlayerCam() { return playerCam; }
}
