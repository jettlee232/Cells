using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using HighlightPlus;
using MoreMountains.Feedbacks;

public class LaserDescriptionTween_Home : MonoBehaviour
{
    // 원래는 뒤에가 _Lobby여야 하지만 실수로 _Home이라고 적어버림

    public Button closeBtn;
    public GameObject hlObj;

    [Header("LaserPointer_Lobby")] // Latley Update - 240701 pm 0118
    public LaserPointer_Lobby lpLobby; // Latley Update - 240726    

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
        //transform.DORotate(new Vector3(0f, 360f + transform.parent.GetComponent<RectTransform>().eulerAngles.y, 0f), 1f, RotateMode.FastBeyond360);
        //transform.DORotate(new Vector3(0f, 360f + transform.parent.GetComponent<RectTransform>().localEulerAngles.y, 0f), 1f, RotateMode.FastBeyond360).OnComplete(() => StartCoroutine(LookPlayer()));

        if (closeBtn == null)
        {
            GameObject go = transform.Find("List").Find("Button_Close").gameObject;
            go.AddComponent<Button>();
            closeBtn = go.GetComponent<Button>();
        }
        closeBtn.onClick.AddListener(ReverseTweenAndDestroy);
        Debug.Log(closeBtn.onClick);

        // Latley Update - 240726
        lpLobby = GameObject.FindGameObjectWithTag("Inventory").GetComponent<LaserPointer_Lobby>();
        // Temporary Tagging = Inventory

        AudioMgr_CM.Instance.PlaySFXByInt(4); // SSS
    }

    public void ReverseTweenAndDestroy()
    {
        if (lpLobby != null) lpLobby.currentPanel = null; // Latley Update - 240701 pm 0118 & 240726
        lpLobby.watchParticle2.Stop();

        transform.DOScale(Vector3.zero, 1f);
        transform.DOLocalRotate(new Vector3(0f, 360f, 0f), 1f, RotateMode.FastBeyond360);
        StartCoroutine(DestroyAfterRewind()); // 나중에 삭제해야 됨

        AudioMgr_CM.Instance.PlaySFXByInt(16); // SSS
    }

    private IEnumerator DestroyAfterRewind()
    {
        if (hlObj != null) hlObj.GetComponent<HighLightColorchange_CM>().GlowEnd(); // Update - 240701 am 0204

        yield return new WaitForSeconds(1.05f);

        Destroy(this.gameObject);
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

    void ActivateFeedback1()
    {
        feedback1?.PlayFeedbacks();
    }
}
