using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager_CM : MonoBehaviour
{
    [Header("Gaming Variable")]
    public int rightAns; // 정답 개수
    public int wrongAns; // 오답 개수
    private int score = 100; // 정답 시 마다 획득하는 점수, 고정
    public int curScore; // 현재 점수
    public int goalScore = 5000; // 목표 점수
    public int rightAnsStreak; // 연속 정답
    public float scoreMultiply = 1.0f; // 콤보 배수
    public float curLeftTime; // 남은 게임 시간
    public float leftTime = 180.0f;
    public float countdownTime = 3; // 카운트다운 시간
    public bool isGameStart = false;
    public bool isSucces;    

    [Header("In-Game Text Contents")]
    public TextMeshProUGUI countdownTimerText;
    public TextMeshProUGUI timerCountText;
    public GameObject scorePanel;
    public TextMeshProUGUI scoreCountText;
    public TextMeshProUGUI comboCountText;
    public TextMeshProUGUI multiplyCountText;
    public TextMeshProUGUI ruleText;

    [Header("Over-Game Text Contents")]
    public GameObject gameoverPanel;
    public TextMeshProUGUI clearTimeText;
    public TextMeshProUGUI rightAnsText;
    public TextMeshProUGUI wrongAnsText;
    public TextMeshProUGUI rightAnsStreakText;
    public TextMeshProUGUI curScoreCountText;
    public TextMeshProUGUI successOrFailText;
    public GameObject restartBlock;
    public GameObject exitBlock;

    [Header("Other Scripts")]
    public BlockSpawnManager_CM bsMgr;

    void Start()
    {
        GameStart();
    }

    public void GameStart()
    {
        Debug.Log("Game Start!");
        StartCoroutine(CountDown()); // 카운트다운 코루틴        
    }

    public void GameRestart()
    {
        gameoverPanel.SetActive(false);

        restartBlock.SetActive(false);
        exitBlock.SetActive(false);

        rightAns = 0;
        wrongAns = 0;
        curScore = 0;
        rightAnsStreak = 0;
        scoreMultiply = 1.0f;
        TextUpdate_ScoreAndCombo();

        GameStart();
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(1);

        countdownTimerText.gameObject.SetActive(true);
        countdownTimerText.text = "";
        while (countdownTime != 0)
        {            
            Debug.Log(countdownTime);

            countdownTimerText.text = countdownTime.ToString();
            
            yield return new WaitForSeconds(1);

            countdownTime--;
        }
        Debug.Log("Count Down Over!");
        isGameStart = true;

        countdownTime = 3;
        countdownTimerText.gameObject.SetActive(false);

        // 인게임 UI들 보이게
        timerCountText.gameObject.SetActive(true);
        scorePanel.gameObject.SetActive(true);
        ruleText.gameObject.SetActive(true);

        // 블록 스폰 시작
        bsMgr.BlockSpawnStart();

        // 남은 게임 시간 카운트 시작
        curLeftTime = leftTime;
        StartCoroutine(TimerOnGo());
    }

    public void Scoreup() // 정답 시 실행 함수 - 점수 증가 (외부에서 실행 like SaberWF/CC)
    {
        curScore += (int)(score * scoreMultiply); // 현재 점수에 점수 추가 (100 X 점수 배수)
        rightAns++; // 정답 개수 증가

        rightAnsStreak++; // 연속 정답 개수 증가
        ScoreStreakComboMultiplyCheck();
        
        TextUpdate_ScoreAndCombo(); // 텍스트 업데이트_점수와 콤보 함수 실행

        CheckScore(); // 게임 종료 조건 (목표 점수 달성 여부) 검사
    }
    
    public void ScoreStreakComboMultiplyCheck() // 연속 정답 개수로 점수 배수 증가 확인
    {
        if (rightAnsStreak > 0 && rightAnsStreak % 2 == 0) // 10의 단위로 점수가 증가하면
        {
            scoreMultiply += 0.2f; // 점수 배수 0.2 증가 ex) 1.0x -> 1.2x -> 1.4x

            multiplyCountText.text = "x " + scoreMultiply.ToString();
            Debug.Log(scoreMultiply);
        }
    }

    public void TextUpdate_ScoreAndCombo()
    {
        scoreCountText.text = curScore.ToString(); // 현재 점수를 string으로 변환하여 scoreCountText의 text 컴포넌트에 대입
        comboCountText.text = rightAnsStreak.ToString(); // 마찬가지로 연속 정답 개수도 적용
        multiplyCountText.text = "x " + scoreMultiply.ToString();
    }

    public void CheckScore()
    {
        if (curScore >= goalScore) // 목표 점수에 도달하면
        {
            isSucces = true;
            GameOver();
        }
    }

    public void ScoreDown() // 오답 시 실행 함수 - 점수 하락은 없는데 일단 ㅇㅇ (외부에서 실행 like BlockDestPlane Or SaberWF/CC)
    {
        wrongAns++;
        rightAnsStreak = 0;
        scoreMultiply = 1.0f;
        TextUpdate_ScoreAndCombo();
    }

    public void GameOver()
    {
        isGameStart = false;

        // BlockSpawnManager한테 블록 생성 그만하라고 하기
        bsMgr.BlockSpawnStop();

        // 종료와 동시에 실행되는 함수들 작성, (기존 UI들 안보이게, 게임 종료 전용 UI는 보이게)
        timerCountText.gameObject.SetActive(false);
        scorePanel.gameObject.SetActive(false);
        ruleText.gameObject.SetActive(false);

        gameoverPanel.SetActive(true);
        // clearTimeText.text =  // 총 시간에서 현재 시간을 뺀 걸 Time.date 형식으로
        rightAnsText.text = rightAns.ToString();
        wrongAnsText.text = wrongAns.ToString();
        rightAnsStreakText.text = rightAnsStreak.ToString();
        curScoreCountText.text = curScore.ToString();
        
        if (isSucces == true) successOrFailText.text = "Success!";
        else successOrFailText.text = "Fail!";

        StartCoroutine(MakeEndEventBlock());
    }

    IEnumerator MakeEndEventBlock()
    {
        yield return new WaitForSeconds(2f);

        restartBlock.SetActive(true);
        if (isSucces == true) exitBlock.SetActive(true);        
    }

    IEnumerator TimerOnGo()
    {
        while(isGameStart == true)
        {
            if (curLeftTime <= 0)
            {
                isSucces = false;
                GameOver();
            }

            int minutes = Mathf.FloorToInt(curLeftTime / 60);
            int seconds = Mathf.FloorToInt(curLeftTime % 60);

            timerCountText.text = string.Format("{0}:{1:00}", minutes, seconds);
            
            yield return new WaitForSeconds(1f);
            curLeftTime -= 1.0f;
        }
    }
}
