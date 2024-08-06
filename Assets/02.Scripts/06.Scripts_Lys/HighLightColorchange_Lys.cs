using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightColorchange_Lys : MonoBehaviour
{
    public HighlightEffect tempHl;
    public HighlightEffect hlEffect;
    public float updownFloat = 0.02f;
    public bool glowFlag = false;
    public bool multi = false;
    public HighlightEffect[] multiHls;
    private Coroutine glowCoroutine = null;

    void Start()
    {
        if (!multi)
        {
            if (tempHl == null) { hlEffect = GetComponent<HighlightEffect>(); }
            else { hlEffect = tempHl; }
        }
    }

    public HighlightEffect GetHl()
    {
        if (!multi) { return hlEffect; }
        else { return multiHls[0]; }
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
        if (!multi)
        {
            hlEffect.highlighted = true;

            yield return null;

            hlEffect.highlighted = false;
            glowCoroutine = null;
        }
        else
        {
            foreach (HighlightEffect hl in multiHls) { hl.highlighted = true; }

            yield return null;

            foreach (HighlightEffect hl in multiHls) { hl.highlighted = false; }
            glowCoroutine = null;
        }
    }
}
