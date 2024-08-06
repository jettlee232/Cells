using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Oculus.Interaction.OptionalAttribute;

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
    public GameObject timerPanel;
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

    [Header("TutoPanels")]
    public GameObject[] tutoPanels;
    private bool tutoFlag = false;

    [Header("ScoreOver1000")]
    public AudioClip clip1000;
    public GameObject over1000Panel;

    [Header("Entire Game & Cutscenes")]
    public GameObject entireGame;
    public GameObject cutScene;

    void Start()
    {
        //GameStart();
        Invoke("PopupTutoPanel", 3f);
        Invoke("PopupReadyFlag", 6f);

        smoothLocomotion.enabled = false; // Maybe Refactorung Later?
        uiPointer.enabled = false; // Maybe Refactorung Later?
        lineRenderer.enabled = false; // Maybe Refactorung Later?
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(5);
    }

    public void PopupTutoPanel()
    {
        if (tutoFlag == false)
        {
            tutoPanels[0].SetActive(true);
            tutoPanels[1].SetActive(true);
            tutoPanels[2].SetActive(true);
            tutoFlag = true;
        }
        else
        {
            tutoPanels[0].GetComponent<TutorialPanelTween_CM>().ReverseTweenAndDestroy();
            tutoPanels[1].GetComponent<TutorialPanelTween_CM>().ReverseTweenAndDestroy();
            tutoPanels[2].GetComponent<TutorialPanelTween_CM>().ReverseTweenAndDestroy();
            tutoPanels[3].GetComponent<TutorialPanelTween_CM>().ReverseTweenAndDestroy();
            Invoke("GameStart", 2f);
        }
    }

    void PopupReadyFlag()
    {
        tutoPanels[3].SetActive(true);
    }

    public void GameStart()
    {
        bsMgr.isHardMode = false;

        countdownTimerText.gameObject.SetActive(true);
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
        timerPanel.SetActive(false);
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
        //yield return new WaitForSeconds(1);

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

        //timerCountText.gameObject.SetActive(true);
        timerPanel.SetActive(true);
        scorePanel.SetActive(true);
        comboPanel.SetActive(true);

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
        if (curScore >= 1000 && bsMgr.isHardMode == false)
        {
            StartCoroutine(ScoreOverThousand());
        }
        if (curScore >= goalScore)
        {
            isSucces = true;
            GameOver();
        }
    }

    public void ScoreDown(int amount) // ���� �� ���� �Լ� - ���� �϶��� ���µ� �ϴ� ���� (�ܺο��� ���� like BlockDestPlane Or SaberWF/CC)
    {
        curScore -= amount;
        if (curScore < 0) curScore = 0;

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
        bsMgr.DestroyAllBlocks();
        //timerCountText.gameObject.SetActive(false);
        timerPanel.SetActive(false);
        gameoverPanel.SetActive(true);
        //over1000Panel.GetComponent<TutorialPanelTween_CM>().ReverseTweenAndDestroy();

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
            successOrFailTextPrefab[1].SetActive(true);
        }

        StartCoroutine(MakeEndEventBlock());
    }
    
    public void NewQuest(string questContents)
    {
        if (quest.transform.GetChild(0).gameObject.activeSelf == false) quest.transform.GetChild(0).gameObject.SetActive(true);

        quest.PanelOpen(FireStoreManager_Test_CM.Instance.ReadCSV(questContents)); // text quest change
    }

    public void DoLoadNewScene()
    {
        StartCoroutine(EndGameAndStartCutScene());
    }

    public void LoadStageMap()
    {
        SceneManager.LoadScene(5);
    }


    IEnumerator EndGameAndStartCutScene()
    {
        scrFader.ChangeFadeImageColor(Color.black, 2f, 1f);
        scrFader.DoFadeIn();

        yield return new WaitForSeconds(3.25f);

        cutScene.SetActive(true);
        entireGame.SetActive(false);   
    }

    IEnumerator QuestStart()
    {
        yield return new WaitForSeconds(1f);
        NewQuest("Quest_CM_6");
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

    IEnumerator ScoreOverThousand()
    {
        bsMgr.BlockSpawnStop();
        bsMgr.DestroyAllBlocks();
        //AudioMgr_CM.Instance.audioSrc.PlayOneShot(clip1000);

        yield return new WaitForSeconds(7f);

        bsMgr.isHardMode = true;
        bsMgr.BlockSpawnStart();
        over1000Panel.SetActive(true);
        if (over1000Panel.GetComponent<TutorialPanelTween_CM>().ReturnFlag() == true) 
            over1000Panel.GetComponent<TutorialPanelTween_CM>().StartInstTween();

        yield return new WaitForSeconds(2f);

        over1000Panel.GetComponent<TutorialPanelTween_CM>().ReverseTweenAndDestroy();
    }
}
