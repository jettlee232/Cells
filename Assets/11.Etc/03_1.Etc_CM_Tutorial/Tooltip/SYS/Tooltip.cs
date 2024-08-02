using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public LineRenderer line;

    public TextMeshProUGUI txt;
    public RectTransform panelRectTrns;
    public Transform playerTrns;

    public Vector3 goalScale;
    Vector3 directionToPlayer;
    Vector3 oppositeDirection;
    Quaternion lookRotation;

    public GameObject particle; // SYS Code - Update Date : 240724

    public GameObject[] tutoAnims; // SYS Code - Update Date : 240724

    public Image imgSprite;
    public Sprite[] sprites;

    void Start()
    {
        if (playerTrns == null) playerTrns = GameObject.FindGameObjectWithTag("MainCamera").transform;

        DOTween.Init();
    }

    public void TooltipOn(string content)
    {
        if(particle != null)
        {
            particle.SetActive(true); // SYS Code - Update Date : 240724
            particle.GetComponent<ParticleSystem>().Play();
        }
            

        txt.text = content;

        line.startWidth = 0f;
        DOTween.To(() => line.startWidth, x => line.startWidth = x, 0.005f, 1f);
        DOTween.To(() => line.endWidth, x => line.endWidth = x, 0f, 1f);

        panelRectTrns.localRotation = Quaternion.Euler(0, 0, 0);
        panelRectTrns.localScale = Vector3.zero;
        panelRectTrns.DOScale(goalScale, 1f);
    }

    public void TooltipOff()
    {
        DOTween.To(() => line.startWidth, x => line.startWidth = x, 0f, 1f);
        DOTween.To(() => line.endWidth, x => line.endWidth = x, 0f, 1f);
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
        /*
        directionToPlayer = playerTrns.position;
        lookRotation = Quaternion.LookRotation(directionToPlayer);

        panelRectTrns.rotation = lookRotation;
        */

        
        directionToPlayer = playerTrns.position - transform.position;
        oppositeDirection = -directionToPlayer;
        lookRotation = Quaternion.LookRotation(oppositeDirection);

        panelRectTrns.rotation = lookRotation;        
    }

    void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }

    public void UnShowingTooltipAnims()
    {
        for (int i = 0; i < tutoAnims.Length; i++)
        {
            tutoAnims[i].SetActive(false);
        }
    }

    public void ShowingTooltipAnims(int index)
    {
        UnShowingTooltipAnims();
        tutoAnims[index].SetActive(true);
    }

    public void ChangeSprite(int index)
    {
        imgSprite.sprite = sprites[index];
    }
}