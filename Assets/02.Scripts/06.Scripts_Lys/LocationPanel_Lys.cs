using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class LocationPanel_Lys : MonoBehaviour
{
    public TextMeshProUGUI tmpText;
    public RectTransform targetRectTransform1;
    public RectTransform targetRectTransform2;
    public RectTransform targetRectTransform3;
    public WordEffect1 wordEffect;
    public string name;
    private float duration = 2.0f;

    void Start()
    {
        if (wordEffect == null) wordEffect = gameObject.transform.GetChild(2).GetComponent<WordEffect1>();
        wordEffect.enabled = false;

        if (tmpText == null) tmpText = gameObject.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        tmpText.text = "";

        if (targetRectTransform1 == null) targetRectTransform1 = GetComponent<RectTransform>();
        targetRectTransform1.sizeDelta = Vector2.zero;

        if (targetRectTransform2 == null) targetRectTransform2 = transform.GetChild(3).GetComponent<RectTransform>();
        targetRectTransform2.sizeDelta = Vector2.zero;

        if (targetRectTransform3 == null) targetRectTransform2 = transform.GetChild(3).GetChild(0).GetComponent<RectTransform>();
        targetRectTransform3.sizeDelta = Vector2.zero;

        DOTween.Init();
        PanelOpen(name);
    }

    public void PanelOpen(string newLoc)
    {
        tmpText.text = "";
        StartCoroutine(ChangeQuestTextAfterFewSec(newLoc));
        targetRectTransform1.DOSizeDelta(new Vector2(420, 100), duration);
        targetRectTransform2.DOSizeDelta(new Vector2(80, 80), duration);
        targetRectTransform3.DOSizeDelta(new Vector2(33, 40), duration);
    }

    IEnumerator ChangeQuestTextAfterFewSec(string newLoc)
    {
        yield return new WaitForSeconds(1f);
        ChangeText(newLoc);
        yield return new WaitForSeconds(3f);
        PanelClose();
    }

    public void ChangeText(string newLoc)
    {
        wordEffect.enabled = true;
        tmpText.text = newLoc;
    }

    public void PanelClose()
    {
        wordEffect.enabled = false;
        tmpText.text = "";
        targetRectTransform1.DOSizeDelta(Vector2.zero, duration);
        targetRectTransform2.DOSizeDelta(Vector2.zero, duration - 1f);
        targetRectTransform3.DOSizeDelta(Vector2.zero, duration - 1f);
        StartCoroutine(PanelDisabled());
    }

    IEnumerator PanelDisabled()
    {
        yield return new WaitForSeconds(duration + 1f);
        gameObject.SetActive(false);
    }
}
