using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StarWatch_Home : MonoBehaviour
{
    public TextMeshProUGUI starsText;

    void Start()
    {
        starsText = GetComponentInChildren<TextMeshProUGUI>();

        int currentScore = PlayerPrefs.GetInt("PlayerScore", 0);
        starsText.text = currentScore.ToString();
    }
}
