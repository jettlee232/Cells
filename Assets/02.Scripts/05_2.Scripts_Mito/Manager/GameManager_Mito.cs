using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_Mito : MonoBehaviour
{
    public int atpScore = 0;
    public float atpTime = 0;

    void Start()
    {
        // Fixed Timestep을 보정해주는 코드?
        Time.fixedDeltaTime = (Time.timeScale / UnityEngine.XR.XRDevice.refreshRate);
    }

    //void Update()
    //{

    //}

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
}
