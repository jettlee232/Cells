using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class Cutscene_Lobby : MonoBehaviour
{
    public bool isButtonPressed = false;
    public Image black1;
    public Image black2;
    public float totalTime;
    UnityEngine.XR.InputDevice right;


    void Start()
    {
        FadeIn();
        Invoke("FadeOutAndMoveScene", totalTime / 60f);
    }


    void Update()
    {
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (right.isValid)
        {
            right.TryGetFeatureValue(CommonUsages.primaryButton, out isButtonPressed);
        }

        if (isButtonPressed == true)
        {
            SceneManager.LoadScene("02_Lobby");
        }
    }

    public void FadeOutAndMoveScene()
    {
        StartCoroutine(MoveScene());
    }

    IEnumerator MoveScene()
    {
        float elapsedTime = 0f;
        Color startColor = black1.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        while (elapsedTime < 0.75f)
        {
            float alpha = Mathf.Lerp(startColor.a, targetColor.a, elapsedTime / 0.75f);
            black1.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        black1.color = targetColor;
        SceneManager.LoadScene("02_Lobby"); // Scene ÀÎµ¦½ºµµ ¼öÁ¤ÇÏ¸é µÊ
    }

    public void FadeIn()
    {
        StartCoroutine(cFadeIn());
    }

    IEnumerator cFadeIn()
    {
        float elapsedTime = 0f;
        Color startColor = black2.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsedTime < 0.25f)
        {
            float alpha = Mathf.Lerp(startColor.a, targetColor.a, elapsedTime / 0.25f);
            black2.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        black2.color = targetColor;
    }
}
