using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager_Lobby : MonoBehaviour
{
    public static UIManager_Lobby instance;
    public GameObject alert_UI;
    public GameObject Desc_UI;
    public GameObject BlackPanel;
    public GameObject UpsideSubtitle;
    private GameObject lastInteract = null;

    public float fadeInTimer = 1f;
    public float fadeOutTimer = 1f;
    public float UpsideSubtitleVanishTimer = 1f;

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
        StartCoroutine(cStart());
    }

    public void OnClickTitle()
    {
        alert_UI.SetActive(false);
        Desc_UI.SetActive(false);
        GameManager_Lobby.instance.MoveScene("01_Home");
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
    public void OffDesc() { Desc_UI.SetActive(false); }
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

    IEnumerator cStart()
    {
        GameManager_Lobby.instance.StopPlayer();
        yield return cFade(true);
        GameManager_Lobby.instance.EnableMovePlayer();
        SetUpsideSubtitle("Press Button!");
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
}
