using BNG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UIManager_Lys : MonoBehaviour
{
    enum ENEMY
    {
        DP = 0,     // 손상된 단백질
        CD = 1,     // 세포 잔해
        ES = 2,     // 외부 물질
    }

    [Header("Settings")]
    public static UIManager_Lys instance;
    public GameObject Desc_UI;      // 노폐물들 소개 UI용
    private GameObject[] Descs;
    public GameObject QuestPanel;
    public GameObject[] TutorialPanels;
    public GameObject OptionPanel;
    public GameObject BlackPanel;
    public GameObject tooltip;

    [Header("Variables")]
    public float UpsideSubtitleVanishTimer = 1f;
    public float DescUITimer = 0.2f;
    public float fadeInTimer = 1f;
    public float fadeOutTimer = 1f;
    private int nowTutorial = 0;
    private Nullable<ENEMY> nowSelectedEnemy;
    private Vector3 tutoSize;
    private Vector3 DescSize;

    // SYS Code
    public ParticleSystem handPanelParticle;
    private bool isDescClosing = false;


    void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
    }

    void Start()
    {
        InitDesc();
        //InitTutorial();
        DOTween.Init();
        FadeIn();
        SetQuest("NPC에게 말을 걸어보자!");

        // SYS Code
        handPanelParticle.Stop();
    }

    #region 노폐물 설명창
    public void InitDesc()
    {
        Descs = new GameObject[Desc_UI.transform.childCount];
        for (int i = 0; i < Desc_UI.transform.childCount; i++)
        {
            Descs[i] = Desc_UI.transform.GetChild(i).gameObject;
        }
    }
    public bool CheckDesc()
    {
        bool temp = false;
        foreach (GameObject organelle in Descs) { if (organelle.gameObject.activeSelf) { temp = true; break; } }
        return temp;
    }
    public GameObject GetDesc() { return Desc_UI; }

    public void OnDesc(GameObject go)
    {
        ENEMY type = (ENEMY)Enum.Parse(typeof(ENEMY), go.GetComponent<EnemyType_Lys>().GetType());

        if (CheckDesc() && nowSelectedEnemy != null) { StartCoroutine(DestroyAfterRewind(Descs[(int)nowSelectedEnemy])); }
        StartCoroutine(ShowDesc(Descs[(int)type].gameObject));

        nowSelectedEnemy = type;

        AudioMgr_CM.Instance.PlaySFXByInt(4);
        handPanelParticle.Play();
    }
    public void OffDesc()
    {
        if (isDescClosing == false)
        {
            foreach (GameObject enemyUI in Descs)
            {
                if (enemyUI.gameObject.activeSelf)
                {
                    // SYS Code
                    isDescClosing = true;
                    enemyUI.transform.DOScale(Vector3.zero, 1f);
                    enemyUI.transform.DORotate(new Vector3(0f, 360f, 0f), 1f, RotateMode.FastBeyond360).OnComplete(() =>
                    {
                        enemyUI.SetActive(false);
                        isDescClosing = false;
                    });
                    //StartCoroutine(DestroyAfterRewind(enemyUI.gameObject));
                    break;
                }
            }
            GameManager_Lys.instance.GetUIPointer().GetComponent<LaserPointer_Lys>().InitObj();
            nowSelectedEnemy = null;

            AudioMgr_CM.Instance.PlaySFXByInt(16);
        }        
    }
    private IEnumerator DestroyAfterRewind(GameObject enemy)
    {
        yield return null;
        enemy.SetActive(false);
    }
    private IEnumerator ShowDesc(GameObject enemyUI)
    {
        yield return new WaitForSeconds(0.05f);
        enemyUI.SetActive(true);
        enemyUI.transform.localPosition = Vector3.zero;
        enemyUI.transform.localRotation = Quaternion.Euler(0, 0, 0);
        enemyUI.transform.localScale = Vector3.zero;
        enemyUI.transform.DOScale(new Vector3(.55f, .55f, .55f), 1f);
        enemyUI.transform.DORotate(new Vector3(0f, 360f, 0f), 1f, RotateMode.FastBeyond360).SetRelative(true);

    }
    #endregion

    #region 퀘스트

    public void SetQuest(string str)
    {
        if (QuestPanel.transform.GetChild(0).gameObject.activeSelf == false) QuestPanel.transform.GetChild(0).gameObject.SetActive(true);
        QuestPanel.GetComponent<QuestPanel_Lys>().PanelOpen(str);
    }
    public void SetQuest(string str, float time)
    {
        if (QuestPanel.transform.GetChild(0).gameObject.activeSelf == false) QuestPanel.transform.GetChild(0).gameObject.SetActive(true);
        QuestPanel.GetComponent<QuestPanel_Lys>().PanelOpen(str, time);
    }
    public void ChangeQuest(string str) { QuestPanel.GetComponent<QuestPanel_Lys>().ChangeText(str); }
    public void HideQuest() { QuestPanel.GetComponent<QuestPanel_Lys>().PanelClose(); }
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
        GameManager_Lys.instance.EnableMove();
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
        GameManager_Lys.instance.GetPlayer().GetComponent<PlayerMoving_Lys>().RotateUp();
        OptionPanel.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = ((int)(GameManager_Lys.instance.GetPlayer().GetComponent<PlayerMoving_Lys>().GetRotateSpeed() / 10f)).ToString();
    }
    public void RotateDown() { GameManager_Lys.instance.GetComponent<PlayerMoving_Lys>().RotateDown(); }

    #endregion

    #region 까만 패널

    public void FadeIn() { StartCoroutine(cFadeIn()); }
    IEnumerator cFadeIn()
    {
        float timer = 0f;

        Color blackColor = new Color(0f, 0f, 0f, 1f);
        Color transparentColor = new Color(0f, 0f, 0f, 0f);

        BlackPanel.gameObject.SetActive(true);

        BlackPanel.GetComponent<Image>().color = blackColor;
        while (true)
        {
            if (BlackPanel.GetComponent<Image>().color.a <= 0.00001f) { BlackPanel.GetComponent<Image>().color = transparentColor; break; }
            timer += Time.deltaTime;
            BlackPanel.GetComponent<Image>().color = Color.Lerp(blackColor, transparentColor, timer / fadeInTimer);
            yield return null;
        }
        BlackPanel.gameObject.SetActive(false);
    }

    public GameObject GetBlackPanel() { return BlackPanel; }
    public float GetFadeOutTimer() { return fadeOutTimer; }

    #endregion

    #region 툴팁
    public void ShowTalkToolTip() { tooltip.GetComponent<Tooltip>().TooltipOn("A키를 눌러 NPC에게 말을 걸자!"); }
    public void ShowShootToolTip() { tooltip.GetComponent<Tooltip>().TooltipOn("트리거 키를 눌러 발포하자!"); }
    public void HideToolTip() { tooltip.GetComponent<Tooltip>().TooltipOff(); }
    #endregion

    public void OnClickTitle()
    {
        OffDesc();
        GameManager_Lys.instance.MoveScene("01_Home");
    }
}
