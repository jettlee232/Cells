using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
    public GameObject RocketToolTip;
    public GameObject PlayerToolTip;
    public TextMeshProUGUI RocketCoolTimerText;
    public Image RocketCoolTimerImage;
    public GameObject RestartClearButton;
    public GameObject ExitClearButton;
    public GameObject RestartOverButton;
    public GameObject Gun;
    public GameObject Rocket;
    public GameObject grabber;
    public GameObject[] DieEffect;
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
    private Image TimerImage;
    private TextMeshProUGUI TimerText;
    private bool isEnd = false;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
    }

    private void Start()
    {
        StartTimer();
        ShowShootToolTip();
        InitLaunch();
    }

    #region 플레이어, NPC
    public GameObject GetPlayer() { return player; }
    public GameObject GetPlayerCam() { return playerCam; }
    public GameObject GetUIPointer() { return uiPointer; }
    public bool GetIsEnd() { return isEnd; }
    #endregion

    #region 로켓런처 쿨타임

    void InitLaunch()
    {
        launchTimerValue = 1f;
        launchable = true;
        RocketCoolTimer();
    }
    public bool GetLaunchable() { return launchable; }
    public float GetLaunchTimerValue() { return launchTimerValue; }
    public void Launch()
    {
        launchable = false;
        launchTimerValue = 0f;
        StartCoroutine(cWaitLaunch());
    }
    IEnumerator cWaitLaunch()
    {
        float timer = 0f;
        while (timer <= launchTimer)
        {
            timer += Time.deltaTime;
            launchTimerValue = timer / launchTimer;
            RocketCoolTimer();
            yield return null;
        }
        launchable = true;
        launchTimerValue = 1f;
        RocketCoolTimer();
    }
    void RocketCoolTimer()
    {
        RocketCoolTimerText.text = Mathf.FloorToInt(launchTimerValue * 100).ToString() + "%";
        RocketCoolTimerImage.fillAmount = launchTimerValue;
    }
    #endregion

    #region 타이머
    void InitTimer()
    {
        TimerImage = PlayerToolTip.transform.GetChild(4).GetChild(1).GetComponent<Image>();
        TimerText = PlayerToolTip.transform.GetChild(4).GetChild(4).GetComponent<TextMeshProUGUI>();
        int gametime = Mathf.RoundToInt(fullGameTime);
        int min = gametime / 60;
        int sec = gametime % 60;
        TimerText.text = min.ToString() + ":" + sec.ToString();
        TimerImage.fillAmount = 1f;

    }
    void StartTimer()
    {
        InitTimer();
        StartCoroutine(StartWaitTimer());
    }
    void FinishTimer()
    {
        //StopCoroutine(timer());
        StopAllCoroutines();
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
            SetTimerImage();
            SetTimerText();

            yield return null;
        }
        // 게임 클리어 판정
        SetTimerImage();
        SetTimerText();
        SpawnManager.SetActive(false);
        GameClear();
    }
    void SetTimerImage() { TimerImage.fillAmount = (fullGameTime - PlayTime) / fullGameTime; }
    void SetTimerText()
    {
        int restTime = Mathf.RoundToInt(fullGameTime - PlayTime);
        int min = restTime / 60;
        int sec = restTime % 60;
        TimerText.text = min.ToString() + ":" + sec.ToString();
    }
    #endregion

    #region 에너미 관련

    // 속도
    public float GetMaxEnemyComeTimer() { return maxEnemyComeTimer; }
    public float GetMinEnemyComeTimer() { return minEnemyComeTimer; }
    public float GetEPDistance() { return EPDistance; }
    public float GetRotateSpeed() { return rotateSpeed; }
    #endregion

    #region 총, 로켓런처 탄환
    public float GetGunBulletSpeed() { return GunSpeed; }
    public float GetRocketBulletSpeed() { return RocketSpeed; }
    public GameObject GetHittedEffect(int num) { return DieEffect[num]; }   // CD가 0, DP가 1, ES가 2
    #endregion

    #region 툴팁
    public void ShowShootToolTip() { ToolTip.GetComponent<Tooltip>().TooltipOn("A키를 눌러 무기 바꾸기"); }
    public void HideToolTip() { ToolTip.GetComponent<Tooltip>().TooltipOff(); }
    public void ShowGunToolTip() { RocketToolTip.SetActive(true); }
    public GameObject GetPlayerToolTip() { return PlayerToolTip; }
    #endregion

    public void GameOver()
    {
        isEnd = true;
        SpawnManager.SetActive(false);
        FinishTimer();
        UIManager_Lys_Game.instance.ShowGameOver();
        //Instantiate(RestartOverButton, new Vector3(0, 0, 10), Quaternion.identity);
        Instantiate(RestartClearButton, new Vector3(0, 0, 10), Quaternion.identity);
        Instantiate(ExitClearButton, new Vector3(0, 0, 10), Quaternion.identity);
    }

    public void GameClear()
    {
        isEnd = true;
        UIManager_Lys_Game.instance.ShowGameClear();
        Instantiate(RestartClearButton, new Vector3(0, 0, 10), Quaternion.identity);
        Instantiate(ExitClearButton, new Vector3(0, 0, 10), Quaternion.identity);
    }

    public void MoveScene(string sceneName)
    {
        StartCoroutine(cMoveScene(sceneName));
    }

    IEnumerator cMoveScene(string sceneName)
    {
        Gun.GetComponent<FixedGunController_Lys_Game>().enabled = false;
        Rocket.GetComponent<FixedRocketController_Lys_Game>().enabled = false;
        float timer = 0f;

        Color blackColor = new Color(0f, 0f, 0f, 1f);
        Color transparentColor = new Color(0f, 0f, 0f, 0f);

        GameObject BlackPanel = UIManager_Lys_Game.instance.GetBlackPanel();
        float fadeOutTimer = UIManager_Lys_Game.instance.GetFadeOutTimer();

        BlackPanel.gameObject.SetActive(true);

        BlackPanel.GetComponent<Image>().color = transparentColor;
        while (true)
        {
            if (BlackPanel.GetComponent<Image>().color.a >= 0.99999f) { BlackPanel.GetComponent<Image>().color = blackColor; break; }
            timer += Time.deltaTime;
            BlackPanel.GetComponent<Image>().color = Color.Lerp(transparentColor, blackColor, timer / fadeOutTimer);
            yield return null;
        }
        Destroy(grabber);
        yield return new WaitForSeconds(0.75f);

        SceneManager.LoadScene(sceneName);
    }
}
