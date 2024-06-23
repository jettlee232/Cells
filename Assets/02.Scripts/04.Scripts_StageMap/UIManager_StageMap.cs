using BNG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static UnityEngine.GraphicsBuffer;

public class UIManager_StageMap : MonoBehaviour
{
    public static UIManager_StageMap instance;
    public GameObject Desc_UI;
    public GameObject UpsideSubtitle;

    public float UpsideSubtitleVanishTimer = 1f;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
    }

    void Start()
    {
        Desc_UI.SetActive(false);
    }

    #region 소기관 설명창
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
    public void EnableButton() { Desc_UI.transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Button>().interactable = true; }
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

    public void OnClickTitle()
    {
        OffDesc();
        GameManager_StageMap.instance.MoveScene("01_Home");
    }
}
