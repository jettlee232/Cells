using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactables_Lobby : MonoBehaviour
{
    private HighlightEffect[] hlEffect;
    public GameObject[] glows;
    public float updownFloat = 0.02f;
    public bool glowFlag = false;
    private Coroutine glowCoroutine = null;

    void Start()
    {
        hlEffect = new HighlightEffect[glows.Length];
        for (int i = 0; i < glows.Length; i++)
        {
            hlEffect[i] = glows[i].GetComponent<HighlightEffect>();
        }
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
        foreach (HighlightEffect hl in hlEffect) { hl.highlighted = true; }

        while (glowFlag)
        {
            foreach (HighlightEffect hl in hlEffect) { hl.highlighted = true; hl.innerGlow -= updownFloat; }
            yield return new WaitForSeconds(0.02f);
            if (hlEffect[0].innerGlow <= 0 || hlEffect[0].innerGlow >= 1)
            {
                updownFloat = -updownFloat;
            }
        }

        yield return null;

        foreach (HighlightEffect hl in hlEffect) { hl.highlighted = false; }
        glowCoroutine = null;
    }
}
