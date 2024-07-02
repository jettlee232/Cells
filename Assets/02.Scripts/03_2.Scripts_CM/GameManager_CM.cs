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
    public GameObject comboPanel;
    public TextMeshProUGUI scoreCountText;
    public TextMeshProUGUI comboCountText;
    public TextMeshProUGUI multiplyCountText;

    [Header("Over-Game Text Contents")]
    public GameObject gameoverPanel;
    public TextMeshProUGUI clearTimeText;
    public TextMeshProUGUI rightAnsText;
    public TextMeshProUGUI wrongAnsText;
    public TextMeshProUGUI rightAnsStreakText;
    public TextMeshProUGUI curScoreCountText;
    public TextMeshProUGUI successOrFailText;
    public GameObject[] successOrFailTextPrefab = new GameObject[2];
    public GameObject restartBlock;
    public GameObject exitBlock;

    [Header("Other Scripts")]
    public BlockSpawnManager_CM bsMgr;
    public BNG.MyFader_CM scrFader;
    public BNG.SmoothLocomotion smoothLocomotion;
    public BNG.UIPointer uiPointer;
    public LineRenderer lineRenderer;
    public QuestPanel_CM quest;

    void Start()
    {
        GameStart();

        smoothLocomotion.enabled = false; // Maybe Refactorung Later?
        uiPointer.enabled = false; // Maybe Refactorung Later?
        lineRenderer.enabled = false; // Maybe Refactorung Later?
    }


    public void GameStart()
    {
        Debug.Log("Game Start!");
        StartCoroutine(CountDown());
        StartCoroutine(QuestStart());

        smoothLocomotion.enabled = false; // Maybe Refactorung Later?
        uiPointer.enabled = false; // Maybe Refactorung Later?
        lineRenderer.enabled = false; // Maybe Refactorung Later?
    }

    public void GameRestart()
    {
        AudioMgr_CM.Instance.PlaySFXByInt(10);

        gameoverPanel.SetActive(false);
        scorePanel.SetActive(false);
        comboPanel.SetActive(false);

        restartBlock.SetActive(false);
        restartBlock.transform.position = new Vector3(-0.5f, 0.5f, 1.5f);
        restartBlock.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        exitBlock.SetActive(false);
        exitBlock.transform.position = new Vector3(0.5f, 0.5f, 1.5f);
        exitBlock.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

        rightAns = 0;
        wrongAns = 0;
        curScore = 0;
        rightAnsStreak = 0;
        scoreMultiply = 1.0f;

        successOrFailTextPrefab[0].SetActive(false);
        successOrFailTextPrefab[1].SetActive(false);
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
            countdownTimerText.text = countdownTime.ToString();
            AudioMgr_CM.Instance.PlaySFXByInt(2);

            yield return new WaitForSeconds(1);

            countdownTime--;
        }

        AudioMgr_CM.Instance.PlaySFXByInt(15);
        isGameStart = true;

        countdownTime = 3;
        countdownTimerText.gameObject.SetActive(false);

        timerCountText.gameObject.SetActive(true);
        scorePanel.gameObject.SetActive(true);
        comboPanel.gameObject.SetActive(true);

        bsMgr.BlockSpawnStart();

        curLeftTime = leftTime;
        StartCoroutine(TimerOnGo());
    }

    public void Scoreup()
    {
        curScore += (int)(score * scoreMultiply);
        rightAns++;

        rightAnsStreak++;
        ScoreStreakComboMultiplyCheck();

        TextUpdate_ScoreAndCombo();
        CheckScore();
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
        scoreCountText.text = curScore.ToString();
        comboCountText.text = rightAnsStreak.ToString();
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

    public void BlueFade()
    {
        StartCoroutine(RightAnswerEffect());
    }

    public void RedFade()
    {
        StartCoroutine(WrondAnswerEffect());
    }

    public void GameOver()
    {
        isGameStart = false;

        bsMgr.BlockSpawnStop();

        timerCountText.gameObject.SetActive(false);

        gameoverPanel.SetActive(true);

        // Calculate Clear Time
        float remainingTime = leftTime - curLeftTime;
        int minutes = Mathf.FloorToInt(remainingTime / 60F);
        int seconds = Mathf.FloorToInt(remainingTime % 60F);
        clearTimeText.text = string.Format("{0}:{1:00}", minutes, seconds);

        rightAnsText.text = rightAns.ToString();
        wrongAnsText.text = wrongAns.ToString();
        rightAnsStreakText.text = rightAnsStreak.ToString();
        curScoreCountText.text = curScore.ToString();

        //if (isSucces == true) successOrFailText.text = "Success!";
        //else successOrFailText.text = "Fail!";

        if (isSucces == true)
        {
            AudioMgr_CM.Instance.PlaySFXByInt(14);
            successOrFailTextPrefab[0].SetActive(true);
        }
        else
        {
            AudioMgr_CM.Instance.PlaySFXByInt(12);
            successOrFailTextPrefab[1].SetActive(false);
        }

        StartCoroutine(MakeEndEventBlock());
    }

    public void NewQuest(string questContents)
    {
        if (quest.gameObject.activeSelf == false) quest.gameObject.SetActive(true);
        quest.PanelOpen(FireStoreManager_Test_CM.Instance.ReadCSV(questContents)); // text quest change
    }

    IEnumerator QuestStart()
    {
        yield return new WaitForSeconds(1f);
        NewQuest("Quest_CM_7");
        yield return new WaitForSeconds(5f);
        quest.PanelClose();
    }


    IEnumerator MakeEndEventBlock()
    {
        yield return new WaitForSeconds(2f);

        InstantiateTween_CM restartB = restartBlock.GetComponent<InstantiateTween_CM>();
        InstantiateTween_CM exitB = exitBlock.GetComponent<InstantiateTween_CM>();

        restartBlock.SetActive(true);
        if (restartB.IsThisRemade() == true) restartB.GoTween();

        if (isSucces == true)
        {
            exitBlock.SetActive(true);
            if (exitB.IsThisRemade() == true) exitB.GoTween();
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

    IEnumerator RightAnswerEffect()
    {
        scrFader.ChangeFadeImageColor(Color.blue, 12f, 0.33f);
        scrFader.DoFadeIn();

        yield return new WaitForSeconds(0.75f);

        scrFader.DoFadeOut();
    }

    IEnumerator WrondAnswerEffect()
    {
        scrFader.ChangeFadeImageColor(Color.red, 12f, 0.33f);
        scrFader.DoFadeIn();

        yield return new WaitForSeconds(0.75f);

        scrFader.DoFadeOut();
    }
}
