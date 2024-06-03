using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager_Lobby : MonoBehaviour
{
    public static UIManager_Lobby instance;
    public GameObject alert_UI;
    private GameObject lastInteract = null;

    void Awake()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else if (instance != this) { Destroy(gameObject); }
    }

    void Start()
    {
        alert_UI.SetActive(false);
    }

    void Update()
    {
        
    }

    public void SetAlert(GameObject menu)
    {
        lastInteract = menu;
        alert_UI.transform.GetChild(0).gameObject.GetComponent<Text>().text = lastInteract.GetComponent<SelectMenu_Lobby>().getName();
        alert_UI.transform.GetChild(1).gameObject.GetComponent<Text>().text = lastInteract.GetComponent<SelectMenu_Lobby>().getDescription();
        alert_UI.SetActive(true);
        StartCoroutine(cFollowingUI());
    }
    public void HideAlert()
    {
        alert_UI.SetActive(false);
        lastInteract = null;
    }
    IEnumerator cFollowingUI()
    {
        GameObject playerCam = GameManager_Lobby.instance.GetPlayerCam();
        alert_UI.gameObject.transform.localScale = Vector3.one * 0.0005f;
        while (lastInteract != null)
        {
            if (lastInteract == null) break;
            alert_UI.transform.rotation = playerCam.transform.rotation;
            alert_UI.transform.position = playerCam.transform.position + 0.3f * playerCam.transform.forward;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void OnClickAnimal()
    {
        HideAlert();
        GameManager_Lobby.instance.MoveScene("04_StageMap");
    }
    public void OnClickNo()
    {
        HideAlert();
    }
}
