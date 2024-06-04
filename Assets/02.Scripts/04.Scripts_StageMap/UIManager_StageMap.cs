using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_StageMap : MonoBehaviour
{
    public static UIManager_StageMap instance;
    public GameObject Desc_UI;

    void Awake()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else if (instance != this) { Destroy(gameObject); }
    }

    void Start()
    {
        Desc_UI.SetActive(false);
    }

    //void Update()
    //{

    //}

    #region º≥∏Ì√¢
    public GameObject GetDesc() { return Desc_UI; }
    #endregion
}
