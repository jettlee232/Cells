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
    public GameObject alert_UI_Multi;
    public GameObject BlackPanel;
    public GameObject QuestPanel;
    public GameObject[] TutorialPanels;
    public GameObject NPCScript;
    private GameObject lastInteract = null;

    [Header("Upside Subtitle Settings")]
    public float fadeInTimer = 1f;
    public float fadeOutTimer = 1f;

    private Vector3 alertSize;
    private Vector3 tutorialSize;

    // SYS Code
    public GameObject particles;

    // SYS Code
    private Tween alertTween;
    private Tween alertTween_Multi;


    void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
    }

    void Start()
    {
        InitAlert();
        InitTutorial();
        if (GameManager_Lobby.instance.GetLobby() == 0) { StartCoroutine(cStart()); }
        else { GameManager_Lobby.instance.SetPlayerPos(new Vector3(0f, 1f, 3f)); }
        DOTween.Init();
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
        SetQuest("복도 끝으로 이동해보자!");
        yield return new WaitForSeconds(1f);
        ShowMoveTutorial();
    }

    #region 진입 알림
    public void InitAlert()
    {
        alertSize = alert_UI.transform.localScale;
        alert_UI.SetActive(false);
    }

    // SYS Code
    public void SetAlert(GameObject menu)
    {
        alert_UI.SetActive(true);
        alert_UI.transform.localScale = Vector3.zero;
        if (alertTween != null && alertTween.IsActive()) alertTween.Kill(); alertTween = null;
        alertTween = alert_UI.transform.DOScale(alertSize, 2f).OnComplete(() => { alertTween = null; });
    }

    public void SetAlert_Multi(GameObject menu)
    {
        alert_UI_Multi.SetActive(true);
        alert_UI_Multi.transform.localScale = Vector3.zero;
        if (alertTween_Multi != null && alertTween_Multi.IsActive()) alertTween_Multi.Kill(); alertTween_Multi = null;
        alertTween_Multi = alert_UI_Multi.transform.DOScale(alertSize, 2f).OnComplete(() => { alertTween_Multi = null; });
    }


    // SYS Code
    public void HideAlert()
    {
        //StartCoroutine(hideAlert());
        if (alertTween != null && alertTween.IsActive()) alertTween.Kill(); alertTween = null; // alertTween.Complete();
        alertTween = alert_UI.transform.DOScale(Vector3.zero, 1f).OnComplete(() => { alertTween = null; }); ;
    }
    IEnumerator hideAlert() { yield return new WaitForSeconds(1f); alert_UI.gameObject.SetActive(false); }

    public void HideAlert_Multi()
    {
        if (alertTween_Multi != null && alertTween_Multi.IsActive()) alertTween_Multi.Kill(); alertTween_Multi = null; // alertTween.Complete();
        alertTween_Multi = alert_UI_Multi.transform.DOScale(Vector3.zero, 1f).OnComplete(() => { alertTween_Multi = null; }); ;
    }

    public void OnClickAnimal()
    {
        HideAlert();
        GameManager_Lobby.instance.MoveScene("04_StageMap");
    }

    public void OnClickMulti()
    {
        HideAlert_Multi();

        // 멀티 진입 코드

    }

    public void OnClickNo()
    {
        HideAlert();
    }

    public void OnClickNo_Multi()
    {
        HideAlert_Multi();
    }

    #endregion

    #region 페이드인

    public void FadeIn() { StartCoroutine(cFade(true)); }
    public void FadeOut() { StartCoroutine(cFade(false)); }
    IEnumerator cFade(bool inout)   // true 로 넣으면 페이드인, false로 넣으면 페이드아웃
    {
        float timer = 0f;

        Color blackColor = new Color(0f, 0f, 0f, 1f);
        Color transparentColor = new Color(0f, 0f, 0f, 0f);

        BlackPanel.gameObject.SetActive(true);

        if (inout)  // 점점 밝아지게
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

    #region 퀘스트

    public void SetQuest(string str) { QuestPanel.GetComponent<QuestPanel_StageMap>().PanelOpen(str); }
    public void SetQuest(string str, float time) { QuestPanel.GetComponent<QuestPanel_StageMap>().PanelOpen(str, time); }
    public void ChangeQuest(string str) { QuestPanel.GetComponent<QuestPanel_StageMap>().ChangeText(str); }
    public void HideQuest() { QuestPanel.GetComponent<QuestPanel_StageMap>().PanelClose(); }
    public bool GetQuest() { return QuestPanel.activeSelf; }

    #endregion

    #region 튜토리얼 UI

    public void ShowMoveTutorial()
    {
        ShowTutorialTween(TutorialPanels[0]);
        GameManager_Lobby.instance.EnableMovePlayer();

        // SYS Code
        ShowTutorialParticle(0);
    }

    // SYS Code
    public void ShowPressTutorial()
    {
        if (TutorialPanels[0].activeSelf) { ChangeTutorialTween(); } // SYS Code
        else { ShowTutorialTween(TutorialPanels[1]); ShowTutorialParticle(1); }
    }
    public void HideMoveTutorial()
    {
        HideTutorialTween(TutorialPanels[0]);
        if (PlayerPrefs.GetInt("Lobby") == 0) AudioMgr_CM.Instance.PlaySFXByInt(16); // SSS
    }
    public void HidePressTutorial() { HideTutorialTween(TutorialPanels[1]); }

    public void InitTutorial()
    {
        tutorialSize = TutorialPanels[0].gameObject.transform.localScale;
        TutorialPanels[0].gameObject.SetActive(false);
        TutorialPanels[1].gameObject.SetActive(false);
    }

    public void ShowTutorialTween(GameObject tuto)
    {
        tuto.transform.localScale = Vector3.zero;
        tuto.gameObject.SetActive(true);
        tuto.transform.DOScale(tutorialSize, 2f);

        // SYS Code
        tuto.transform.rotation = Quaternion.identity;

        Transform grandParent = tuto.transform.parent.parent.parent;
        tuto.transform.DORotateQuaternion(Quaternion.Euler(0f, 180f, 0f), 1f);

        if (PlayerPrefs.GetInt("Lobby") == 0) AudioMgr_CM.Instance.PlaySFXByInt(4); // SSS               
    }

    public void HideTutorialTween(GameObject tuto)
    {
        StartCoroutine(hideTuto(tuto));
        tuto.transform.DOScale(Vector3.zero, 1f);

        // SYS Code
        //tuto.transform.DORotate(new Vector3(0, 0, 0), 1f, RotateMode.FastBeyond360);
        tuto.transform.DORotateQuaternion(Quaternion.Euler(0f, 0f, 0f), 1f);
        AudioMgr_CM.Instance.PlaySFXByInt(16);
    }
    IEnumerator hideTuto(GameObject tuto) { yield return new WaitForSeconds(1f); tuto.gameObject.SetActive(false); }

    // SYS Code
    public void ChangeTutorialTween()
    {
        HideMoveTutorial();
        ShowTutorialTween(TutorialPanels[1]);
        ShowTutorialParticle(1);
        //TutorialPanels[1].gameObject.SetActive(true);
        //TutorialPanels[0].gameObject.SetActive(false);
        //SYS Code -> // TutorialPanels[1].gameObject.transform.DOPunchScale(tutorialSize * 0.2f, 0.2f).OnComplete(() => TutorialPanels[1].gameObject.transform.localScale = tutorialSize);
    }

    // SYS Code
    public void ShowTutorialParticle(int index)
    {
        GameObject go = Instantiate(particles);
        go.transform.position = TutorialPanels[index].transform.position;
        go.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);

        go.GetComponent<ParticleSystem>().Play();
    }

    #endregion

    #region NPC 말풍선

    public void ShowBubble()
    {
        NPCScript.GetComponent<SpeechBubble_StageMap>().PanelOpen("준비가 되면 다시 내게 말을 걸어줘~");
    }
    public void HideBubble() { NPCScript.GetComponent<SpeechBubble_StageMap>().PanelClose(); }

    #endregion
}
