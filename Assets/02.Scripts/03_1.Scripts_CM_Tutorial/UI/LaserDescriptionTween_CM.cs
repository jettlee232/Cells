using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using HighlightPlus;

public class LaserDescriptionTween_CM : MonoBehaviour
{
    public Button closeBtn;
    public GameObject hlObj;


    [Header("LPAD")] // Latley Update - 240701 pm 0118
    public LPAD_CM lpad; // Latley Update - 240701 pm 0118

    void Start()
    {
        DOTween.Init();

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        transform.localScale = Vector3.zero;
        transform.DOScale(new Vector3(1f, 1f, 1f), 1f);
        transform.DORotate(new Vector3(0f, 360f + transform.parent.GetComponent<RectTransform>().localEulerAngles.y, 0f), 1f, RotateMode.FastBeyond360);
        //transform.DORotate(new Vector3(0f, 360f + transform.parent.GetComponent<RectTransform>().localEulerAngles.y, 0f), 1f, RotateMode.FastBeyond360).OnComplete(() => StartCoroutine(LookPlayer()));

        if (closeBtn == null)
        {
            GameObject go = transform.Find("List").Find("Button_Close").gameObject;
            go.AddComponent<Button>();
            closeBtn = go.GetComponent<Button>();
        }
        closeBtn.onClick.AddListener(ReverseTweenAndDestroy);
        Debug.Log(closeBtn.onClick);

        lpad = GameObject.Find("RightHandPointer").GetComponent<LPAD_CM>();
    }

    public void ReverseTweenAndDestroy()
    {
        lpad.currentPanel = null; // Latley Update - 240701 pm 0118

        transform.DOScale(Vector3.zero, 1f);
        transform.DORotate(new Vector3(0f, 360f, 0f), 1f, RotateMode.FastBeyond360);
        StartCoroutine(DestroyAfterRewind());
    }

    private IEnumerator DestroyAfterRewind()
    {
        if (hlObj != null) hlObj.GetComponent<HighLightColorchange_CM>().GlowEnd(); // Update - 240701 am 0204

        yield return new WaitForSeconds(1.05f);

        Destroy(this.gameObject);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<TutorialManager_CM>().CheckHighlightCount();
    }

    public void HLObjInit(GameObject go)
    {
        hlObj = go;

        HighLightColorchange_CM highlightColorChng = hlObj.GetComponent<HighLightColorchange_CM>();

        HighlightEffect highlightEffect = hlObj.GetComponent<HighlightEffect>();
        if (highlightEffect != null && highlightEffect.highlighted == false)
        {
            highlightColorChng.GlowStart();
        }
    }

    IEnumerator LookPlayer() // Experimental
    {
        Transform playerCam = GameObject.FindGameObjectWithTag("MainCamera").transform;

        if (playerCam == null) playerCam = transform.parent;

        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            Vector3 directionToPlayer = playerCam.position - transform.position;
            Vector3 oppositeDirection = -directionToPlayer;
            Quaternion lookRotation = Quaternion.LookRotation(oppositeDirection);

            transform.rotation = lookRotation;
        }
    }
}
