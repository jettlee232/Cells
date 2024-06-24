using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class QuestPanel_CM : MonoBehaviour
{
    public TextMeshProUGUI tmpText;
    public RectTransform targetRectTransform; // 크기를 변경할 RectTransform
    public WordEffect1 wordEffect;
    public float duration = 1.0f; // 애니메이션 지속 시간

    void Start()
    {
        wordEffect.enabled = false;
        tmpText = gameObject.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        tmpText.text = "";

        targetRectTransform = GetComponent<RectTransform>();
        targetRectTransform.sizeDelta = Vector2.zero;
        
        DOTween.Init();
    }

    public void PanelOpen(string newQuest)
    {        
        StartCoroutine(ChangeQuestTextAfterFewSec(newQuest));
        targetRectTransform.DOSizeDelta(new Vector2(700, 250), duration);
    }

    IEnumerator ChangeQuestTextAfterFewSec(string newQuest)
    {
        yield return new WaitForSeconds(1f);
        ChangeText(newQuest);
    }

    public void ChangeText(string newQuest)
    {
        wordEffect.enabled = true;
        tmpText.text = newQuest;
    }


    public void PanelClose()
    {
        wordEffect.enabled = false;
        tmpText.text = "";
        targetRectTransform.DOSizeDelta(Vector2.zero, duration);
        //StartCoroutine(PanelDisabled());
    }

    IEnumerator PanelDisabled()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
