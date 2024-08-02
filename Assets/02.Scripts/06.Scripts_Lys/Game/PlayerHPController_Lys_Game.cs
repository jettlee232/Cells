using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPController_Lys_Game : MonoBehaviour
{
    public int fullHp = 5;
    public int nowHp = 5;
    public float recoveryTimer = 10f;
    public float nowTimer = 0f;
    public float healTimer = 2f;
    private bool gameover = false;
    private bool recovering = false;

    void Start()
    {
        gameover = false;
        recovering = false;
        nowHp = fullHp;
        nowTimer = 0f;
        StartCoroutine(cRecovery());
    }

    public void Hitted()
    {
        nowHp--;
        if (recovering) { nowTimer = 0f; }
        else { StopCoroutine(cHeal()); }
        // 여기에 피격 이펙트
        if (nowHp <= 0 && !gameover)
        {
            // 게임 오버 코드
            GameManager_Lys_Game.instance.GameOver();
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
}
