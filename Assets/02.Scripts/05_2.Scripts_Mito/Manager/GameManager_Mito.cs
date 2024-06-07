using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_Mito : MonoBehaviour
{
    public int atpScore = 0;
    public float atpTime = 1.0f;
    public Text atpScoreText;
    public Text atpTimeText;
    public Image atpTimeValue;

    void Start()
    {
        // Fixed Timestep을 보정해주는 코드?
        Time.fixedDeltaTime = (Time.timeScale / UnityEngine.XR.XRDevice.refreshRate);
        StartTimer();
    }

    void Update()
    {
        UpdateScoreUI();
        UpdateTimeUI();
    }

    public void IncreaseScore(int amount)
    {
        atpScore += amount;
        //UpdateScoreUI();
    }

    public void IncreaseTime(float amount)
    {
        atpTime += amount;
        //UpdateTimeUI();
    }

    private void UpdateScoreUI()
    {
        atpScoreText.text = "ATP 점수\n" + atpScore;
    }

    private void UpdateTimeUI()
    {
        int roundedTime = Mathf.RoundToInt(atpTime * 100);

        atpTimeText.text = roundedTime.ToString() + "%";
        atpTimeValue.fillAmount = atpTime;
    }

    public void StartTimer()
    {
        StartCoroutine(DecreaseTime());
    }

    IEnumerator DecreaseTime()
    {
        while (atpTime > 0.0f)
        {
            yield return new WaitForSeconds(1.0f);
            atpTime -= 0.01f;
        }
    }
}
