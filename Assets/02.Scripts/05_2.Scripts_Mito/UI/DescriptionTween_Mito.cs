using DG.Tweening;
using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionTween_Mito : MonoBehaviour
{
    public Button closeBtn;
    public GameObject hlObj;

    void Start()
    {
        DOTween.Init();

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        transform.localScale = Vector3.zero;
        transform.DOScale(new Vector3(1f, 1f, 1f), 1f);
        transform.DORotate(new Vector3(0f, 360f + transform.GetComponent<RectTransform>().localEulerAngles.y, 0f), 1f, RotateMode.FastBeyond360);
        //transform.DORotate(new Vector3(0f, 360f + transform.parent.GetComponent<RectTransform>().localEulerAngles.y, 0f), 1f, RotateMode.FastBeyond360).OnComplete(() => StartCoroutine(LookPlayer()));

        if (closeBtn == null)
        {
            GameObject go = transform.Find("List").Find("Button_Close").gameObject;
            go.AddComponent<Button>();
            closeBtn = go.GetComponent<Button>();
        }
        closeBtn.onClick.AddListener(ReverseTweenAndDestroy);
    }

    public void ReverseTweenAndDestroy() // Button으로 호출하는 거
    {
        transform.DOScale(Vector3.zero, 1f);
        transform.DORotate(new Vector3(0f, 360f, 0f), 1f, RotateMode.FastBeyond360);
        StartCoroutine(DestroyAfterRewind());
    }

    private IEnumerator DestroyAfterRewind()
    {
        hlObj.GetComponent<HighLightColorchange_MitoTuto>().GlowEnd();

        yield return new WaitForSeconds(1.05f);

        Destroy(this.gameObject);
    }

    public void HLObjInit(GameObject go)
    {
        hlObj = go;

        HighLightColorchange_MitoTuto highlightColorChng = hlObj.GetComponent<HighLightColorchange_MitoTuto>();

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
