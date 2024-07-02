using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class SpeechBubblePanel_CM : MonoBehaviour
{
    public TextMeshProUGUI tmpText;
    public RectTransform targetRectTransform1;
    public RectTransform targetRectTransform2;
    public WordEffect1 wordEffect;
    private float duration = 2.0f;

    [Header("시작부터 띄울지 말지 정하는 Flag")]
    public bool activatedInStart = false;
    public string textContent = "";

    void Start()
    {
        if (wordEffect == null) wordEffect = gameObject.transform.GetChild(2).GetComponent<WordEffect1>();
        wordEffect.enabled = false;

        if (tmpText == null) tmpText = gameObject.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        tmpText.text = "";

        if (targetRectTransform1 == null) targetRectTransform1 = GetComponent<RectTransform>();
        targetRectTransform1.sizeDelta = Vector2.zero;

        if (targetRectTransform2 == null) targetRectTransform2 = GetComponent<RectTransform>();
        targetRectTransform2.sizeDelta = Vector2.zero;

        DOTween.Init();

        if (activatedInStart == true)
        {
            // 띄우고 계속 존재하는거
            PanelOpen(textContent);

            // 띄우고 3초 후 사라지는 거
            //PanelOpen(textContent, 3f);
        }
    }

    public void PanelOpen(string newSpeech)
    {
        tmpText.text = "";
        StartCoroutine(ChangeQuestTextAfterFewSec(newSpeech));
        targetRectTransform1.DOSizeDelta(new Vector2(570, 100), duration);
        targetRectTransform2.DOSizeDelta(new Vector2(24, 12), duration);
    }

    public void PanelOpen(string newSpeech, float exitTime)
    {
        tmpText.text = "";
        StartCoroutine(ChangeQuestTextAfterFewSec(newSpeech));
        targetRectTransform1.DOSizeDelta(new Vector2(570, 100), duration);
        targetRectTransform2.DOSizeDelta(new Vector2(24, 12), duration);
        Invoke("PanelClose", exitTime);
    }

    IEnumerator ChangeQuestTextAfterFewSec(string newSpeech)
    {
        yield return new WaitForSeconds(1f);
        ChangeText(newSpeech);
    }

    public void ChangeText(string newSpeech)
    {
        wordEffect.enabled = true;
        tmpText.text = newSpeech;
    }

    public void PanelClose()
    {
        wordEffect.enabled = false;
        tmpText.text = "";
        targetRectTransform1.DOSizeDelta(Vector2.zero, duration);
        targetRectTransform2.DOSizeDelta(Vector2.zero, duration - 1f);
        StartCoroutine(PanelDisabled());
    }

    IEnumerator PanelDisabled()
    {
        yield return new WaitForSeconds(duration + 0.5f);
        gameObject.SetActive(false);
    }
}
