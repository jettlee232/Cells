using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightColorchange_StageMap : MonoBehaviour
{
    public HighlightEffect hlEffect;

    public float updownFloat = 0.02f;

    public bool glowFlag = false;

    private Coroutine glowCoroutine = null;

    void Start()
    {
        hlEffect = transform.parent.GetComponent<HighlightEffect>();
    }

    public void GlowStart()
    {
        Debug.Log("하이라이트 글로우스타트 들어옴");
        if (glowCoroutine == null)
        {
            Debug.Log("하이라이트 글로우스타트 이프문 들어옴");
            glowFlag = true;
            glowCoroutine = StartCoroutine(Glow());
        }
    }

    public void GlowEnd()
    {
        if (glowCoroutine != null)
        {
            glowFlag = false;
            StopCoroutine(glowCoroutine);
            glowCoroutine = null;
        }
    }

    IEnumerator Glow()
    {
        Debug.Log("하이라이트 글로우 코루틴 들어옴");
        while (glowFlag)
        {
            hlEffect.innerGlow -= updownFloat;
            yield return new WaitForSeconds(0.02f);

            if (hlEffect.innerGlow <= 0 || hlEffect.innerGlow >= 1)
            {
                updownFloat = -updownFloat;
            }
        }

        glowCoroutine = null;
    }
}
