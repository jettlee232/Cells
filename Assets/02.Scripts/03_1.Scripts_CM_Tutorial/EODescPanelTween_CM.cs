using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class EODescPanelTween_CM : MonoBehaviour
{
    public GameObject descPanels;

    public TextMeshProUGUI tmpText_Title;
    public TextMeshProUGUI tmpText_Detail;
    public RectTransform targetRectTransform; // ũ�⸦ ������ RectTransform
    public float duration = 1.0f; // �ִϸ��̼� ���� �ð�

    void Start()
    {
        tmpText_Title = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        tmpText_Title.text = "";
        tmpText_Detail = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        tmpText_Detail.text = "";

        targetRectTransform = GetComponent<RectTransform>();
        targetRectTransform.sizeDelta = Vector2.zero;

        DOTween.Init();
    }

    public void PanelOpen(string title, string detail)
    {
        StartCoroutine(ChangeQuestTextAfterFewSec(title, detail));
        targetRectTransform.DOSizeDelta(new Vector2(125, 75), duration);
    }

    IEnumerator ChangeQuestTextAfterFewSec(string title, string detail)
    {
        yield return new WaitForSeconds(1f);
        ChangeText(title, detail);
    }

    public void ChangeText(string title, string detail)
    {
        tmpText_Title.text = title;
        tmpText_Detail.text = detail;
    }


    public void PanelClose()
    {
        tmpText_Title.text = "";
        tmpText_Detail.text = "";
        targetRectTransform.DOSizeDelta(Vector2.zero, duration);
        //StartCoroutine(PanelDisabled());
    }
}
