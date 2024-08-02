using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCommon_Lys_Game : MonoBehaviour
{
    private HighlightEffect hlEffect;
    private HighLightColorchange_Lys hlChange;

    private void Start()
    {
        hlEffect = GetComponent<HighlightEffect>();
        hlChange = GetComponent<HighLightColorchange_Lys>();
    }

    public void Die()
    {
        ScoreManager_Lys.instance.UpScore(100);

        hlEffect.highlighted = true;
        hlChange.GlowStart();
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
