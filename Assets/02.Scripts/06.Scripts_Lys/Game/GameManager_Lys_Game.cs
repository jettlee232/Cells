using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class GameManager_Lys_Game : MonoBehaviour
{
    public static GameManager_Lys_Game instance;
    UnityEngine.XR.InputDevice right;
    UnityEngine.XR.InputDevice left;

    [Header("Settings")]
    public GameObject player;
    public GameObject playerCam;
    public GameObject uiPointer;
    public GameObject ScoreManager;
    public GameObject SpawnManager;
    public GameObject ToolTip;
    public float PlayTime = 0f;
    public float launchTimer = 10f;
    public float fullGameTime = 100f;
    public float waitTime = 3f;
    public float maxEnemyComeTimer = 10f;
    public float minEnemyComeTimer = 7f;
    public float EPDistance = 1.5f;
    public float rotateSpeed = 45f;
    public float GunSpeed = 30f;
    public float RocketSpeed = 15f;

    private bool launchable = true;
    private float launchTimerValue = 1f;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
    }

    private void Start()
    {
        StartTimer();
        ShowShootToolTip();
    }

    #region �÷��̾�, NPC
    public GameObject GetPlayer() { return player; }
    public GameObject GetPlayerCam() { return playerCam; }
    public GameObject GetUIPointer() { return uiPointer; }
    #endregion

    #region ���Ϸ�ó ��Ÿ��

    public bool GetLaunchable() { return launchable; }
    public float GetLaunchTimerValue() { return launchTimerValue; }
    public void Launch()
    {
        launchable = false;
        launchTimerValue = 0f;
    }
    IEnumerator cWaitLaunch()
    {
        float timer = 0f;
        while (timer > launchTimer)
        {
            timer += Time.deltaTime;
            launchTimerValue = timer / launchTimer;
            yield return null;
        }
        launchable = true;
        launchTimerValue = 1f;
    }

    #endregion

    #region Ÿ�̸�

    void StartTimer()
    {
        StartCoroutine(StartWaitTimer());
    }
    void FinishTimer()
    {
        StopCoroutine(timer());
    }
    IEnumerator StartWaitTimer()
    {
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(timer());
    }
    IEnumerator timer()
    {
        PlayTime = 0f;

        while (true)
        {
            if (PlayTime >= fullGameTime) { break; }
            PlayTime += Time.deltaTime;

            yield return null;
        }
        // ���� Ŭ���� ����
        SpawnManager.SetActive(false);
    }

    #endregion

    #region ���ʹ� ����

    // �ӵ�
    public float GetMaxEnemyComeTimer() { return maxEnemyComeTimer; }
    public float GetMinEnemyComeTimer() { return minEnemyComeTimer; }
    public float GetEPDistance() { return EPDistance; }
    public float GetRotateSpeed() { return rotateSpeed; }
    #endregion

    #region ��, ���Ϸ�ó źȯ
    public float GetGunBulletSpeed() { return GunSpeed; }
    public float GetRocketBulletSpeed() { return RocketSpeed; }
    #endregion

    #region ����
    public void ShowShootToolTip() { ToolTip.GetComponent<Tooltip>().TooltipOn("AŰ�� ���� ���� �ٲٱ�"); }
    public void HideToolTip() { ToolTip.GetComponent<Tooltip>().TooltipOff(); }
    #endregion

    public void GameOver()
    {
        SpawnManager.SetActive(false);
        FinishTimer();
    }

    public GameObject GetScoreManager() { return ScoreManager; }

    public void MoveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
