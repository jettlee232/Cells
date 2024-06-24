using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.Networking;
using UnityEditor;
using UnityEngine.XR;
using Language.Lua;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using TMPro;

public class TutorialManager_CM : MonoBehaviour
{
    [Header("Test Block Variable")]
    public GameObject[] testBlocks; // 12���� �׽�Ʈ����
    public Transform[] testBlockSpawnpos; // 12���� �׽�Ʈ���� ��ġ ��ġ
    public int curCorrectAns; // ���� ���� ����
    public int correctAns; // ���� ����
    public bool[] correctAnss; // ���� ���� üũ

    [Header("Test Saber Variable")]
    public Transform spawnPos_Tail;
    public Transform spawnPos_Head;
    public Transform spawnPos_Single;
    public Transform spawnPos_Double;
    public GameObject tutorialObj_Tail;
    public GameObject tutorialObj_Head;
    public GameObject tutorialObj_Single;
    public GameObject tutorialObj_Double;
    public GameObject saberVar;

    [Header("Narrator Mgr")]
    public NarratorDialogueHub_CM_Tutorial narrator;
    public DialogueSystemController dsc;
    public bool firstGrab = false;
    public AudioSource audioSrc;
    public AudioClip[] audioClipsArr;
    public string audioFilePath;
    public bool isInzizilMakeIt = true;
    public int conv2Cnt = 0;
    public GameObject quizCanvas;
    public Animator anim;
    private bool nextFlag = false;

    [Header("Laser")]
    public BNG.UIPointer uIPointer;
    public LineRenderer lineRenderer;

    [Header("Player Variables")]
    public BNG.MyFader_CM scrFader;
    public BNG.SmoothLocomotion smoothLocomotion;

    [Header("Eyes Only Obj")]
    public GameObject[] eoVar = new GameObject[3];
    public GameObject[] eoObj = new GameObject[3];
    public Transform[] eoSpawnPos = new Transform[3];

    [Header("Phos Sticks")]
    public GameObject[] phosSticks;

    [Header("Make Effect")] // ����Ʈ ����
    public GameObject[] makeEffect;

    [Header("Panels")]
    public QuestPanel_CM quest;
    public RulePanel_CM rule;

    [Header("Grabbale")]
    public BNG.Grabber grab1;
    public BNG.Grabber grab2;


    void Start()
    {

        audioClipsArr = Resources.LoadAll<AudioClip>("");
        isInzizilMakeIt = true;
        quizCanvas.SetActive(false);

        //UILaserOnOff();

        //quest.gameObject.SetActive(false);
        //rule.gameObject.SetActive(false);
    }

    public void UILaserOnOff()
    {
        lineRenderer.enabled = false;
        if (uIPointer.enabled == true) uIPointer.enabled = false;
        else uIPointer.enabled = true;

        //Debug.Log("UIPointer is " + uIPointer.enabled);
        //uIPointer.AutoUpdateUITransforms = !uIPointer.AutoUpdateUITransforms;
    }

    public void QuestionByNarrator()
    {
        quest.gameObject.SetActive(false);
        rule.gameObject.SetActive(false);

        quizCanvas.SetActive(true);

        quizCanvas.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = FireStoreManager_Test_CM.Instance.ReadCSV("Quiz_CM_1");
        quizCanvas.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = FireStoreManager_Test_CM.Instance.ReadCSV("Quiz_CM_2");
        quizCanvas.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = FireStoreManager_Test_CM.Instance.ReadCSV("Quiz_CM_3");
    }

    // ������ ������ �Ӹ� ����� -> After First Encounter
    public void MaketutorialObj_Tail()
    {
        saberVar = Instantiate(tutorialObj_Tail, spawnPos_Tail.position, Quaternion.identity);
        saberVar.name = tutorialObj_Tail.name;
        MakeEffect(saberVar.transform, 0);
        AudioMgr_CM.Instance.PlaySFXByInt(7);

    }

    public void MakeTutorialObj_Head()
    {
        GameObject go = Instantiate(tutorialObj_Head, spawnPos_Head.position, Quaternion.identity);
        go.name = tutorialObj_Head.name;
        MakeEffect(go.transform, 0);
        AudioMgr_CM.Instance.PlaySFXByInt(7);
    }

    // ������ ������ �Ӹ� ��ġ�� -> After Second Conv
    public void MakeTutorialObj_Single()
    {
        GameObject go = Instantiate(tutorialObj_Single, spawnPos_Single.position, Quaternion.Euler(0, 0, 0));
        go.name = tutorialObj_Single.name;
        MakeEffect(go.transform, 0);
        AudioMgr_CM.Instance.PlaySFXByInt(7);
    }

    // ������ ������ ���� ����� �ϳ� �� ����� -> After Third Conv
    public void MaketutorialObj_Double()
    {
        GameObject go = Instantiate(tutorialObj_Double, spawnPos_Double.position, Quaternion.Euler(90, 90, 0));
        go.name = tutorialObj_Double.name;
        MakeEffect(go.transform, 0);
        AudioMgr_CM.Instance.PlaySFXByInt(7);
    }

    // �׽�Ʈ ���ϵ� �� ��ġ�ϱ� -> After Fourth Conv
    public void MakeTestBlocks(bool philicOrPhobic) // �׽�Ʈ ���ϵ� �� ��ġ�ϱ�
    {
        Debug.Log(philicOrPhobic);
        correctAnss = new bool[testBlocks.Length];

        int x = 0, y = 0;

        if (philicOrPhobic == true)
        {
            x = 0;
            y = 6;
        }
        else
        {
            x = 6;
            y = 12;
        }

        for (int i = x; i < y; i++)
        {
            GameObject go = Instantiate(testBlocks[i]);
            go.GetComponent<BlockMoving_CM>().speed = 0f; // �� �����̰� ���ǵ�� 0���� �������ѳ���
            go.transform.position = testBlockSpawnpos[i].transform.position;
            go.transform.name = testBlocks[i].transform.name;

            MakeEffect(go.transform, 0);

            if (i < 6) go.transform.rotation = Quaternion.Euler(0, -45, 0);
            else go.transform.rotation = Quaternion.Euler(0, 45, 0);

            correctAnss[i] = false;
        }

        AudioMgr_CM.Instance.PlaySFXByInt(9);
    }

    public void DeleteSaber()
    {
        if (saberVar.activeSelf == true)
        {
            if (saberVar.GetComponent<ObjectBeingHeldOrNot_CM>().statusFlag == 3)
            {
                Destroy(saberVar);
                MakeEffect(saberVar.transform, 0);
            }
            else
            {
                saberVar.SetActive(false);
                MakeEffect(saberVar.transform, 0);
            }
        }
        else
        {
            saberVar.SetActive(true);
            saberVar.GetComponent<BNG.Grabbable>().BeingHeld = false;
            saberVar.transform.position = spawnPos_Tail.position;
            saberVar.transform.rotation = Quaternion.identity;
            saberVar.GetComponent<InstantiateTween_CM>().GoTween();
        }


        AudioMgr_CM.Instance.PlaySFXByInt(8);
    }

    public void PlayAudioClip(int i)
    {
        audioSrc.Stop();
        audioSrc.PlayOneShot(audioClipsArr[i]);
    }

    public void PlayAudioClip(double num)
    {
        int i = (int)num;
        audioSrc.Stop();
        audioSrc.PlayOneShot(audioClipsArr[i]);
    }

    public void CorrectAnswer(GameObject go)
    {
        int rnd = Random.Range(0, 4);
        if (rnd == 0) PlayAudioClip(7);
        else if (rnd == 1) PlayAudioClip(14);
        else if (rnd == 2) PlayAudioClip(18);
        else if (rnd == 3) PlayAudioClip(28);

        int correctAnsCnt = 0;
        for (int i = 0; i < testBlocks.Length; i++) // (�Ʒÿ�)���̹��� ���� ������ � �������� �˾Ƴ���
        {
            if (testBlocks[i].name == go.name) // � �������� ã�Ƴ�����
            {
                correctAnss[i] = true; // �ش� ���ϰ� ������ ��ȣ�� ĭ�� ���� ó��                
                StartCoroutine(RemakeAfter2Sec(i));
            }

            if (correctAnss[i] == true) // ���� ���� ���� ���� ���� ����
            {
                correctAnsCnt++;
            }
        }

        if (correctAnsCnt > 6) // ���� ������ 6���� �Ѿ��
        {
            // ���� �������� �̵�
            isInzizilMakeIt = false;
            narrator.StartCov_6(); // ������ ����
        }
    }

    public void WrongAnswer(GameObject go)
    {
        StartCoroutine(WrondAnswerEffect());

        int rnd = Random.Range(0, 2);
        if (rnd == 0) PlayAudioClip(34);
        else if (rnd == 1) PlayAudioClip(35);

        for (int i = 0; i < testBlocks.Length; i++)
        {
            if (testBlocks[i].name == go.name)
            {
                StartCoroutine(RemakeAfter2Sec(i));
            }

            // ������ ����� ���μ�����
        }
    }

    public void PlayClip(double d)
    {
        AudioMgr_CM.Instance.PlaySFXByInt(d);
    }

    public void IntroduceEyesOnlyObject(double num)
    {
        int i = (int)num;

        eoVar[i] = Instantiate(eoObj[i], eoSpawnPos[i].position, Quaternion.identity);
        eoVar[i].name = eoObj[i].name;
        MakeEffect(eoVar[i].transform, 0);
        AudioMgr_CM.Instance.PlaySFXByInt(3);
    }

    public void DestroyEyesOnlyObject(double num)
    {
        int i = (int)num;
        MakeEffect(eoVar[i].transform, 0);
        eoVar[i].SetActive(!eoVar[i].activeSelf);
    }

    public void MakePhosSticks()
    {
        GameObject go = Instantiate(phosSticks[0], spawnPos_Tail.position, Quaternion.identity);
        go.name = phosSticks[0].name;
        MakeEffect(go.transform, 0);
        go = Instantiate(phosSticks[1], spawnPos_Head.position, Quaternion.identity);
        go.name = phosSticks[1].name;
        MakeEffect(go.transform, 0);

        AudioMgr_CM.Instance.PlaySFXByInt(3);
    }

    void MakeEffect(Transform pos, int i)
    {
        Instantiate(makeEffect[0], pos.position, Quaternion.identity);
    }

    public void FadeOutAndMoveScene()
    {
        StartCoroutine(MoveScene());
    }

    public void MakePlayerMove()
    {
        smoothLocomotion.enabled = !smoothLocomotion.enabled;
    }

    public void PlayNarratorAnimation(string animName)
    {
        //string animStr = "Base Layer." + animName;
        //anim.Play(animStr);
        anim.Play(animName);
    }

    public void NewQuest(string questContents)
    {
        if (quest.gameObject.activeSelf == false) quest.gameObject.SetActive(true);
        quest.PanelOpen(FireStoreManager_Test_CM.Instance.ReadCSV(questContents)); // text quest change
    }

    public void QuestOver()
    {
        quest.PanelClose();
    }

    public void NewRule(string ruleContents, string imgName)
    {
        if (rule.gameObject.activeSelf == false) rule.gameObject.SetActive(true);
        rule.PanelOpen(FireStoreManager_Test_CM.Instance.ReadCSV(ruleContents), imgName);
    }

    public void RuleOver()
    {
        rule.PanelClose();
    }

    public void EODescPanelMade(double eoObj, double descPanel, string panelConentes, string detailContents)
    {
        int i = (int)eoObj;
        int j = (int)descPanel;
        Debug.Log(FireStoreManager_Test_CM.Instance.ReadCSV(panelConentes));
        Debug.Log(FireStoreManager_Test_CM.Instance.ReadCSV(detailContents));

        eoVar[i].transform.GetChild(j).GetComponent<EODescPanelTween_CM>().
        PanelOpen(FireStoreManager_Test_CM.Instance.ReadCSV(panelConentes), FireStoreManager_Test_CM.Instance.ReadCSV(detailContents));
    }

    public void SkipConv()
    {
        dsc.StopAllConversations();
    }

    public void HighlightRemote(double eoObj) // Highlight Remote
    {
        int i = (int)eoObj;

        for (int j = 0; j < eoVar[i].transform.childCount; j++)
        {
            HighlightPlus.HighlightEffect hlEffect = eoVar[i].transform.GetChild(j).GetComponent<HighlightPlus.HighlightEffect>();

            if (hlEffect != null)
            {
                hlEffect.highlighted = !hlEffect.highlighted;
            }
        }
    }

    public void EOObj3DText(double d)
    {
        int i = (int)d;

        for (int j = 0; j < eoVar[i].transform.childCount; j++)
        {
            if (eoVar[i].transform.GetChild(j).name == "3DText")
            {
                eoVar[i].transform.GetChild(j).gameObject.SetActive(!eoVar[i].transform.GetChild(j).gameObject.activeSelf);
            }
        }
    }

    public void GrabberTurn()
    {

    }

    IEnumerator WrondAnswerEffect()
    {
        scrFader.ChangeFadeImageColor(Color.red, 12f, 0.33f);
        scrFader.DoFadeIn();

        yield return new WaitForSeconds(0.75f);

        scrFader.DoFadeOut();
    }

    IEnumerator RemakeAfter2Sec(int i)
    {
        yield return new WaitForSeconds(2f);

        GameObject go = Instantiate(testBlocks[i]);
        go.GetComponent<BlockMoving_CM>().speed = 0f; // �� �����̰� ���ǵ�� 0���� �������ѳ���
        go.transform.position = testBlockSpawnpos[i].transform.position;
        go.transform.name = testBlocks[i].transform.name;

        if (i < 6) go.transform.rotation = Quaternion.Euler(0, -90, 0);
        else go.transform.rotation = Quaternion.Euler(0, 90, 0);
    }


    IEnumerator MoveScene()
    {
        scrFader.ChangeFadeImageColor(Color.black, 6f, 1f);
        scrFader.DoFadeIn();

        yield return new WaitForSeconds(0.75f);

        SceneManager.LoadScene(3);
    }


    #region Register with Lua
    private void OnEnable()
    {
        Lua.RegisterFunction("MaketutorialObj_Tail", this, SymbolExtensions.GetMethodInfo(() => MaketutorialObj_Tail()));
        Lua.RegisterFunction("MakeTutorialObj_Head", this, SymbolExtensions.GetMethodInfo(() => MakeTutorialObj_Head()));
        Lua.RegisterFunction("MakeTutorialObj_Single", this, SymbolExtensions.GetMethodInfo(() => MakeTutorialObj_Single()));
        Lua.RegisterFunction("MaketutorialObj_Double", this, SymbolExtensions.GetMethodInfo(() => MaketutorialObj_Double()));
        Lua.RegisterFunction("MakeTestBlocks", this, SymbolExtensions.GetMethodInfo(() => MakeTestBlocks((bool)false)));
        Lua.RegisterFunction("DeleteSaber", this, SymbolExtensions.GetMethodInfo(() => DeleteSaber()));
        Lua.RegisterFunction("IntroduceEyesOnlyObject", this, SymbolExtensions.GetMethodInfo(() => IntroduceEyesOnlyObject((double)0)));
        Lua.RegisterFunction("DestroyEyesOnlyObject", this, SymbolExtensions.GetMethodInfo(() => DestroyEyesOnlyObject((double)0)));
        Lua.RegisterFunction("MakePhosSticks", this, SymbolExtensions.GetMethodInfo(() => MakePhosSticks()));
        Lua.RegisterFunction("PlayAudioClip", this, SymbolExtensions.GetMethodInfo(() => PlayAudioClip((double)0)));
        Lua.RegisterFunction("QuestionByNarrator", this, SymbolExtensions.GetMethodInfo(() => QuestionByNarrator()));
        Lua.RegisterFunction("UILaserOnOff", this, SymbolExtensions.GetMethodInfo(() => UILaserOnOff()));
        Lua.RegisterFunction("MakePlayerMove", this, SymbolExtensions.GetMethodInfo(() => MakePlayerMove()));
        Lua.RegisterFunction("PlayClip", this, SymbolExtensions.GetMethodInfo(() => PlayClip((double)0)));
        Lua.RegisterFunction("FadeOutAndMoveScene", this, SymbolExtensions.GetMethodInfo(() => FadeOutAndMoveScene()));
        Lua.RegisterFunction("PlayNarratorAnimation", this, SymbolExtensions.GetMethodInfo(() => PlayNarratorAnimation((string)null)));
        Lua.RegisterFunction("NewQuest", this, SymbolExtensions.GetMethodInfo(() => NewQuest((string)null)));
        Lua.RegisterFunction("QuestOver", this, SymbolExtensions.GetMethodInfo(() => QuestOver()));
        Lua.RegisterFunction("NewRule", this, SymbolExtensions.GetMethodInfo(() => NewRule((string)null, (string)null)));
        Lua.RegisterFunction("RuleOver", this, SymbolExtensions.GetMethodInfo(() => RuleOver()));
        Lua.RegisterFunction("EODescPanelMade", this, SymbolExtensions.GetMethodInfo(() => EODescPanelMade((double)0, (double)0, (string)null, (string)null)));
        Lua.RegisterFunction("SkipConv", this, SymbolExtensions.GetMethodInfo(() => SkipConv()));
        Lua.RegisterFunction("HighlightRemote", this, SymbolExtensions.GetMethodInfo(() => HighlightRemote((double)0)));
        Lua.RegisterFunction("EOObj3DText", this, SymbolExtensions.GetMethodInfo(() => EOObj3DText((double)0)));
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction("MaketutorialObj_Tail");
        Lua.UnregisterFunction("MakeTutorialObj_Head");
        Lua.UnregisterFunction("MakeTutorialObj_Single");
        Lua.UnregisterFunction("MaketutorialObj_Double");
        Lua.UnregisterFunction("MakeTestBlocks");
        Lua.UnregisterFunction("DeleteSaber");
        Lua.UnregisterFunction("IntroduceEyesOnlyObject");
        Lua.UnregisterFunction("DestroyEyesOnlyObject");
        Lua.UnregisterFunction("MakePhosSticks");
        Lua.UnregisterFunction("PlayAudioClip");
        Lua.UnregisterFunction("QuestionByNarrator");
        Lua.UnregisterFunction("UILaserOnOff");
        Lua.UnregisterFunction("MakePlayerMove");
        Lua.UnregisterFunction("PlayClip");
        Lua.UnregisterFunction("FadeOutAndMoveScene");
        Lua.UnregisterFunction("PlayNarratorAnimation");
        Lua.UnregisterFunction("NewQuest");
        Lua.UnregisterFunction("QuestOver");
        Lua.UnregisterFunction("NewRule");
        Lua.UnregisterFunction("RuleOver");
        Lua.UnregisterFunction("EODescPanelMade");
        Lua.UnregisterFunction("SkipConv");
        Lua.UnregisterFunction("HighlightRemote");
        Lua.UnregisterFunction("EOObj3DText");
    }
    #endregion
}
