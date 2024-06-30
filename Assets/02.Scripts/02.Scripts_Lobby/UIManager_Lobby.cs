using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Meta.WitAi.Utilities;

public class UIManager_Lobby : MonoBehaviour
{
    [Header("Settings")]
    public static UIManager_Lobby instance;
    public GameObject alert_UI;
    public GameObject BlackPanel;
    public GameObject QuestPanel;
    public GameObject[] TutorialPanels;
    public GameObject NPCScript;
    private GameObject lastInteract = null;

    [Header("Upside Subtitle Settings")]
    public float fadeInTimer = 1f;
    public float fadeOutTimer = 1f;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
    }

    void Start()
    {
        alert_UI.SetActive(false);
        StartCoroutine(cStart());
    }

    public void OnClickTitle()
    {
        alert_UI.SetActive(false);
        GameManager_Lobby.instance.MoveScene("01_Home");
    }

    IEnumerator cStart()
    {
        GameManager_Lobby.instance.StopPlayer();
        yield return cFade(true);
        GameManager_Lobby.instance.EnableMovePlayer();
        SetQuest("���� ������ �̵��غ���!");
        ShowMoveTutorial();
    }

    #region ���� �˸�
    public void SetAlert(GameObject menu)
    {
        lastInteract = menu;
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

    #region ���̵���

    public void FadeIn() { StartCoroutine(cFade(true)); }
    public void FadeOut() { StartCoroutine(cFade(false)); }
    IEnumerator cFade(bool inout)   // true �� ������ ���̵���, false�� ������ ���̵�ƿ�
    {
        float timer = 0f;

        Color blackColor = new Color(0f, 0f, 0f, 1f);
        Color transparentColor = new Color(0f, 0f, 0f, 0f);

        BlackPanel.gameObject.SetActive(true);

        if (inout)  // ���� �������
        {
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
        else
        {
            BlackPanel.GetComponent<Image>().color = transparentColor;
            while (true)
            {
                if (BlackPanel.GetComponent<Image>().color.a >= 0.99999f) { BlackPanel.GetComponent<Image>().color = blackColor; break; }
                timer += Time.deltaTime;
                BlackPanel.GetComponent<Image>().color = Color.Lerp(transparentColor, blackColor, timer / fadeOutTimer);
                yield return null;
            }
        }
    }

    #endregion

    #region ����Ʈ

    public void SetQuest(string str) { QuestPanel.GetComponent<QuestPanel_CM>().PanelOpen(str); }
    public void SetQuest(string str, float time) { QuestPanel.GetComponent<QuestPanel_CM>().PanelOpen(str, time); }
    public void ChangeQuest(string str) { QuestPanel.GetComponent<QuestPanel_CM>().ChangeText(str); }
    public void HideQuest() { QuestPanel.GetComponent<QuestPanel_CM>().PanelClose(); }
    public bool GetQuest() { return QuestPanel.activeSelf; }

    #endregion

    #region Ʃ�丮�� UI

    public void ShowMoveTutorial()
    {
        TutorialPanels[0].gameObject.SetActive(true);
        TutorialPanels[1].gameObject.SetActive(false);
        GameManager_Lobby.instance.EnableMovePlayer();
    }
    public void ShowPressTutorial()
    {
        TutorialPanels[0].gameObject.SetActive(false);
        TutorialPanels[1].gameObject.SetActive(true);
    }
    public void HideTutorial()
    {
        TutorialPanels[0].gameObject.SetActive(false);
        TutorialPanels[1].gameObject.SetActive(false);
    }

    #endregion

    #region NPC ��ǳ��

    public void ShowBubble()
    {
        NPCScript.GetComponent<SpeechBubble_StageMap>().PanelOpen("�غ� �Ǹ� �ٽ� ���� ���� �ɾ���~");
    }
    public void HideBubble() { NPCScript.GetComponent<SpeechBubble_StageMap>().PanelClose(); }

    #endregion
}
