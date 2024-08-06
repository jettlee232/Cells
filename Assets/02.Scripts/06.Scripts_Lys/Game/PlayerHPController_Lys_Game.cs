using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPController_Lys_Game : MonoBehaviour
{
    public int fullHp = 5;
    public int nowHp = 5;
    public float recoveryTimer = 10f;
    public float nowTimer = 0f;
    public float healTimer = 2f;
    private bool gameover = false;
    private bool recovering = false;
    private GameObject PlayerToolTip;
    private Image HPImage;
    private TextMeshProUGUI HPText;
    private Image StImage;
    private TextMeshProUGUI StText;

    void Start()
    {
        PlayerToolTip = GameManager_Lys_Game.instance.GetPlayerToolTip();
        gameover = false;
        recovering = false;
        nowHp = fullHp;
        nowTimer = 0f;
        InitToolTip();
        StartCoroutine(cRecovery());
    }
    private void Update()
    {
        SetHP();
        SetSt();
    }

    public void Hitted()
    {
        if (GameManager_Lys_Game.instance.GetIsEnd()) { return; }

        if (recovering) { nowTimer = 0f; }
        else
        {
            StopAllCoroutines();
            StartCoroutine(cRecovery());
        }
        // 여기에 피격 이펙트
        UIManager_Lys_Game.instance.RedFade();
        if (nowHp >= 2) { nowHp--; }
        else if (!gameover)
        {
            nowHp = 0;
            // 게임 오버 코드
            gameover = true;
            GameManager_Lys_Game.instance.GameOver();
            StopAllCoroutines();
        }
    }

    public void Heal()
    {
        nowHp++;
        if (nowHp >= fullHp) { nowHp = fullHp; }
    }

    IEnumerator cRecovery()
    {
        recovering = true;
        nowTimer = 0f;

        while (true)
        {
            if (nowTimer >= recoveryTimer) { recovering = false; break; }
            nowTimer += Time.deltaTime;

            yield return null;
        }

        nowTimer = recoveryTimer;
        StartCoroutine(cHeal());
    }
    IEnumerator cHeal()
    {
        while (true)
        {
            if (nowHp < fullHp) { Heal(); }
            yield return new WaitForSeconds(healTimer);
        }
    }

    #region 툴팁

    void InitToolTip()
    {
        HPImage = PlayerToolTip.transform.GetChild(2).GetChild(1).GetComponent<Image>();
        HPText = PlayerToolTip.transform.GetChild(2).GetChild(4).GetComponent<TextMeshProUGUI>();
        StImage = PlayerToolTip.transform.GetChild(3).GetChild(1).GetComponent<Image>();
        StText = PlayerToolTip.transform.GetChild(3).GetChild(4).GetComponent<TextMeshProUGUI>();
    }

    void SetHP()
    {
        float hp = (float)nowHp / fullHp;
        HPImage.fillAmount = hp;
        HPText.text = Mathf.RoundToInt(hp * 100).ToString();
    }

    void SetSt()
    {
        float st = nowTimer / recoveryTimer;
        StImage.fillAmount = st;
        StText.text = Mathf.RoundToInt(st * 100).ToString();
    }

    #endregion
}
