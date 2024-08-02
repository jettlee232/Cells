using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager_Lys : MonoBehaviour
{
    public static ScoreManager_Lys instance;
    public int score = 0;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
    }

    public void UpScore(int plusScore)
    {
        score += plusScore;
    }

    public void DownScore(int minusScore)
    {
        score -= minusScore;
    }
}
