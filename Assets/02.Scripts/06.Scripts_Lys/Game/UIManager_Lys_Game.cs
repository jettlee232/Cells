using BNG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UIManager_Lys_Game : MonoBehaviour
{
    enum ENEMY
    {
        DP = 0,     // 손상된 단백질
        CD = 1,     // 세포 잔해
        ES = 2,     // 외부 물질
    }

    [Header("Settings")]
    public static UIManager_Lys_Game instance;
    public GameObject QuestPanel;
    public GameObject OptionPanel;
    public GameObject GameClearPanel;
    public GameObject GameOverPanel;
    public GameObject BlackPanel;
    public BNG.MyFader_CM scrFader;

    [Header("Variables")]
    public float UpsideSubtitleVanishTimer = 1f;
    public float DescUITimer = 0.2f;
    public float fadeInTimer = 1f;
    public float fadeOutTimer = 1f;


    void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
    }

    void Start()
    {
        //InitTutorial();
        DOTween.Init();
        FadeIn();
        SetQuest("A키를 눌러 무기를 바꿀 수 있다!", 10f);
    }

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

    #region 옵션창

    public void RotateUp()
    {
        GameManager_Lys.instance.GetPlayer().GetComponent<PlayerMoving_Lys>().RotateUp();
        OptionPanel.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = ((int)(GameManager_Lys.instance.GetPlayer().GetComponent<PlayerMoving_Lys>().GetRotateSpeed() / 10f)).ToString();
    }
    public void RotateDown() { GameManager_Lys.instance.GetComponent<PlayerMoving_Lys>().RotateDown(); }

    #endregion

    #region 게임오버/클리어
    public void ShowGameClear()
    {
        GameClearPanel.SetActive(true);
    }
    public void ShowGameOver()
    {
        GameOverPanel.SetActive(true);
    }
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

    #region 피격이펙트

    public void RedFade()
    {
        StartCoroutine(WrondAnswerEffect());
    }
    IEnumerator WrondAnswerEffect()
    {
        scrFader.ChangeFadeImageColor(Color.red, 12f, 0.33f);
        scrFader.DoFadeIn();

        yield return new WaitForSeconds(0.75f);

        scrFader.DoFadeOut();
    }

    #endregion

    public void OnClickTitle()
    {
        GameManager_Lys.instance.MoveScene("01_Home");
    }
}
