using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InstantiateTween2_CM : MonoBehaviour
{

    void Start()
    {
        DOTween.Init();        
    }

    public void DoingTween()
    {
        transform.localScale = Vector3.zero;

        transform.DOScale(new Vector3(1f, 1f, 1f), 1f);
        transform.DORotate(new Vector3(0f, 360f, 0f), 1f, RotateMode.FastBeyond360);
    }

    public void ReverseTweenAndDestroy() // Button���� ȣ���ϴ� ��
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
