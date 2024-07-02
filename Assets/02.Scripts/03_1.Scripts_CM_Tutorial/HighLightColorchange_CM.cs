using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightColorchange_CM : MonoBehaviour
{
    public HighlightEffect hlEffect;

    public float updownFloat = 0.02f;

    public bool glowFlag = false;

    private Coroutine glowCoroutine = null;

    void Start()
    {
        hlEffect = GetComponent<HighlightEffect>();
    }

    public void GlowStart()
    {
        if (glowCoroutine == null)
        {
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
