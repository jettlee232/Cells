using BNG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;
using System;

public class UIManager_StageMap : MonoBehaviour
{
    enum ORGANELLES
    {
        Mito = 0,
        Lys = 1,
        Gol = 2,
        Nuc = 3,
        Cen = 4,
        Rib = 5,
        SER = 6,
        RER = 7,
        Per = 8
    }

    [Header("Settings")]
    public static UIManager_StageMap instance;
    public GameObject Desc_UI;
    private GameObject[] Descs;
    public GameObject UpsideSubtitle;
    public GameObject NPCTalkPanel;
    public GameObject[] TutorialPanels;
    public GameObject OrganelleDescUI;

    [Header("Variables")]
    public float UpsideSubtitleVanishTimer = 1f;
    public float DescUITimer = 0.2f;
    //private Vector3 DescUIFullSize;
    private int nowTutorial = 0;
    private Nullable<ORGANELLES> nowSelectedOrganelle;


    void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
    }

    void Start()
    {
        //DescUIFullSize = Descs[0].GetComponent<RectTransform>().localScale;
        InitDesc();
    }

    #region 소기관 설명창
    public void InitDesc()
    {
        Descs = new GameObject[Desc_UI.transform.childCount];
        for (int i = 0; i < Desc_UI.transform.childCount; i++)
        {
            Descs[i] = Desc_UI.transform.GetChild(i).gameObject;
        }
    }
    public GameObject GetDesc() { return Desc_UI; }
    public bool CheckDesc()
    {
        bool temp = false;
        foreach (GameObject organelle in Descs) { if (organelle.gameObject.activeSelf) { temp = true; break; } }
        return temp;
    }

    public void OnDesc(GameObject go)
    {
        ORGANELLES type = (ORGANELLES)Enum.Parse(typeof(ORGANELLES), go.GetComponent<DescObj_StageMap>().GetType());

        if (CheckDesc()) { Descs[(int)nowSelectedOrganelle].SetActive(false); }
        Descs[(int)type].SetActive(true);

        nowSelectedOrganelle = type;
    }
    public void OffDesc()
    {
        foreach (GameObject organelle in Descs) { if (organelle.gameObject.activeSelf) organelle.SetActive(false); }
        nowSelectedOrganelle = null;
    }


    //public void OffDesc() { StartCoroutine(VanishDesc()); }
    //IEnumerator ShowDesc(GameObject go)
    //{
    //    Desc_UI.SetActive(true);

    //    Desc_UI.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = go.GetComponent<DescObj_StageMap>().GetName();
    //    Desc_UI.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = go.GetComponent<DescObj_StageMap>().GetDesc();

    //    Vector3 nowDescSize = Desc_UI.GetComponent<RectTransform>().localScale;

    //    float timer = 0f;
    //    float totalTimer = (DescUIFullSize.x - nowDescSize.x) / DescUIFullSize.x * DescUITimer;
    //    while (true)
    //    {
    //        if (DescUIFullSize.x - Desc_UI.GetComponent<RectTransform>().localScale.x <= 0.0000001f) { break; }
    //        Desc_UI.GetComponent<RectTransform>().localScale = Vector3.Lerp(nowDescSize, DescUIFullSize, timer / totalTimer);
    //        timer += Time.deltaTime;
    //        yield return null;
    //    }
    //}
    //IEnumerator VanishDesc()
    //{
    //    Vector3 nowDescSize = Desc_UI.GetComponent<RectTransform>().localScale;

    //    float timer = 0f;
    //    float totalTimer = nowDescSize.x / DescUIFullSize.x * DescUITimer;
    //    while (true)
    //    {
    //        if (Desc_UI.GetComponent<RectTransform>().localScale.x <= 0.0000001f) { break; }
    //        Desc_UI.GetComponent<RectTransform>().localScale = Vector3.Lerp(nowDescSize, Vector3.zero, timer / totalTimer);
    //        timer += Time.deltaTime;
    //        yield return null;
    //    }

    //    Desc_UI.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
    //    Desc_UI.SetActive(false);
    //}
    //public bool CheckDesc() { return Desc_UI.activeSelf; }
    #endregion

    #region 상단 자막

    public void SetUpsideSubtitle(string des)
    {
        UpsideSubtitle.SetActive(true);
        UpsideSubtitle.GetComponent<TextMeshProUGUI>().text = des;
        UpsideSubtitleChangeEffect();        
    }

    public void ClearUpsideSubtitle()
    {
        UpsideSubtitle.SetActive(false);
        UpsideSubtitle.GetComponent<TextMeshProUGUI>().text = "";
    }

    public void VanishUpsideSubtitle()
    {
        StartCoroutine(cVanishUpsideSubtitle());
    }

    IEnumerator cVanishUpsideSubtitle()
    {
        float timer = 0f;

        Color nowColor = UpsideSubtitle.GetComponent<TextMeshProUGUI>().color;
        Color opaqueColor = new Color(nowColor.r, nowColor.g, nowColor.b, 1f);
        Color transparentColor = new Color(nowColor.r, nowColor.g, nowColor.b, 0f);

        UpsideSubtitle.gameObject.SetActive(true);

        while (true)
        {
            if (UpsideSubtitle.GetComponent<TextMeshProUGUI>().color.a <= 0.00001f) { UpsideSubtitle.GetComponent<TextMeshProUGUI>().color = transparentColor; break; }
            timer += Time.deltaTime;
            UpsideSubtitle.GetComponent<TextMeshProUGUI>().color = Color.Lerp(opaqueColor, transparentColor, timer / UpsideSubtitleVanishTimer);
            yield return null;
        }
        UpsideSubtitle.GetComponent<TextMeshProUGUI>().text = "";
        UpsideSubtitle.gameObject.SetActive(false);
    }

    public bool GetUpsideSubtitle() { return UpsideSubtitle.activeSelf; }

    private void UpsideSubtitleChangeEffect()
    {
        Vector3 temp = UpsideSubtitle.transform.localScale;
        UpsideSubtitle.transform.DOPunchScale(temp * 0.5f, 0.2f).OnComplete(() => UpsideSubtitle.transform.localScale = temp);
    }

    #endregion

    #region NPC 대화 창

    public void SetNPCPanel(string des, string name, Sprite img, string sceneName)
    {
        NPCTalkPanel.SetActive(true);
        NPCTalkPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = des;
        NPCTalkPanel.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
        NPCTalkPanel.transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<Image>().sprite = img;
        NPCTalkPanel.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<UnityEngine.UI.Button>().interactable = false;
        NPCTalkPanel.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => GameManager_StageMap.instance.MoveScene(sceneName));
    }
    public void OnClickNPCXBtn()
    {
        GameManager_StageMap.instance.SetSelectable(true);
        NPCTalkPanel.SetActive(false);
        GameManager_StageMap.instance.EnableMove();
    }
    public void EnanbleOrganelleButton()
    {
        NPCTalkPanel.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<UnityEngine.UI.Button>().interactable = true;
    }

    #endregion

    #region 튜토리얼

    public void ShowTutorial()
    {
        TutorialPanels[nowTutorial].gameObject.SetActive(true);
        GameManager_StageMap.instance.EnableMove();
    }

    public void HideTutorial()
    {
        TutorialPanels[0].transform.parent.gameObject.SetActive(false);
    }

    public void NextTutorial()
    {
        if (nowTutorial < TutorialPanels.Length - 1)
        {
            TutorialPanels[nowTutorial].SetActive(false);
            nowTutorial++;
            ShowTutorial();
        }
        else { return; }
    }
    public void PreviousTutorial()
    {
        if (nowTutorial > 0)
        {
            TutorialPanels[nowTutorial].SetActive(false);
            nowTutorial--;
            ShowTutorial();
        }
        else { return; }
    }

    #endregion

    public void ShowOrganelleUI() { OrganelleDescUI.SetActive(true); }
    public void HideOrganelleUI() { OrganelleDescUI.SetActive(false); }

    public void OnClickTitle()
    {
        OffDesc();
        GameManager_StageMap.instance.MoveScene("01_Home");
    }
}
