using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTween_Lobby : MonoBehaviour
{
    RectTransform rectTrns;
    public Vector3 m_scale;
    public float m_duration;

    Vector3 InitScale;

    void Start()
    {
        rectTrns = GetComponent<RectTransform>();
        InitScale = rectTrns.localScale;
        DoButtonTween();
    }

    public void DoButtonTween()
    {
        StartCoroutine(Tweening());
    }

    IEnumerator Tweening()
    {
        while (true)
        {
            //rectTrns.DOPunchScale(m_scale, m_duration);
            //yield return new WaitForSeconds(2f);

            rectTrns.DOScale(m_scale, m_duration);            
            yield return new WaitForSeconds(m_duration + 0.1f);
            rectTrns.DOScale(InitScale, m_duration);
            yield return new WaitForSeconds(m_duration + 0.1f);
        }
    }
}
