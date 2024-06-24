using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager_CM : MonoBehaviour
{
    [Header("Gaming Variable")]
    public int rightAns; // ���� ����
    public int wrongAns; // ���� ����
    private int score = 100; // ���� �� ���� ȹ���ϴ� ����, ����
    public int curScore; // ���� ����
    public int goalScore = 5000; // ��ǥ ����
    public int rightAnsStreak; // ���� ����
    public float scoreMultiply = 1.0f; // �޺� ���
    public float curLeftTime; // ���� ���� �ð�
    public float leftTime = 180.0f;
    public float countdownTime = 3; // ī��Ʈ�ٿ� �ð�
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
    public BNG.MyFader_CM scrFader;
    public BNG.SmoothLocomotion smoothLocomotion;
    public BNG.UIPointer uiPointer;


    void Start()
    {
        smoothLocomotion.enabled = false; // 일단 이 코드 2개는 무조건 비활성화되게 수정
        uiPointer.enabled = false; // 일단 이 코드 2개는 무조건 비활성화되게 수정

        GameStart();
    }

    //
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameRestart();
        }
    }
    //

    public void GameStart()
    {
        Debug.Log("Game Start!");
        StartCoroutine(CountDown()); // ī��Ʈ�ٿ� �ڷ�ƾ        
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

        // �ΰ��� UI�� ���̰�
        timerCountText.gameObject.SetActive(true);
        scorePanel.gameObject.SetActive(true);
        ruleText.gameObject.SetActive(true);

        // ���� ���� ����
        bsMgr.BlockSpawnStart();

        // ���� ���� �ð� ī��Ʈ ����
        curLeftTime = leftTime;
        StartCoroutine(TimerOnGo());
    }

    public void Scoreup() // ���� �� ���� �Լ� - ���� ���� (�ܺο��� ���� like SaberWF/CC)
    {
        curScore += (int)(score * scoreMultiply); // ���� ������ ���� �߰� (100 X ���� ���)
        rightAns++; // ���� ���� ����

        rightAnsStreak++; // ���� ���� ���� ����
        ScoreStreakComboMultiplyCheck();

        TextUpdate_ScoreAndCombo(); // �ؽ�Ʈ ������Ʈ_������ �޺� �Լ� ����

        CheckScore(); // ���� ���� ���� (��ǥ ���� �޼� ����) �˻�
    }

    public void ScoreStreakComboMultiplyCheck() // ���� ���� ������ ���� ��� ���� Ȯ��
    {
        if (rightAnsStreak > 0 && rightAnsStreak % 2 == 0) // 10�� ������ ������ �����ϸ�
        {
            scoreMultiply += 0.2f; // ���� ��� 0.2 ���� ex) 1.0x -> 1.2x -> 1.4x

            multiplyCountText.text = "x " + scoreMultiply.ToString();
            Debug.Log(scoreMultiply);
        }
    }

    public void TextUpdate_ScoreAndCombo()
    {
        scoreCountText.text = curScore.ToString(); // ���� ������ string���� ��ȯ�Ͽ� scoreCountText�� text ������Ʈ�� ����
        comboCountText.text = rightAnsStreak.ToString(); // ���������� ���� ���� ������ ����
        multiplyCountText.text = "x " + scoreMultiply.ToString();
    }

    public void CheckScore()
    {
        if (curScore >= goalScore) // ��ǥ ������ �����ϸ�
        {
            isSucces = true;
            GameOver();
        }
    }

    public void ScoreDown() // ���� �� ���� �Լ� - ���� �϶��� ���µ� �ϴ� ���� (�ܺο��� ���� like BlockDestPlane Or SaberWF/CC)
    {
        wrongAns++;
        rightAnsStreak = 0;
        scoreMultiply = 1.0f;
        TextUpdate_ScoreAndCombo();
    }

    public void RedFade()
    {
        StartCoroutine(WrondAnswerEffect());
    }

    public void GameOver()
    {
        isGameStart = false;

        // BlockSpawnManager���� ���� ���� �׸��϶�� �ϱ�
        bsMgr.BlockSpawnStop();

        // ����� ���ÿ� ����Ǵ� �Լ��� �ۼ�, (���� UI�� �Ⱥ��̰�, ���� ���� ���� UI�� ���̰�)
        timerCountText.gameObject.SetActive(false);
        scorePanel.gameObject.SetActive(false);
        ruleText.gameObject.SetActive(false);

        gameoverPanel.SetActive(true);
        // clearTimeText.text =  // �� �ð����� ���� �ð��� �� �� Time.date ��������
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
        //restartBlock.GetComponent<InstantiateEffect_CM>().GoStart();

        if (isSucces == true)
        {
            exitBlock.SetActive(true);
            //exitBlock.GetComponent<InstantiateEffect_CM>().GoStart();
        }
    }

    IEnumerator TimerOnGo()
    {
        while (isGameStart == true)
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

    IEnumerator WrondAnswerEffect()
    {
        scrFader.ChangeFadeImageColor(Color.red, 12f, 0.33f);
        scrFader.DoFadeIn();

        yield return new WaitForSeconds(0.75f);

        scrFader.DoFadeOut();
    }
}
