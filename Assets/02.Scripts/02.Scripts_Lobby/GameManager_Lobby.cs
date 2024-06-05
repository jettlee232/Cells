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

    public bool firstDialogue = false;
    public bool secondDialogue = false;

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

    public float GetMoveSpeed() { return moveSpeed; }
    public GameObject GetPlayerCam() { return playerCam; }
    public GameObject GetPlayer() { return player; }

    public void SetDialogueFirstTrue() { firstDialogue = true; }
    public void SetDailogueSecondTrue() { secondDialogue = true; }
}
