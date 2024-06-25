using BNG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;

public class UIManager_StageMap : MonoBehaviour
{
    [Header("Settings")]
    public static UIManager_StageMap instance;
    public GameObject Desc_UI;
    public GameObject UpsideSubtitle;
    public GameObject NPCTalkPanel;

    [Header("Variables")]
    public float UpsideSubtitleVanishTimer = 1f;
    public float DescUITimer = 0.2f;
    private Vector3 DescUIFullSize;


    void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
    }

    void Start()
    {
        Desc_UI.SetActive(false);
        DescUIFullSize = Desc_UI.GetComponent<RectTransform>().localScale;
    }

    #region 소기관 설명창
    public GameObject GetDesc() { return Desc_UI; }

    public void OnDesc(GameObject go)
    {
        if (CheckDesc())
        {
            Desc_UI.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = go.GetComponent<DescObj_StageMap>().GetName();
            Desc_UI.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = go.GetComponent<DescObj_StageMap>().GetDesc();
        }
        else { StartCoroutine(ShowDesc(go)); }
    }
    public void OffDesc() { StartCoroutine(VanishDesc()); }
    IEnumerator ShowDesc(GameObject go)
    {
        Desc_UI.SetActive(true);

        Desc_UI.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = go.GetComponent<DescObj_StageMap>().GetName();
        Desc_UI.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = go.GetComponent<DescObj_StageMap>().GetDesc();

        Vector3 nowDescSize = Desc_UI.GetComponent<RectTransform>().localScale;

        float timer = 0f;
        float totalTimer = (DescUIFullSize.x - nowDescSize.x) / DescUIFullSize.x * DescUITimer;
        while (true)
        {
            if (DescUIFullSize.x - Desc_UI.GetComponent<RectTransform>().localScale.x <= 0.0000001f) { break; }
            Desc_UI.GetComponent<RectTransform>().localScale = Vector3.Lerp(nowDescSize, DescUIFullSize, timer / totalTimer);
            timer += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator VanishDesc()
    {
        Vector3 nowDescSize = Desc_UI.GetComponent<RectTransform>().localScale;

        float timer = 0f;
        float totalTimer = nowDescSize.x / DescUIFullSize.x * DescUITimer;
        while (true)
        {
            if (Desc_UI.GetComponent<RectTransform>().localScale.x <= 0.0000001f) { break; }
            Desc_UI.GetComponent<RectTransform>().localScale = Vector3.Lerp(nowDescSize, Vector3.zero, timer / totalTimer);
            timer += Time.deltaTime;
            yield return null;
        }

        Desc_UI.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        Desc_UI.SetActive(false);
    }
    public bool CheckDesc() { return Desc_UI.activeSelf; }
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

    public void OnClickTitle()
    {
        OffDesc();
        GameManager_StageMap.instance.MoveScene("01_Home");
    }
}
