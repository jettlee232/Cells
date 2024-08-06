using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Result_Mito : MonoBehaviour
{
    public TextMeshProUGUI atpCountText;
    public TextMeshProUGUI atpGoalText;
    public TextMeshProUGUI atpTimeText;
    public TextMeshProUGUI atpScoreText;

    public GameObject successImage;
    public GameObject failImage;

    public void SetupPanel(bool isSuccess, int atpCount, int atpGoal, float atpCurTime, int atpScore)
    {
        // UI 텍스트 설정
        atpCountText.text = atpCount.ToString();
        atpGoalText.text = atpGoal.ToString();

        int roundedTime = Mathf.RoundToInt(atpCurTime * 100);
        atpTimeText.text = roundedTime.ToString() + "%";

        atpScoreText.text = atpScore.ToString();

        // 성공 또는 실패 이미지 활성화
        successImage.SetActive(isSuccess);
        failImage.SetActive(!isSuccess);
    }
}
