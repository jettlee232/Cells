using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightColorchange_Lobby : MonoBehaviour
{
    private GameObject[] hlGameObjects;
    public HighlightEffect[] hlEffect;

    public float updownFloat = 0.02f;

    public bool glowFlag = false;

    private Coroutine glowCoroutine = null;

    void Start()
    {
        hlGameObjects = GameObject.FindGameObjectsWithTag("Interactable");
        hlEffect = new HighlightEffect[hlGameObjects.Length];
        for (int i = 0; i < hlGameObjects.Length; i++)
        {
            hlEffect[i] = hlGameObjects[i].GetComponent<HighlightEffect>();
        }
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
        foreach (HighlightEffect effect in hlEffect) { effect.highlighted = true; }
        
        while (glowFlag)
        {
            foreach (HighlightEffect effect in hlEffect) { effect.highlighted = true; effect.innerGlow -= updownFloat; }
            yield return new WaitForSeconds(0.02f);

            if (hlEffect[0].innerGlow <= 0 || hlEffect[0].innerGlow >= 1)
            {
                updownFloat = -updownFloat;
            }
        }
        
        yield return null;

        foreach (HighlightEffect effect in hlEffect) { effect.highlighted = false; }
        glowCoroutine = null;
    }
}
