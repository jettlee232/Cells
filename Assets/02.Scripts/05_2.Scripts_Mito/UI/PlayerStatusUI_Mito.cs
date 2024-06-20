using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI_Mito : MonoBehaviour
{
    public TextMeshProUGUI atpGoalText;
    public TextMeshProUGUI atpScoreText;
    public TextMeshProUGUI atpTimeText;
    public Image atpTimeValue;

    void Update()
    {
        UpdateScoreUI();
        UpdateTimeUI();
    }

    private void UpdateScoreUI()
    {
        int atpCount = GameManager_Mito.Instance.atpCount;
        int atpGoal = GameManager_Mito.Instance.atpGoal;
        int atpScore = GameManager_Mito.Instance.atpScore;

        atpGoalText.text = atpCount + " / " + atpGoal;
        atpScoreText.text = atpScore + " Á¡";
    }

    private void UpdateTimeUI()
    {
        float atpCurTime = GameManager_Mito.Instance.atpCurTime;
        int roundedTime = Mathf.RoundToInt(atpCurTime * 100);

        atpTimeText.text = roundedTime.ToString() + "%";
        atpTimeValue.fillAmount = atpCurTime;
    }
}
