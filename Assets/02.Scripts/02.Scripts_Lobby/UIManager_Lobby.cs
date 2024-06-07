using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager_Lobby : MonoBehaviour
{
    public static UIManager_Lobby instance;
    public GameObject alert_UI;
    public GameObject Desc_UI;
    private GameObject lastInteract = null;

    void Awake()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
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

    #region 진입 알림
    public void SetAlert(GameObject menu)
    {
        lastInteract = menu;
        alert_UI.transform.GetChild(0).gameObject.GetComponent<Text>().text = lastInteract.GetComponent<SelectMenu_Lobby>().getName();
        alert_UI.transform.GetChild(1).gameObject.GetComponent<Text>().text = lastInteract.GetComponent<SelectMenu_Lobby>().getDescription();
        alert_UI.SetActive(true);
        //StartCoroutine(cFollowingUI());
    }
    public void HideAlert()
    {
        alert_UI.SetActive(false);
        lastInteract = null;
    }
    //IEnumerator cFollowingUI()
    //{
    //    GameObject playerCam = GameManager_Lobby.instance.GetPlayerCam();
    //    alert_UI.gameObject.transform.localScale = Vector3.one * 0.003f;
    //    while (lastInteract != null)
    //    {
    //        if (lastInteract == null) break;
    //        alert_UI.transform.rotation = playerCam.transform.rotation;
    //        alert_UI.transform.position = playerCam.transform.position + 2f * playerCam.transform.forward;
    //        yield return new WaitForSeconds(0.02f);
    //    }
    //}

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

    #region 자막
    //public void SetSub(GameObject menu)
    //{
    //    lastInteract = menu;
    //    alert_UI.transform.GetChild(0).gameObject.GetComponent<Text>().text = lastInteract.GetComponent<SelectMenu_Lobby>().getName();
    //    alert_UI.transform.GetChild(1).gameObject.GetComponent<Text>().text = lastInteract.GetComponent<SelectMenu_Lobby>().getDescription();
    //    alert_UI.SetActive(true);
    //    StartCoroutine(cFollowingUI());
    //}
    //public void HideSub()
    //{
    //    alert_UI.SetActive(false);
    //    lastInteract = null;
    //}
    //IEnumerator cFollowingSub()
    //{
    //    GameObject playerCam = GameManager_Lobby.instance.GetPlayerCam();
    //    alert_UI.gameObject.transform.localScale = Vector3.one * 0.003f;
    //    while (lastInteract != null)
    //    {
    //        if (lastInteract == null) break;
    //        alert_UI.transform.rotation = playerCam.transform.rotation;
    //        alert_UI.transform.position = playerCam.transform.position + 2f * playerCam.transform.forward;
    //        yield return new WaitForSeconds(0.02f);
    //    }
    //}
    #endregion

    #region 설명창
    public GameObject GetDesc() { return Desc_UI; }
    #endregion
}
