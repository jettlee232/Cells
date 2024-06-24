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
    public GameObject Desc_UI;
    public GameObject BlackPanel;
    public GameObject UpsideSubtitle;
    public GameObject TutorialPanel;
    public GameObject NPCScript;
    private GameObject lastInteract = null;

    [Header("Upside Subtitle Settings")]
    public float fadeInTimer = 1f;
    public float fadeOutTimer = 1f;
    public float UpsideSubtitleVanishTimer = 1f;
    public float TutorialShowTimer = 0.3f;
    public float TutorialVanishTimer = 0.3f;
    public float NPCScriptShowTimer = 0.5f;
    public float NPCScriptHideTimer = 0.5f;

    [Header("Tutorial Settings")]
    public string movingTutorialText = "하...";
    public Sprite movingTutorialImage;
    public string pressTutorialText = "이건 버튼 누르라는 설명";
    public Sprite pressTutorialImage;

    // 아래로는 튜토리얼 UI에 쓰일 변수들
    private Color tNowPanelColor;
    private Color tNowImageColor;
    private Color tNowTextColor;
    private Color tOpaquePanelColor;
    private Color tOpaqueImageColor;
    private Color tOpaqueTextColor;
    private Color tTransparentPanelColor;
    private Color tTransparentImageColor;
    private Color tTransparentTextColor;

    // 아래로는 NPC 말풍선에 쓰일 변수들
    private Vector3 NPCTalkSize;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
    }

    void Start()
    {
        alert_UI.SetActive(false);
        Desc_UI.SetActive(false);
        ClearUpsideSubtitle();
        InitTutorialUI();
        InitNPCScript();
        StartCoroutine(cStart());
    }

    public void OnClickTitle()
    {
        alert_UI.SetActive(false);
        Desc_UI.SetActive(false);
        GameManager_Lobby.instance.MoveScene("01_Home");
    }

    IEnumerator cStart()
    {
        GameManager_Lobby.instance.StopPlayer();
        yield return cFade(true);
        GameManager_Lobby.instance.EnableMovePlayer();
        SetUpsideSubtitle("복도 끝으로 이동해보자!");
        SetTutorial(movingTutorialText, movingTutorialImage);
    }

    #region 진입 알림
    public void SetAlert(GameObject menu)
    {
        lastInteract = menu;
        alert_UI.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = lastInteract.GetComponent<SelectMenu_Lobby>().GetName();
        alert_UI.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = lastInteract.GetComponent<SelectMenu_Lobby>().GetDescription();
        alert_UI.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(() => GameManager_Lobby.instance.MoveScene(menu.GetComponent<SelectMenu_Lobby>().GetSceneName()));

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

    #region 설명창
    public GameObject GetDesc() { return Desc_UI; }
    public void OnDesc() { Desc_UI.SetActive(true); }
    public void OffDesc() { StartCoroutine(VanishDesc()); }
    IEnumerator VanishDesc()
    {
        while (Desc_UI.GetComponent<RectTransform>().localScale.x >= 0.00005f)
        {
            Desc_UI.GetComponent<RectTransform>().localScale =
            new Vector3(Desc_UI.GetComponent<RectTransform>().localScale.x - 0.0002f,
            Desc_UI.GetComponent<RectTransform>().localScale.y - 0.0002f,
            Desc_UI.GetComponent<RectTransform>().localScale.z - 0.0002f);
            yield return null;
        }
        Desc_UI.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        Desc_UI.SetActive(false);
    }
    public bool CheckDesc() { return Desc_UI.activeSelf; }
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

    public void VanishUpsideSubtitle() { StartCoroutine(cVanishUpsideSubtitle()); }

    private void UpsideSubtitleChangeEffect()
    {
        Vector3 temp = UpsideSubtitle.transform.localScale;
        UpsideSubtitle.transform.DOPunchScale(temp * 0.5f, 0.2f).OnComplete(() => UpsideSubtitle.transform.localScale = temp);
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

    #endregion

    #region 튜토리얼 UI

    private void InitTutorialUI()
    {
        tOpaquePanelColor = TutorialPanel.GetComponent<Image>().color;
        tOpaqueImageColor = TutorialPanel.transform.GetChild(0).GetComponent<Image>().color;
        tOpaqueTextColor = TutorialPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color;
        tTransparentPanelColor = new Color(tOpaquePanelColor.r, tOpaquePanelColor.g, tOpaquePanelColor.b, 0f);
        tTransparentImageColor = new Color(tOpaqueImageColor.r, tOpaqueImageColor.g, tOpaqueImageColor.b, 0f);
        tTransparentTextColor = new Color(tOpaqueTextColor.r, tOpaqueTextColor.g, tOpaqueTextColor.b, 0f);

        TutorialPanel.gameObject.SetActive(false);
        TutorialPanel.transform.GetChild(0).gameObject.SetActive(false);
        TutorialPanel.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void SetTutorial(string des, Sprite img) { StartCoroutine(cShowTutorial(des, img)); }

    public void VanishTutorial() { StartCoroutine(cVanishTutorial()); }

    IEnumerator cVanishTutorial()
    {
        float timer = 0f;

        tNowPanelColor = TutorialPanel.GetComponent<Image>().color;
        tNowImageColor = TutorialPanel.transform.GetChild(0).GetComponent<Image>().color;
        tNowTextColor = TutorialPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color;

        TutorialPanel.gameObject.SetActive(true);
        TutorialPanel.transform.GetChild(0).gameObject.SetActive(true);
        TutorialPanel.transform.GetChild(1).gameObject.SetActive(true);

        float totalTimer = tNowPanelColor.a / tOpaquePanelColor.a * TutorialVanishTimer;

        while (true)
        {
            if (TutorialPanel.GetComponent<Image>().color.a <= 0.00001f)
            {
                TutorialPanel.GetComponent<Image>().color = tTransparentPanelColor;
                TutorialPanel.transform.GetChild(0).GetComponent<Image>().color = tTransparentImageColor;
                TutorialPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = tTransparentTextColor;
                break;
            }
            timer += Time.deltaTime;
            TutorialPanel.GetComponent<Image>().color = Color.Lerp(tOpaquePanelColor, tTransparentPanelColor, timer / totalTimer);
            TutorialPanel.transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(tOpaqueImageColor, tTransparentImageColor, timer / totalTimer);
            TutorialPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.Lerp(tOpaqueTextColor, tTransparentTextColor, timer / totalTimer);
            yield return null;
        }
        TutorialPanel.gameObject.SetActive(false);
        TutorialPanel.transform.GetChild(0).gameObject.SetActive(false);
        TutorialPanel.transform.GetChild(1).gameObject.SetActive(false);
    }

    IEnumerator cShowTutorial(string des, Sprite img)
    {
        float timer = 0f;

        TutorialPanel.gameObject.SetActive(true);
        TutorialPanel.transform.GetChild(0).gameObject.SetActive(true);
        TutorialPanel.transform.GetChild(1).gameObject.SetActive(true);

        TutorialPanel.transform.GetChild(0).GetComponent<Image>().sprite = img;
        TutorialPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = des;

        if (tOpaquePanelColor.a - TutorialPanel.GetComponent<Image>().color.a <= 0.00001f)
        {
            TutorialChangeEffect();
            yield break;
        }

        tNowPanelColor = TutorialPanel.GetComponent<Image>().color;
        tNowImageColor = TutorialPanel.transform.GetChild(0).GetComponent<Image>().color;
        tNowTextColor = TutorialPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color;

        float totalTimer = (tOpaquePanelColor.a - tNowPanelColor.a) / tOpaquePanelColor.a * TutorialShowTimer;

        while (true)
        {
            if (tOpaquePanelColor.a - TutorialPanel.GetComponent<Image>().color.a <= 0.00001f)
            {
                TutorialPanel.GetComponent<Image>().color = tOpaquePanelColor;
                TutorialPanel.transform.GetChild(0).GetComponent<Image>().color = tOpaqueImageColor;
                TutorialPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = tOpaqueTextColor;
                break;
            }
            timer += Time.deltaTime;
            TutorialPanel.GetComponent<Image>().color = Color.Lerp(tTransparentPanelColor, tOpaquePanelColor, timer / totalTimer);
            TutorialPanel.transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(tTransparentImageColor, tOpaqueImageColor, timer / totalTimer);
            TutorialPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.Lerp(tTransparentTextColor, tOpaqueTextColor, timer / totalTimer);
            yield return null;
        }
    }

    private void TutorialChangeEffect()
    {
        Vector3 temp = TutorialPanel.transform.localScale;
        TutorialPanel.transform.DOPunchScale(temp * 0.5f, 0.2f).OnComplete(() => TutorialPanel.transform.localScale = temp);
    }

    public string GetPressTutorialText() { return pressTutorialText; }
    public Sprite GetPressTutorialImage() { return pressTutorialImage; }

    #endregion

    #region NPC 말풍선

    // 임시로 하드코딩함... 나중에 NPC 대사 늘어나면 수정함

    private void InitNPCScript()
    {
        NPCTalkSize = NPCScript.gameObject.transform.localScale;
        NPCScript.gameObject.SetActive(false);
    }

    public void ShowNPCTalk() { StartCoroutine(cShowNPCTalk()); }
    public void HideNPCTalk() { StartCoroutine(cVanishNPCTalk()); }

    IEnumerator cShowNPCTalk()
    {
        NPCScript.SetActive(true);
        float timer = 0f;

        Vector3 nowSize = NPCScript.transform.localScale;
        float totalTimer = (NPCTalkSize.x - nowSize.x) / NPCTalkSize.x * NPCScriptShowTimer;

        while (true)
        {
            if (NPCTalkSize.x - NPCScript.transform.localScale.x <= 0.00005f) { break; }

            NPCScript.transform.localScale = Vector3.Lerp(nowSize, NPCTalkSize, timer / totalTimer);
            yield return null;
        }
    }

    IEnumerator cVanishNPCTalk()
    {
        float timer = 0f;

        Vector3 nowSize = NPCScript.transform.localScale;
        float totalTimer = nowSize.x / NPCTalkSize.x * NPCScriptHideTimer;

        while (true)
        {
            if (NPCScript.transform.localScale.x <= 0.00005f) { break; }

            NPCScript.transform.localScale = Vector3.Lerp(nowSize, Vector3.zero, timer / totalTimer);
            yield return null;
        }
        NPCScript.SetActive(false);
    }

    #endregion
}
