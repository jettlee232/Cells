using DG.Tweening;
using MoreMountains.Feedbacks;
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

    // ReSize
    public bool isThisHandTooltip = false;

    private RectTransform canvasSize;
    private Vector3 canvasSize_goal;
    private Vector3 canvasSize_init;

    private RectTransform fontPos;
    private Vector3 fontPos_goal;
    private Vector3 fontPos_init;

    // AlwaysOnTop
    [Header("Always On Top")]
    public bool isThisHandtoolTip = false;
    public Transform pointA;
    public Transform pointB;
    public Transform handTransform;
    private float overHandPos = 0.175f;
    public bool leftOrRight = false;
    private Quaternion yAxisRotation;

    // Feel
    [Header("FEEL")]
    public MMF_Player feedback;    

    void Start()
    {
        if (playerTrns == null) playerTrns = GameObject.FindGameObjectWithTag("MainCamera").transform;

        DOTween.Init();

        // ReSize
        if (isThisHandTooltip)
        {
            canvasSize = transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<RectTransform>();
            fontPos = transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetComponent<RectTransform>();

            canvasSize_init = canvasSize.localScale;
            canvasSize_goal = new Vector3(0.75f, 0.75f, 0.75f);

            fontPos_init = fontPos.localPosition;
            fontPos_goal = Vector3.zero;
        }

        AudioMgr_CM.Instance.PlaySFXByInt(4);

        if (leftOrRight == false) yAxisRotation = Quaternion.Euler(0f, 0f, -2.5f);
        else yAxisRotation = Quaternion.Euler(0f, 0f, 2.5f);    
    }

    public void TooltipOn(string content)
    {
        if (particle != null)
        {
            particle.SetActive(true); // SYS Code - Update Date : 240724
            particle.GetComponent<ParticleSystem>().Play();
        }
            

        txt.text = content;

        //line.startWidth = 0f;
        //DOTween.To(() => line.startWidth, x => line.startWidth = x, 0.005f, 1f);
        //DOTween.To(() => line.endWidth, x => line.endWidth = x, 0f, 1f);

        panelRectTrns.localRotation = Quaternion.Euler(0, 0, 0);
        panelRectTrns.localScale = Vector3.zero;
        panelRectTrns.DOScale(goalScale, 1f);

        feedback?.PlayFeedbacks();
    }

    public void TooltipOff()
    {
        //DOTween.To(() => line.startWidth, x => line.startWidth = x, 0f, 1f);
        //DOTween.To(() => line.endWidth, x => line.endWidth = x, 0f, 1f);
        panelRectTrns.DOScale(new Vector3(0f, 0f, 0f), 1f).OnComplete(SetActiveFalse);

        feedback?.PlayFeedbacks();
    }

    public void TooltipTextChange(string newContent)
    {
        feedback?.PlayFeedbacks();

        txt.text = newContent;
    }

    void LateUpdate()
    {
        if (isThisHandtoolTip) 
        { 
            AlwaysOnTop(); 
            LookingPlayer2(); 
        }
        else LookingPlayer();        
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

    void LookingPlayer2()
    {
        directionToPlayer = playerTrns.position - pointB.position;
        oppositeDirection = -directionToPlayer;
        lookRotation = Quaternion.LookRotation(oppositeDirection);

        pointB.rotation = lookRotation * yAxisRotation;
    }

    void AlwaysOnTop()
    {
        pointA.transform.position = handTransform.position;

        pointB.transform.position = new Vector3(handTransform.position.x, handTransform.position.y + overHandPos,
            handTransform.position.z);
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
        //imgSprite.sprite = sprites[index];
        //ReSizeCanvas(index); // ReSize
    }

    // ReSize
    void ReSizeCanvas(int index)
    {
        if (index == 0)
        {
            canvasSize.localScale = canvasSize_init;
            fontPos.localPosition = fontPos_init;
        }
        else
        {
            canvasSize.localScale = canvasSize_goal;
            fontPos.localPosition = fontPos_goal;
        }
    }
}