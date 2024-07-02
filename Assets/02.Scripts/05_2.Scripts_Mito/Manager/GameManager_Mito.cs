using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_Mito : MonoBehaviour
{
    public static GameManager_Mito Instance { get; private set; }

    PlayerMoving_Mito playerMoving_Mito;
    public QuestPanel_Mito questPanelMito;

    public int atpScore = 0;
    public float atpCurTime = 1.0f;
    public float atpMaxTime = 1.0f;
    public int atpCount = 0;
    public int atpGoal = 4;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        // Fixed Timestep을 보정해주는 코드?
        Time.fixedDeltaTime = (Time.timeScale / UnityEngine.XR.XRDevice.refreshRate);

        playerMoving_Mito = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoving_Mito>();

        StartCoroutine(StartGame(5.0f));
    }

    IEnumerator StartGame(float delay)
    {
        questPanelMito.PanelOpen("미토콘드리아 속을 탐험하며 ATP를 만들어보자!");
        yield return new WaitForSeconds(delay);
        questPanelMito.PanelClose();
    }

    public void IncreaseScore(int amount)
    {
        atpScore += amount;
        atpCount++;
        
        if (atpCount >= atpGoal)
        {
            GameClear();
        }
    }

    public void IncreaseTime(float amount)
    {
        atpCurTime += amount;
        if (atpCurTime > atpMaxTime)
            atpCurTime = atpMaxTime;
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
        while (atpCurTime > 0.0f)
        {
            yield return new WaitForSeconds(5.0f);
            atpCurTime -= 0.01f;
        }
    }

    void GameClear()
    {
        Debug.Log("ATP를 목표치만큼 모았습니다.");
    }
}
