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
    private Coroutine glowCoroutine = null;

    void Start()
    {
        if (tempHl == null) { hlEffect = GetComponent<HighlightEffect>(); }
        else { hlEffect = tempHl; }
    }

    public HighlightEffect GetHl() { return hlEffect; }

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
        hlEffect.highlighted = true;

        yield return null;

        hlEffect.highlighted = false;
        glowCoroutine = null;
    }
}
