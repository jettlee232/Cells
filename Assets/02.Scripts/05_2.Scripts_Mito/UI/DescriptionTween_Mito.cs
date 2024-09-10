using DG.Tweening;
using HighlightPlus;
using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionTween_Mito : MonoBehaviour
{
    public Button closeBtn;
    public GameObject hlObj;

    public RayDescription_MitoTuto rayDesc;
    public Coroutine lookPlayer;

    [Header("Rotation")]
    public Vector3 first;
    public Vector3 later = new Vector3(0f, 360f, 0f);

    [Header("Feedback")]
    public MMF_Player feedback1;


    void Start()
    {
        DOTween.Init();

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(first);
        transform.localScale = Vector3.zero;
        transform.DOScale(new Vector3(.5f, .5f, .5f), 1f);
        transform.DOLocalRotate(later, 1f, RotateMode.FastBeyond360).OnComplete(ActivateFeedback1);

        /*
        DOTween.Init();

        Quaternion parentRotation = transform.parent ? transform.parent.localRotation : Quaternion.identity;

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        transform.localScale = Vector3.zero;
        transform.DOScale(new Vector3(1f, 1f, 1f), 1f);
        */


        //transform.DORotate(new Vector3(0f, 360f + transform.GetComponent<RectTransform>().localEulerAngles.y, 0f), 1f, RotateMode.FastBeyond360);
        //transform.DORotate(new Vector3(0f, 360f + transform.parent.GetComponent<RectTransform>().localEulerAngles.y, 0f), 1f, RotateMode.FastBeyond360).OnComplete(()
        //    => lookPlayer = StartCoroutine(LookPlayer()));

        //transform.DORotate(new Vector3(0f, 360f, 0f), 1f, RotateMode.FastBeyond360).OnComplete(() =>
        //{
        //    transform.localRotation = Quaternion.Euler(0, 0, 0);
        //    lookPlayer = StartCoroutine(LookPlayer());
        //});

        /*
        Transform playerCam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        if (playerCam == null) playerCam = transform.parent;

        Vector3 directionToPlayer = playerCam.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(-directionToPlayer);

        transform.DORotate(new Vector3(0f, 360f + lookRotation.eulerAngles.y, 0f), 1f, RotateMode.FastBeyond360).OnComplete(() =>
        {
            lookPlayer = StartCoroutine(LookPlayer());
        });
        */

        // 한 바퀴 돌고 부모의 각도만큼 더 돌고 정확히 원래 각도로 돌아가기
        /*
        transform.DORotate(new Vector3(0f, 360f, 0f), 1f, RotateMode.FastBeyond360).OnComplete(() =>
        {
            transform.localRotation = parentRotation; // 부모의 로컬 회전 각도로 설정
        });
        */

        if (closeBtn == null)
        {
            GameObject go = transform.Find("List").Find("Button_Close").gameObject;
            go.AddComponent<Button>();
            closeBtn = go.GetComponent<Button>();
        }
        closeBtn.onClick.AddListener(ReverseTweenAndDestroy);

        rayDesc = GameObject.Find("RightHandPointer").GetComponent<RayDescription_MitoTuto>();

        AudioMgr_CM.Instance.PlaySFXByInt(4); // SSS
    }

    public void ReverseTweenAndDestroy() // Button으로 호출하는 거
    {
        //StopCoroutine(lookPlayer);
        if (rayDesc != null) rayDesc.currentPanel = null; // Latley Update - 240701 pm 0118 & 240726
        rayDesc.watchParticle2.Stop();

        //rayDesc.currentPanel = null;

        transform.DOScale(Vector3.zero, 1f);
        transform.DOLocalRotate(new Vector3(0f, 360f, 0f), 1f, RotateMode.FastBeyond360);
        StartCoroutine(DestroyAfterRewind());

        AudioMgr_CM.Instance.PlaySFXByInt(16); // SSS
    }

    private IEnumerator DestroyAfterRewind()
    {
        if (hlObj != null)
            hlObj.GetComponent<HighLightColorchange_MitoTuto>().GlowEnd();

        yield return new WaitForSeconds(1.05f);

        Destroy(this.gameObject);
        //Destroy(transform.parent.gameObject);
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
            yield return null;
            //yield return new WaitForSeconds(0.1f);
            Vector3 directionToPlayer = playerCam.position - transform.position;
            Vector3 oppositeDirection = -directionToPlayer;
            Quaternion lookRotation = Quaternion.LookRotation(oppositeDirection);

            transform.rotation = lookRotation;
            //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    void ActivateFeedback1()
    {
        feedback1?.PlayFeedbacks();
    }
}
