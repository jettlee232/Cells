using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightColorchange_MitoTuto : MonoBehaviour
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
            StartCoroutine(Glow());
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
    
    IEnumerator Glow() // 추가, 수정한 부분
    {
        //hlEffect.highlighted = true;

        
        while (glowFlag)
        {
            hlEffect.highlighted = true;

            hlEffect.innerGlow -= updownFloat;
            yield return new WaitForSeconds(0.02f);

            if (hlEffect.innerGlow <= 0 || hlEffect.innerGlow >= 1)
            {
                updownFloat = -updownFloat;
            }
        }
        
        yield return null;

        hlEffect.highlighted = false;        
        glowCoroutine = null;
    }
}