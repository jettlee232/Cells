using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip_CM : MonoBehaviour
{
    public TextMeshProUGUI txt;
    public RectTransform panelRectTrns;
    public Transform playerTrns;

    public Vector3 goalScale;
    Vector3 directionToPlayer;
    Vector3 oppositeDirection;
    Quaternion lookRotation;

    void Start()
    {
        if (panelRectTrns == null) panelRectTrns = GetComponent<RectTransform>();
        if (playerTrns == null) playerTrns = GameObject.FindGameObjectWithTag("MainCamera").transform;        

        DOTween.Init();
    }

    public void TooltipOn(string content)
    {
        Debug.Log(content);
        txt.text = content;

        panelRectTrns.localRotation = Quaternion.Euler(0, 0, 0);
        panelRectTrns.localScale = Vector3.zero;
        panelRectTrns.DOScale(goalScale, 1f);
    }

    public void TooltipOff()
    {
        panelRectTrns.DOScale(new Vector3(0f, 0f, 0f), 1f).OnComplete(SetActiveFalse);
    }

    public void TooltipTextChange(string newContent)
    {
        txt.text = newContent;
    }

    void Update()
    {
        LookingPlayer();
    }

    void LookingPlayer()
    {
        directionToPlayer = playerTrns.position - transform.position;
        oppositeDirection = -directionToPlayer;
        lookRotation = Quaternion.LookRotation(oppositeDirection);

        panelRectTrns.rotation = lookRotation;
    }

    void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
