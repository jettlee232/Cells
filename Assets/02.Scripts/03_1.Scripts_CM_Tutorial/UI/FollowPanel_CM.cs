using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FollowPanel_CM : MonoBehaviour
{
    public Button closeBtn;


    void Start()
    {
        DOTween.Init();

<<<<<<< HEAD
        transform.localRotation = Quaternion.Euler(0, 0, 0);
=======
        transform.localRotation = Quaternion.Euler(0,0,0);
>>>>>>> Dev
        transform.localScale = Vector3.zero;
        transform.DOScale(new Vector3(1f, 1f, 1f), 1f);
        Debug.Log(gameObject.name + "s Parent : " + transform.parent.name + " Rot is... " + transform.parent.GetComponent<RectTransform>().localEulerAngles.y);
        transform.DORotate(new Vector3(0f, 360f + transform.parent.GetComponent<RectTransform>().localEulerAngles.y, 0f), 1f, RotateMode.FastBeyond360);

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
        yield return new WaitForSeconds(1.05f);

        Destroy(this.gameObject);
    }
}
