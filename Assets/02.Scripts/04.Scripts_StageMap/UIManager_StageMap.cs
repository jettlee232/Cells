using BNG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using static UnityEngine.Rendering.DebugUI;

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
    public GameObject QuestPanel;
    public GameObject[] TutorialPanels;
    public GameObject OrganelleDescUI;
    public GameObject OptionPanel;

    [Header("Variables")]
    public float UpsideSubtitleVanishTimer = 1f;
    public float DescUITimer = 0.2f;
    private int nowTutorial = 0;
    private Nullable<ORGANELLES> nowSelectedOrganelle;
    private Vector3 tutoSize;
    private Vector3 organelleExSize;


    void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
    }

    void Start()
    {
        InitDesc();
        InitTutorial();
        InitOrganelleUI();
        DOTween.Init();
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

        if (CheckDesc() && nowSelectedOrganelle != null) { StartCoroutine(DestroyAfterRewind(Descs[(int)nowSelectedOrganelle])); }
        StartCoroutine(ShowDesc(Descs[(int)type].gameObject));

        nowSelectedOrganelle = type;
    }
    public void OffDesc()
    {
        foreach (GameObject organelleUI in Descs)
        {
            if (organelleUI.gameObject.activeSelf)
            {
                transform.DOScale(Vector3.zero, 1f);
                transform.DORotate(new Vector3(0f, 360f, 0f), 1f, RotateMode.FastBeyond360);
                StartCoroutine(DestroyAfterRewind(organelleUI.gameObject));
                break;
            }
        }
        GameManager_StageMap.instance.GetUIPointer().GetComponent<LaserPointer_StageMap>().InitObj();
        nowSelectedOrganelle = null;
    }
    private IEnumerator DestroyAfterRewind(GameObject organelle)
    {
        yield return null;
        organelle.SetActive(false);
    }
    private IEnumerator ShowDesc(GameObject organelleUI)
    {
        yield return new WaitForSeconds(0.05f);
        organelleUI.SetActive(true);
        organelleUI.transform.localPosition = Vector3.zero;
        organelleUI.transform.localRotation = Quaternion.Euler(0, 0, 0);
        organelleUI.transform.localScale = Vector3.zero;
        organelleUI.transform.DOScale(new Vector3(1f, 1f, 1f), 1f);
        //organelleUI.transform.DORotate(new Vector3(0f, 360f + organelleUI.transform.parent.parent.parent.GetComponent<RectTransform>().localEulerAngles.y, 0f), 1f, RotateMode.FastBeyond360);
        //organelleUI.transform.DORotate(new Vector3(0f, 360f + organelleUI.transform.parent.transform.localEulerAngles.y, 0f), 1f, RotateMode.FastBeyond360);
        //organelleUI.transform.DORotate(new Vector3(0f, 360f, 0f), 1f, RotateMode.FastBeyond360);
        organelleUI.transform.DORotate(new Vector3(0f, 360f, 0f), 1f, RotateMode.FastBeyond360).SetRelative(true);

    }

    public void EnanbleOrganelleButton()
    {
        foreach (GameObject organelle in Descs)
        {
            organelle.transform.GetChild(3).GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
    }
    #endregion

    #region 퀘스트

    public void SetQuest(string str) { QuestPanel.GetComponent<QuestPanel_StageMap>().PanelOpen(str); }
    public void SetQuest(string str, float time) { QuestPanel.GetComponent<QuestPanel_StageMap>().PanelOpen(str, time); }
    public void ChangeQuest(string str) { QuestPanel.GetComponent<QuestPanel_StageMap>().ChangeText(str); }
    public void HideQuest() { QuestPanel.GetComponent<QuestPanel_StageMap>().PanelClose(); }
    public bool GetQuest() { return QuestPanel.activeSelf; }

    #endregion

    #region 튜토리얼

    public void InitTutorial()
    {
        tutoSize = TutorialPanels[0].gameObject.transform.localScale;
        foreach (GameObject tuto in TutorialPanels) { tuto.SetActive(false); }
    }
    public void ShowTutorial()
    {
        TutorialPanels[nowTutorial].gameObject.SetActive(true);
        GameManager_StageMap.instance.EnableMove();
        TutorialPanels[nowTutorial].transform.localScale = Vector3.zero;
        TutorialPanels[nowTutorial].transform.DOScale(tutoSize, 2f);
    }

    public void NextTutorial()
    {
        if (nowTutorial < TutorialPanels.Length - 1)
        {
            TutorialPanels[nowTutorial].SetActive(false);
            nowTutorial++;
            TutorialPanels[nowTutorial].gameObject.SetActive(true);
            TutorialPanels[nowTutorial].gameObject.transform.DOPunchScale(tutoSize * 0.2f, 0.2f).OnComplete(() => TutorialPanels[nowTutorial].gameObject.transform.localScale = tutoSize);
        }
        else
        {
            foreach (GameObject panel in TutorialPanels)
            {
                StartCoroutine(hideTuto(panel));
                panel.transform.DOScale(Vector3.zero, 1f);
            }
        }
    }

    IEnumerator hideTuto(GameObject tuto) { yield return new WaitForSeconds(1f); tuto.gameObject.SetActive(false); }

    #endregion

    #region 옵션창

    public void RotateUp()
    {
        GameManager_StageMap.instance.GetPlayer().GetComponent<PlayerMoving_StageMap>().RotateUp();
        OptionPanel.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = ((int)(GameManager_StageMap.instance.GetPlayer().GetComponent<PlayerMoving_StageMap>().GetRotateSpeed() / 10f)).ToString();
    }
    public void RotateDown() { GameManager_StageMap.instance.GetComponent<PlayerMoving_StageMap>().RotateDown(); }

    #endregion

    #region "소기관"

    public void InitOrganelleUI() { organelleExSize = OrganelleDescUI.transform.localScale; OrganelleDescUI.SetActive(false); }
    public void ShowOrganelleUI()
    {
        OrganelleDescUI.SetActive(true);
        OrganelleDescUI.transform.localScale = Vector3.zero;
        OrganelleDescUI.transform.DOScale(organelleExSize, 2f);
    }
    public void HideOrganelleUI()
    {
        StartCoroutine(hideOrganelle());
        OrganelleDescUI.transform.DOScale(Vector3.zero, 1f);
    }
    IEnumerator hideOrganelle() { yield return new WaitForSeconds(1f); OrganelleDescUI.SetActive(false); }

    #endregion
    public void OnClickTitle()
    {
        OffDesc();
        GameManager_StageMap.instance.MoveScene("01_Home");
    }
}
