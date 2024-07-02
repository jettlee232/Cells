using BNG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
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
    public GameObject QuestPanel;
    public GameObject NPCTalkPanel;
    public GameObject NPCTalkButton;
    public GameObject[] TutorialPanels;
    public GameObject OrganelleDescUI;
    public GameObject OptionPanel;

    [Header("Variables")]
    public float UpsideSubtitleVanishTimer = 1f;
    public float DescUITimer = 0.2f;
    private int nowTutorial = 0;
    private Nullable<ORGANELLES> nowSelectedOrganelle;


    void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
    }

    void Start()
    {
        InitDesc();
        DOTween.Init();
    }

    #region �ұ�� ����â
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
        //organelleUI.transform.DORotate(new Vector3(0f, 360f + transform.parent.GetComponent<RectTransform>().localEulerAngles.y, 0f), 1f, RotateMode.FastBeyond360);
        //organelleUI.transform.DORotate(new Vector3(0f, 360f + organelleUI.transform.parent.gameObject.transform.localEulerAngles.y, 0f), 1f, RotateMode.FastBeyond360);
        organelleUI.transform.DORotate(new Vector3(0f, 360f + GameManager_StageMap.instance.GetPlayer().transform.localEulerAngles.y, 0f), 1f, RotateMode.FastBeyond360);
    }
    #endregion

    #region ����Ʈ

    public void SetQuest(string str) { QuestPanel.GetComponent<QuestPanel_CM>().PanelOpen(str); }
    public void SetQuest(string str, float time) { QuestPanel.GetComponent<QuestPanel_CM>().PanelOpen(str, time); }
    public void ChangeQuest(string str) { QuestPanel.GetComponent<QuestPanel_CM>().ChangeText(str); }
    public void HideQuest() { QuestPanel.GetComponent<QuestPanel_CM>().PanelClose(); }
    public bool GetQuest() { return QuestPanel.activeSelf; }

    #endregion

    #region NPC ��ȭ â

    public void SetNPCPanel(string des, string name, Sprite img, string sceneName)
    {
        NPCTalkPanel.SetActive(true);
        NPCTalkPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = des;
        NPCTalkPanel.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
        NPCTalkPanel.transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<Image>().sprite = img;
    }
    public void OnClickNPCXBtn()
    {
        GameManager_StageMap.instance.SetSelectable(true);
        NPCTalkPanel.SetActive(false);
        GameManager_StageMap.instance.EnableMove();
    }
    public void HideNPCPanel() { NPCTalkPanel.SetActive(false); }
    public void EnanbleOrganelleButton() { NPCTalkButton.GetComponent<UnityEngine.UI.Button>().interactable = true; }

    #endregion

    #region Ʃ�丮��

    public void ShowTutorial()
    {
        TutorialPanels[nowTutorial].gameObject.SetActive(true);
        GameManager_StageMap.instance.EnableMove();
    }

    public void NextTutorial()
    {
        if (nowTutorial < TutorialPanels.Length - 1)
        {
            TutorialPanels[nowTutorial].SetActive(false);
            nowTutorial++;
            ShowTutorial();
        }
        else
        {
            foreach (GameObject panel in TutorialPanels) { panel.SetActive(false); }
        }
    }

    #endregion

    #region �ɼ�â

    public void RotateUp()
    {
        GameManager_StageMap.instance.GetPlayer().GetComponent<PlayerMoving_StageMap>().RotateUp();
        OptionPanel.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = ((int)(GameManager_StageMap.instance.GetPlayer().GetComponent<PlayerMoving_StageMap>().GetRotateSpeed() / 10f)).ToString();
    }
    public void RotateDown() { GameManager_StageMap.instance.GetComponent<PlayerMoving_StageMap>().RotateDown(); }

    #endregion

    public void ShowOrganelleUI() { OrganelleDescUI.SetActive(true); }
    public void HideOrganelleUI() { OrganelleDescUI.SetActive(false); }

    public void OnClickTitle()
    {
        OffDesc();
        GameManager_StageMap.instance.MoveScene("01_Home");
    }
}
