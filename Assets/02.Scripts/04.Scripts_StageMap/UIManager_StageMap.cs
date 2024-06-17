using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_StageMap : MonoBehaviour
{
    public static UIManager_StageMap instance;
    public GameObject Desc_UI;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
    }

    void Start()
    {
        Desc_UI.SetActive(false);
    }

    //void Update()
    //{

    //}

    #region 소기관 설명창
    public GameObject GetDesc() { return Desc_UI; }

    public void OnDesc() { Desc_UI.SetActive(true); }
    public void OffDesc() { Desc_UI.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = ""; Desc_UI.SetActive(false); }
    public bool CheckDesc() { return Desc_UI.activeSelf; }
    public void EnableButton() { Desc_UI.transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Button>().interactable = true; }
    #endregion

    public void OnClickTitle()
    {
        OffDesc();
        GameManager_StageMap.instance.MoveScene("01_Home");
    }
}
