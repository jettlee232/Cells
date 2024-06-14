using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_Mito : MonoBehaviour
{
    public int atpScore = 0;
    public float atpTime = 1.0f;
    public int atpCount = 0;
    public int atpGoal = 4;

    public Text atpGoalText;
    public Text atpScoreText;
    public Text atpTimeText;
    public Image atpTimeValue;

    void Start()
    {
        // Fixed Timestep�� �������ִ� �ڵ�?
        Time.fixedDeltaTime = (Time.timeScale / UnityEngine.XR.XRDevice.refreshRate);
        BtnOnClickGameStart();
    }

    void Update()
    {
        UpdateScoreUI();
        UpdateTimeUI();
    }

    public void IncreaseScore(int amount)
    {
        atpScore += amount;
        atpCount++;
        //UpdateScoreUI();
        
        if (atpCount >= atpGoal)
        {
            GameClear();
        }
    }

    public void IncreaseTime(float amount)
    {
        atpTime += amount;
        //UpdateTimeUI();
    }

    private void UpdateScoreUI()
    {
        atpScoreText.text = "ATP ����\n" + atpScore;
        atpGoalText.text = "ATP ��ǥ\n" + atpCount + " / " + atpGoal;
    }

    private void UpdateTimeUI()
    {
        int roundedTime = Mathf.RoundToInt(atpTime * 100);

        atpTimeText.text = roundedTime.ToString() + "%";
        atpTimeValue.fillAmount = atpTime;
    }

    public void BtnOnClickGameStart()
    {
        StartTimer();
    }

    void StartTimer()
    {
        StartCoroutine(DecreaseTime());
    }

    IEnumerator DecreaseTime()
    {
        while (atpTime > 0.0f)
        {
            yield return new WaitForSeconds(2.0f);
            atpTime -= 0.01f;
        }
    }

    void GameClear()
    {
        Debug.Log("ATP�� ��ǥġ��ŭ ��ҽ��ϴ�.");
    }
}
