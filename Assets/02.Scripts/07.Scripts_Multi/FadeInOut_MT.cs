using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut_MT : MonoBehaviour
{
    public GameObject BlackPanel;

    [Header("Upside Subtitle Settings")]
    public float fadeInTimer = 0.75f;
    public float fadeOutTimer = 0.75f;

    private void Start()
    {
        FadeIn();
    }

    public void FadeIn() { StartCoroutine(cFade(true)); }
    public void FadeOut() { StartCoroutine(cFade(false)); }
    IEnumerator cFade(bool inout)   // true 로 넣으면 페이드인, false로 넣으면 페이드아웃
    {
        float timer = 0f;

        Color blackColor = new Color(0f, 0f, 0f, 1f);
        Color transparentColor = new Color(0f, 0f, 0f, 0f);

        BlackPanel.gameObject.SetActive(true);

        if (inout)  // 점점 밝아지게
        {
            BlackPanel.GetComponent<Image>().color = blackColor;
            while (true)
            {
                if (BlackPanel.GetComponent<Image>().color.a <= 0.00001f) { BlackPanel.GetComponent<Image>().color = transparentColor; break; }
                timer += Time.deltaTime;
                BlackPanel.GetComponent<Image>().color = Color.Lerp(blackColor, transparentColor, timer / fadeInTimer);
                yield return null;
            }
            BlackPanel.gameObject.SetActive(false);
        }
        else
        {
            BlackPanel.GetComponent<Image>().color = transparentColor;
            while (true)
            {
                if (BlackPanel.GetComponent<Image>().color.a >= 0.99999f) { BlackPanel.GetComponent<Image>().color = blackColor; break; }
                timer += Time.deltaTime;
                BlackPanel.GetComponent<Image>().color = Color.Lerp(transparentColor, blackColor, timer / fadeOutTimer);
                yield return null;
            }
        }
    }
}
