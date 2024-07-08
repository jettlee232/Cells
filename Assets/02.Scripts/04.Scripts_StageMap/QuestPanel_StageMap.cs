using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class QuestPanel_StageMap : MonoBehaviour
{
    public TextMeshProUGUI tmpText;
    public RectTransform targetRectTransform;
    public WordEffect1 wordEffect;
    private float duration = 2.0f;

    private Coroutine corou;

    // 능동적 UI 
    public Transform playerCam; // Experimental
    private bool lookPlayerFlag = false; // Experimental

    void Start()
    {
        if (tmpText == null) tmpText = gameObject.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        tmpText.text = "";

        if (targetRectTransform == null) targetRectTransform = GetComponent<RectTransform>();
        targetRectTransform.sizeDelta = Vector2.zero;

        if (wordEffect == null) wordEffect = gameObject.transform.GetChild(0).GetChild(0).GetComponent<WordEffect1>();
        wordEffect.enabled = true;

        DOTween.Init();
    }

    public void PanelOpen(string newQuest)
    {
        if (corou != null)
        {
            StopCoroutine(corou);
            corou = null;
        }

        gameObject.SetActive(true);
        ChangeText(newQuest);
        wordEffect.enabled = true;
        tmpText.transform.GetComponent<RectTransform>().localScale = new Vector3(0.01f, 0.01f, 0.01f);
        tmpText.transform.GetComponent<RectTransform>().DOScale(new Vector3(1, 1, 1), duration);
        targetRectTransform.DOSizeDelta(new Vector2(530, 100), duration);
    }

    public void PanelOpen(string newQuest, float exitTime) // 추가, 수정한 부분
    {
        if (corou != null)
        {
            StopCoroutine(corou);
            corou = null;
        }

        gameObject.SetActive(true);
        ChangeText(newQuest);
        wordEffect.enabled = true;
        tmpText.transform.GetComponent<RectTransform>().localScale = new Vector3(0.01f, 0.01f, 0.01f);
        tmpText.transform.GetComponent<RectTransform>().DOScale(new Vector3(1, 1, 1), duration);
        targetRectTransform.DOSizeDelta(new Vector2(530, 100), duration);

        Invoke("PanelClose", exitTime);
    }

    public void ChangeText(string newQuest)
    {
        tmpText.text = newQuest;
    }

    public void PanelClose()
    {
        if (corou != null) StopCoroutine(corou);

        corou = StartCoroutine(PanelDisabled());
    }

    IEnumerator PanelDisabled()
    {
        if (!this.gameObject.activeSelf) { yield break; }

        wordEffect.enabled = false;
        tmpText.transform.GetComponent<RectTransform>().DOScale(Vector3.zero, duration);
        targetRectTransform.DOSizeDelta(Vector2.zero, duration);
        yield return new WaitForSeconds(duration);
        tmpText.text = "";
        gameObject.SetActive(false);

        corou = null;
    }


    // 능동적 UI
    public void PanelTween(GameObject go) // Experimental
    {
        Transform goPos = go.transform;
        StartCoroutine(PanelMoveAndSetPos(goPos));
    }

    IEnumerator PanelMoveAndSetPos(Transform goPos) // Experimental
    {
        if (!this.gameObject.activeSelf) { yield break; }

        yield return new WaitForSeconds(3f);
        Vector3 pevPos = transform.position;
        transform.SetParent(null);
        targetRectTransform.position = pevPos;

        Vector3 newPos = new Vector3(goPos.position.x, goPos.position.y + 0.5f, goPos.position.z);

        transform.DOMove(newPos, 0.5f);

        lookPlayerFlag = true;
        StartCoroutine(LookPlayer());

        yield return new WaitForSeconds(2f);

    }

    IEnumerator LookPlayer() // Experimental
    {
        if (!this.gameObject.activeSelf) { yield break; }

        if (playerCam == null) playerCam = transform.parent;

        while (lookPlayerFlag)
        {
            yield return new WaitForSeconds(0.1f);
            Vector3 directionToPlayer = playerCam.position - transform.position;
            Vector3 oppositeDirection = -directionToPlayer;
            Quaternion lookRotation = Quaternion.LookRotation(oppositeDirection);

            transform.rotation = lookRotation;
        }
    }

    public void ChangePlayerLookFlag(bool flag) // Experimental
    {
        lookPlayerFlag = flag;
    }

    public void SetPanelInit() // Experimental
    {
        ChangePlayerLookFlag(false);

        transform.SetParent(GameObject.FindGameObjectWithTag("MainCamera").transform);
        targetRectTransform.localPosition = new Vector3(0, -0.65f, 3f);
        targetRectTransform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}
