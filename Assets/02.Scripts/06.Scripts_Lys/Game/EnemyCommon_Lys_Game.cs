using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCommon_Lys_Game : MonoBehaviour
{
    private HighlightEffect hlEffect;
    private HighLightColorchange_Lys hlChange;
    private string type;
    private int fullhp;
    private int hp;
    private GameObject HPBar;
    private GameObject HitEffect;

    private void Start()
    {
        hlEffect = GetComponent<HighlightEffect>();
        hlChange = GetComponent<HighLightColorchange_Lys>();
        type = GetComponent<EnemyType_Lys>().GetType();
        HPBar = GetComponent<EnemyMove_Lys>().GetHP();

        switch (type)
        {
            case "CD":
                fullhp = 1;
                HitEffect = GameManager_Lys_Game.instance.GetHittedEffect(0);
                break;
            case "DP":
                fullhp = 2;
                HitEffect = GameManager_Lys_Game.instance.GetHittedEffect(1);
                break;
            case "ES":
                fullhp = 3;
                HitEffect = GameManager_Lys_Game.instance.GetHittedEffect(2);
                break;
        }
        hp = fullhp;
    }

    public void Hitted()
    {
        hlEffect.highlighted = true;
        hlChange.GlowStart();
        hp--;
        HPBar.GetComponent<Slider>().value = (float)(fullhp - hp) / fullhp * 100f;
        if (hp <= 0) { Die(); }
    }

    public void Die()
    {
        ScoreManager_Lys.instance.UpScore(fullhp * 100);

        Instantiate(HitEffect, this.transform.position, Quaternion.identity);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    public void Killed()
    {
        ScoreManager_Lys.instance.UpScore(fullhp * 100);

        Instantiate(HitEffect, this.transform.position, Quaternion.identity);
        hlEffect.highlighted = true;
        hlChange.GlowStart();
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
