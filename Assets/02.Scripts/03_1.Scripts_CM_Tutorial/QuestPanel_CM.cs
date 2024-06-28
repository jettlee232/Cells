using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using DG.Tweening;

public class QuestPanel_CM : MonoBehaviour
{
    public TextMeshProUGUI tmpText;
    public RectTransform targetRectTransform;
    public WordEffect1 wordEffect;
    private float duration = 2.0f;

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
        wordEffect.enabled = false;

        DOTween.Init();
    }

    public void PanelOpen(string newQuest)
    {
        tmpText.text = "";
        StartCoroutine(ChangeQuestTextAfterFewSec(newQuest));
        targetRectTransform.DOSizeDelta(new Vector2(530, 100), duration);
    }

    IEnumerator ChangeQuestTextAfterFewSec(string newQuest)
    {
        yield return new WaitForSeconds(1f);
        ChangeText(newQuest);
    }

    public void ChangeText(string newQuest)
    {
        wordEffect.enabled = true;
        tmpText.text = newQuest;
    }

    public void PanelClose()
    {
        wordEffect.enabled = false;
        tmpText.text = "";
        targetRectTransform.DOSizeDelta(Vector2.zero, duration);
        StartCoroutine(PanelDisabled());
    }

    IEnumerator PanelDisabled()
    {
        yield return new WaitForSeconds(duration + 0.5f);
        gameObject.SetActive(false);
    }


    // 능동적 UI
    public void PanelTween(GameObject go) // Experimental
    {
        Transform goPos = go.transform;
        StartCoroutine(PanelMoveAndSetPos(goPos));
    }

    IEnumerator PanelMoveAndSetPos(Transform goPos) // Experimental
    {
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
