using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class EODescPanelTween_CM : MonoBehaviour
{
    public TextMeshProUGUI tmpText_Title;
    public TextMeshProUGUI tmpText_Detail;
    public RectTransform targetRectTransform;
    public float duration = 1.0f; // �ִϸ��̼� ���� �ð�

    private Vector2 initVec;

    void Start()
    {
        if (tmpText_Title == null) tmpText_Title = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        tmpText_Title.text = "";

        if (tmpText_Detail == null) tmpText_Detail = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        tmpText_Detail.text = "";

        if (targetRectTransform == null) targetRectTransform = GetComponent<RectTransform>();
        //initVec = targetRectTransform.sizeDelta;
        targetRectTransform.sizeDelta = Vector3.zero;

        DOTween.Init();
    }

    public void PanelOpen(string title, string detail)
    {
        StartCoroutine(ChangeQuestTextAfterFewSec(title, detail));
        targetRectTransform.DOSizeDelta(new Vector2(125, 75), duration);
        //targetRectTransform.DOSizeDelta(initVec, duration);
    }

    IEnumerator ChangeQuestTextAfterFewSec(string title, string detail)
    {
        yield return new WaitForSeconds(1f);
        ChangeText(title, detail);
    }

    public void ChangeText(string title, string detail)
    {
        tmpText_Title.text = title;
        //tmpText_Detail.text = detail;
    }


    public void PanelClose()
    {
        tmpText_Title.text = "";
        tmpText_Detail.text = "";
        targetRectTransform.DOSizeDelta(Vector2.zero, duration);
        //StartCoroutine(PanelDisabled());
    }
}
