using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager_Home : MonoBehaviour
{
    public Image black;

    private void Start()
    {
        PlayerPrefs.SetInt("MultiCheck1", 0);
        PlayerPrefs.SetInt("MultiCheck2", 0);
    }

    public void OnClickStartBtn() { StartCoroutine(MoveScene()); }

    IEnumerator MoveScene()
    {
        PlayerPrefs.SetInt("PlayerScore", 0);

        float elapsedTime = 0f;
        Color startColor = black.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        while (elapsedTime < 0.75f)
        {
            float alpha = Mathf.Lerp(startColor.a, targetColor.a, elapsedTime / 0.75f);
            black.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        black.color = targetColor;
        SceneManager.LoadScene("02_0_Lobby_Cutscenes1"); // Scene ÀÎµ¦½ºµµ ¼öÁ¤ÇÏ¸é µÊ
    }
}
