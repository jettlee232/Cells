using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_Lobby : MonoBehaviour
{
    public static UIManager_Lobby instance;
    public GameObject alert_UI;
    public GameObject Desc_UI;
    private GameObject lastInteract = null;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
    }

    void Start()
    {
        alert_UI.SetActive(false);
        Desc_UI.SetActive(false);
    }

    void Update()
    {
        
    }

    public void OnClickTitle()
    {
        alert_UI.SetActive(false);
        Desc_UI.SetActive(false);
        GameManager_Lobby.instance.MoveScene("01_Home");
    }

    #region 진입 알림
    public void SetAlert(GameObject menu)
    {
        lastInteract = menu;
        alert_UI.transform.GetChild(0).gameObject.GetComponent<Text>().text = lastInteract.GetComponent<SelectMenu_Lobby>().GetName();
        alert_UI.transform.GetChild(1).gameObject.GetComponent<Text>().text = lastInteract.GetComponent<SelectMenu_Lobby>().GetDescription();
        alert_UI.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(() => GameManager_Lobby.instance.MoveScene(menu.GetComponent<SelectMenu_Lobby>().GetSceneName()));

        alert_UI.SetActive(true);
    }

    public void HideAlert()
    {
        alert_UI.SetActive(false);
        lastInteract = null;
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
    #endregion

    #region 설명창
    public GameObject GetDesc() { return Desc_UI; }
    public void OnDesc() { Desc_UI.SetActive(true); }
    public void OffDesc() { Desc_UI.SetActive(false); }
    public bool CheckDesc() { return Desc_UI.activeSelf; }
    #endregion
}
