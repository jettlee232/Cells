using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPanelTween_CM : MonoBehaviour
{
    private bool madeFlag = false;

    public Vector3 goalSize = new Vector3(1f, 1f, 1f);
    //public Vector3 goalRotate = new Vector3(0f, 0f, 0f);

    void Start()
    {
        DOTween.Init();

        if (madeFlag == false)
        {
            StartInstTween();
            madeFlag = true;
        }                       
    }

    public bool ReturnFlag()
    {
        return madeFlag;
    }

    public void StartInstTween()
    {
        //transform.localRotation = Quaternion.Euler(0, 0, 0);
        transform.localScale = Vector3.zero;
        transform.DOScale(goalSize, 1f);
        //transform.DORotate(goalRotate, 1f, RotateMode.FastBeyond360);
    }

    public void ReverseTweenAndDestroy() // Button으로 호출하는 거
    {
        transform.DOScale(Vector3.zero, 1f);
        //transform.DORotate(new Vector3(0f, 360f, 0f), 1f, RotateMode.FastBeyond360);
        StartCoroutine(DestroyAfterRewind());
    }

    private IEnumerator DestroyAfterRewind()
    {
        yield return new WaitForSeconds(1.05f);

        //Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }
}
