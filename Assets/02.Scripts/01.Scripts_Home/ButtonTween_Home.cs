using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ButtonTween_Home : MonoBehaviour
{
    RectTransform rectTrns;

    private void Start()
    {
        rectTrns = GetComponent<RectTransform>();
    }


    public void DoButtonTween()
    {
        StartCoroutine(Tweening());
    }

    IEnumerator Tweening()
    {
        while (true)
        {
            rectTrns.DOPunchScale(new Vector3(0.125f, 0.125f, 0.125f), 0.3f);
            yield return new WaitForSeconds(2f);
        }
    }
}
