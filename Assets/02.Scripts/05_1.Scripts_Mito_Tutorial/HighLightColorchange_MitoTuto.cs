using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightColorchange_MitoTuto : MonoBehaviour
{
    public HighlightEffect hlEffect;

    public float updownFloat = 0.02f;

    public bool glowFlag = false;
    public bool updownFlag = false;

    private Coroutine glowCoroutine = null;

    void Start()
    {
        hlEffect = GetComponent<HighlightEffect>();
    }

    public void GlowStart()
    {
        if (glowCoroutine == null)
        {
            glowCoroutine = StartCoroutine(Glow());
        }
    }

    public void GlowEnd()
    {
        glowFlag = false;

        if (glowCoroutine != null)
        {
            StopCoroutine(glowCoroutine);
            glowCoroutine = null;
        }
    }

    IEnumerator Glow()
    {
        glowFlag = true;

        while (glowFlag)
        {
            hlEffect.innerGlow -= updownFloat;
            yield return new WaitForSeconds(0.02f);

            if (hlEffect.innerGlow <= 0 || hlEffect.innerGlow >= 1)
            {
                updownFloat = -updownFloat;
            }
        }

        updownFloat = 0.02f;
    }
}