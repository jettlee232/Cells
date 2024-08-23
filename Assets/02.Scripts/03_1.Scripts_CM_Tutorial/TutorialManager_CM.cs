using System.Collections;
using System;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEditor;
using UnityEngine.XR;
using Language.Lua;
using UnityEngine.SceneManagement;
using TMPro;
using HighlightPlus;
using PixelCrushers;

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
    public AudioSource audioSrc;
    public AudioClip[] audioClipsArr;
    public string audioFilePath;
    public bool isInzizilMakeIt = true;
    public int conv2Cnt = 0;
    public GameObject quizCanvas;
    public Animator anim;
    private bool nextFlag = false;
    public bool allComplete = false;

    [Header("Laser")]
    public BNG.UIPointer uIPointer;
    public LineRenderer lineRenderer;
    public LPAD_CM lpd;
    //public LaserPointerAndDescription_CM lpd;

    [Header("Player Variables")]
    public BNG.MyFader_CM scrFader;
    public BNG.SmoothLocomotion smoothLocomotion;

    [Header("Eyes Only Obj")]
    public GameObject[] eoVar = new GameObject[3];
    public GameObject[] eoObj = new GameObject[3];
    public Transform[] eoSpawnPos = new Transform[3];

    [Header("Phos Sticks")]
    private GameObject[] phosStickVar = new GameObject[2];
    public GameObject[] phosSticks;
    private int phosGrabCnt = 0;

    [Header("Make Effect")] // ����Ʈ ����
    public GameObject[] makeEffect;

    [Header("Panels")]
    public QuestPanel_CM quest;
    public RulePanel_CM rule;
    public LocationPanel_CM loc;
    public SpeechBubblePanel_CM spb;

    [Header("Grabbale")]
    public BNG.Grabber grab1;
    public BNG.Grabber grab2;

    [Header("3D Texts")]
    public GameObject[] texts;

    [Header("Follwing Description Prefabs")]
    public GameObject[] followingPanels;
    public Transform[] followPanelPos;
    public GameObject followPanelParticlePrefab;
    public ParticleSystem followPanelParticleSys;

    [Header("Tooltip")]
    public Tooltip[] tooltips;
    public GameObject[] worldTooltips;

    void Start()
    {
        audioClipsArr = Resources.LoadAll<AudioClip>("CM_Tuto");
        isInzizilMakeIt = true;
        quizCanvas.SetActive(false);

        LocPanel("세포막"); // 나중에 수정해야 됨               

        InitFollowPanelParticle();
        //UILaserOnOff();

        //quest.gameObject.SetActive(false);
        //rule.gameObject.SetActive(false);
    }

    public void InitFollowPanelParticle()
    {
        GameObject go = Instantiate(followPanelParticlePrefab);
        go.transform.position = followPanelPos[0].position;
        go.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        followPanelParticleSys = go.GetComponent<ParticleSystem>();
        followPanelParticleSys.Stop();
    }    

    public void LocPanel(string location)
    {
        loc.PanelOpen(location);
    }

    public void UILaserOnOff(bool flag)
    {
        lineRenderer.enabled = flag;
        uIPointer.enabled = flag;
    }

    public void QuestionByNarrator()
    {
        //quest.gameObject.SetActive(false);
        //rule.gameObject.SetActive(false);

        quizCanvas.SetActive(true);
        quizCanvas.transform.GetChild(0).GetComponent<InstantiateTween2_CM>().DoingTween();

        //quizCanvas.transform.GetChild(0).GetChild(1).GetChild(1).Find("Text").GetComponent<TextMeshProUGUI>().text = FireStoreManager_Test_CM.Instance.ReadCSV("Quiz_CM_1");

        //quizCanvas.transform.GetChild(0).GetChild(1).GetChild(1).Find("O").GetChild(0).GetComponent<TextMeshProUGUI>().text = FireStoreManager_Test_CM.Instance.ReadCSV("Quiz_CM_2");
        //quizCanvas.transform.GetChild(0).GetChild(1).GetChild(1).Find("X").GetChild(0).GetComponent<TextMeshProUGUI>().text = FireStoreManager_Test_CM.Instance.ReadCSV("Quiz_CM_3");
    }

    public void MaketutorialObj_Tail()
    {
        saberVar = Instantiate(tutorialObj_Tail, spawnPos_Tail.position, Quaternion.identity);
        saberVar.name = tutorialObj_Tail.name;
        MakeEffect(saberVar.transform, 0);
        AudioMgr_CM.Instance.PlaySFXByInt(9);

        for (int i = 0; i < saberVar.transform.childCount; i++)
        {
            // Must Refactoring
            if (saberVar.transform.GetChild(i).GetComponent<Canvas>() != null && saberVar.transform.GetChild(i).GetComponent<Tooltip_CM>() == null)
            {
                Transform canvas = saberVar.transform.GetChild(i);
                canvas.GetChild(0).Find("Title").GetComponent<TextMeshProUGUI>().text =
                    FireStoreManager_Test_CM.Instance.ReadCSV("Phospholipid_Tail_CM");

                //canvas.GetChild(0).Find("Detail").GetComponent<TextMeshProUGUI>().text = FireStoreManager_Test_CM.Instance.ReadCSV("Phospholipid_Tail_CM_D");
            }
        }
    }

    public void MakeTutorialObj_Head()
    {
        GameObject go = Instantiate(tutorialObj_Head, spawnPos_Head.position, Quaternion.identity);
        go.name = tutorialObj_Head.name;
        MakeEffect(go.transform, 0);
        AudioMgr_CM.Instance.PlaySFXByInt(9);

        for (int i = 0; i < go.transform.childCount; i++)
        {
            if (go.transform.GetChild(i).GetComponent<Canvas>() != null)
            {
                Transform canvas = go.transform.GetChild(i);
                canvas.GetChild(0).Find("Title").GetComponent<TextMeshProUGUI>().text =
                    FireStoreManager_Test_CM.Instance.ReadCSV("Phospholipid_Head_CM");

                //canvas.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = FireStoreManager_Test_CM.Instance.ReadCSV("Phospholipid_Head_CM_D");
            }
        }
    }

    // -> After Second Conv
    public void MakeTutorialObj_Single()
    {
        GameObject go = Instantiate(tutorialObj_Single, spawnPos_Single.position, Quaternion.Euler(0, 0, 0));
        go.name = tutorialObj_Single.name;
        MakeEffect(go.transform, 0);
        AudioMgr_CM.Instance.PlaySFXByInt(9);

        for (int i = 0; i < go.transform.childCount; i++)
        {
            if (go.transform.GetChild(i).GetComponent<Canvas>() != null)
            {
                Transform canvas = go.transform.GetChild(i);
                canvas.GetChild(0).Find("Title").GetComponent<TextMeshProUGUI>().text =
                    FireStoreManager_Test_CM.Instance.ReadCSV("Phospholipid_Single_CM");
                //canvas.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = FireStoreManager_Test_CM.Instance.ReadCSV("Phospholipid_Single_CM_D");
            }
        }
    }

    // After Third Conv
    public void MaketutorialObj_Double()
    {
        GameObject go = Instantiate(tutorialObj_Double, spawnPos_Double.position, Quaternion.Euler(90, 90, 0));
        go.name = tutorialObj_Double.name;
        MakeEffect(go.transform, 0);
        AudioMgr_CM.Instance.PlaySFXByInt(9);

        for (int i = 0; i < go.transform.childCount; i++)
        {
            if (go.transform.GetChild(i).GetComponent<Canvas>() != null)
            {
                Transform canvas = go.transform.GetChild(i);
                canvas.GetChild(0).Find("Title").GetComponent<TextMeshProUGUI>().text =
                    FireStoreManager_Test_CM.Instance.ReadCSV("Single_EO");
                //canvas.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text =  FireStoreManager_Test_CM.Instance.ReadCSV("Phospholipid_Double_CM_D");
            }
        }
    }

    // -> After Fourth Conv
    public void MakeTestBlocks(bool philicOrPhobic)
    {
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

        AudioMgr_CM.Instance.PlaySFXByInt(7);
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


        AudioMgr_CM.Instance.PlaySFXByInt(0);
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
        StartCoroutine(RightAnswerEffect());


        int correctAnsCnt = 0;
        for (int i = 0; i < testBlocks.Length; i++)
        {
            if (testBlocks[i].name == go.name)
            {
                correctAnss[i] = true;      
                StartCoroutine(RemakeAfter2Sec(i));
            }

            if (correctAnss[i] == true)
            {
                correctAnsCnt++;
                TooltipTextChng_Answer(0, correctAnsCnt);
                TooltipTextChng_Answer(1, correctAnsCnt);
            }
        }

        if (correctAnsCnt > 6)
        {
            isInzizilMakeIt = false;
            narrator.StartCov_6();

            phosStickVar[0].transform.GetComponent<BNG.Grabbable>().BeingHeld = false;
            phosStickVar[1].transform.GetComponent<BNG.Grabbable>().BeingHeld = false;
            phosStickVar[0].transform.GetComponent<BNG.Grabbable>().CanBeDropped = true;
            phosStickVar[1].transform.GetComponent<BNG.Grabbable>().CanBeDropped = true;
            phosStickVar[0].transform.GetComponent<BNG.Grabbable>().ParentToHands = true;
            phosStickVar[1].transform.GetComponent<BNG.Grabbable>().ParentToHands = true;
            phosStickVar[0].SetActive(false);
            phosStickVar[1].SetActive(false);
        }
    }

    public void WrongAnswer(GameObject go)
    {
        StartCoroutine(WrondAnswerEffect());

        for (int i = 0; i < testBlocks.Length; i++)
        {
            if (testBlocks[i].name == go.name)
            {
                StartCoroutine(RemakeAfter2Sec(i));
            }
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
        AudioMgr_CM.Instance.PlaySFXByInt(7);
    }

    public void DestroyEyesOnlyObject(double num)
    {
        int i = (int)num;
        MakeEffect(eoVar[i].transform, 0);
        eoVar[i].SetActive(!eoVar[i].activeSelf);

        AudioMgr_CM.Instance.PlaySFXByInt(0);
    }

    public void MakePhosSticks()
    {
        phosStickVar[0] = Instantiate(phosSticks[0], spawnPos_Tail.position, Quaternion.identity);
        phosStickVar[0].name = phosSticks[0].name;
        MakeEffect(phosStickVar[0].transform, 0);
        phosStickVar[1] = Instantiate(phosSticks[1], spawnPos_Head.position, Quaternion.identity);
        phosStickVar[1].name = phosSticks[1].name;
        MakeEffect(phosStickVar[1].transform, 0);

        AudioMgr_CM.Instance.PlaySFXByInt(9);
    }

    void MakeEffect(Transform pos, int i)
    {
        Instantiate(makeEffect[0], pos.position, Quaternion.identity);
    }

    public void FadeOutAndMoveScene()
    {
        StartCoroutine(MoveScene());
    }

    public void MakePlayerMove(bool flag)
    {
        smoothLocomotion.enabled = flag;
    }

    public void PlayNarratorAnimation(string animName)
    {
        //string animStr = "Base Layer." + animName;
        //anim.Play(animStr);
        anim.Play(animName);
    }

    public void NewQuest(string questContents, double exitTime, bool useFSMorN)
    {
        if (quest.transform.GetChild(0).gameObject.activeSelf == false) quest.transform.GetChild(0).gameObject.SetActive(true);

        if (useFSMorN == true)
        {
            quest.PanelOpen(FireStoreManager_Test_CM.Instance.ReadCSV(questContents), (float)exitTime); // text quest change
        }
        else
        {
            quest.PanelOpen(questContents, (float)exitTime); // text quest change
        }
        
        PlayClip(8);
    }

    public void QuestOver()
    {
        Debug.Log("Quest Over By  : " + gameObject.name);
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
        Debug.Log(FireStoreManager_Test_CM.Instance.ReadCSV(panelConentes));
        Debug.Log(FireStoreManager_Test_CM.Instance.ReadCSV(detailContents));

        eoVar[(int)eoObj].transform.GetChild((int)descPanel).GetComponent<EODescPanelTween_CM>().
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

    public void Showing3DTexts(bool thing)
    {
        if (!thing)
        {
            texts[0].SetActive(true);
            MakeEffect(texts[0].transform, 0);
            AudioMgr_CM.Instance.PlaySFXByInt(0);
        }
        else
        {
            texts[1].SetActive(true);
            MakeEffect(texts[1].transform, 0);
            AudioMgr_CM.Instance.PlaySFXByInt(0);
        }
    }

    public void CheckHighlightCount()
    {
        int cnt = 0;

        if (nextFlag == false)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                if (texts[i].activeSelf == true)
                {
                    if (texts[i].GetComponent<HighlighCount_CM>().enabled == true)
                    {
                        if (texts[i].GetComponent<HighlighCount_CM>().ReturnFlag() == false)
                        {
                            cnt++;
                        }
                    }
                }
            }

            if (cnt >= texts.Length)
            {
                nextFlag = true;
                //StartCoroutine(StartConvWithDelay()); // 일단은 주석처리 <- 나중에 아예 지워야 될 것 같음

                worldTooltips[0].SetActive(true);
                worldTooltips[0].GetComponent<Tooltip>().TooltipOn("우리도 확인해줘!");
                worldTooltips[1].SetActive(true);
                worldTooltips[1].GetComponent<Tooltip>().TooltipOn("우리도 확인해줘!");
                worldTooltips[2].SetActive(true);
                worldTooltips[2].GetComponent<SpeechBubblePanel_CM>().PanelOpen("충분히 다 확인했으면 나한테 말을 걸어줘!");
                tooltips[1].TooltipTextChange("A버튼을 NPC에게 쏘면 말을 걸 수 있어요!");

                allComplete = true;
            }
        }
    }

    public void EnabledHLC()
    {
        texts[0].GetComponent<HighlighCount_CM>().enabled = true;
        texts[1].GetComponent<HighlighCount_CM>().enabled = true;
    }

    public void HighlightTexts()
    {
        tooltips[0].ChangeSprite(0);
        tooltips[1].ChangeSprite(0);

        StartCoroutine(SmallTextsHighlighted());
    }

    public void LPDAOnOff(bool flag)
    {
        lpd.RayStateChange(flag);
    }

    public void PhosGrabCnt()
    {
        phosGrabCnt++;
        if (phosGrabCnt >= 2)
        {
            NewFollow(4, 0);

            TooltipTextChng_Answer(0, 0);          
            TooltipTextChng_Answer(1, 0);

            tooltips[0].ChangeSprite(1);
            tooltips[1].ChangeSprite(1);

            tooltips[0].UnShowingTooltipAnims();
            tooltips[1].UnShowingTooltipAnims();

            if (quest.transform.gameObject.activeSelf == false) quest.transform.gameObject.SetActive(true);
            NewQuest("중앙에 있는 게임 규칙을 읽어보자!", 3f, false);
        }
    }

    public void NewSpeech(string speechContents) // Need Convert to Lua
    {
        if (spb.gameObject.activeSelf == false) spb.gameObject.SetActive(true);
        //spb.PanelOpen(speechContents);
        spb.PanelOpen(speechContents, 3f);
        //spb.PanelOpen(FireStoreManager_Test_CM.Instance.ReadCSV(speechContents)); // text quest change
    }

    public void SpeechOver() // Need Convert to Lua
    {
        spb.PanelClose();
    }

    public void NewFollow(double panelIndex, double followPanelIndex)
    {
        FollowDelete(followPanelIndex);

        GameObject go = Instantiate(followingPanels[(int)panelIndex]);
        go.transform.SetParent(followPanelPos[(int)followPanelIndex]);
        go.transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);

        followPanelParticleSys.Play();
    }

    public void FollowDelete(double followPanelIndex)
    {
        for (int i = 0; i < followPanelPos[(int)followPanelIndex].childCount; i++)
        {
            FollowPanel_CM var1 = followPanelPos[(int)followPanelIndex].GetChild(i).GetComponent<FollowPanel_CM>();
            LaserDescriptionTween_CM var2 = followPanelPos[(int)followPanelIndex].GetChild(i).GetComponent<LaserDescriptionTween_CM>();

            if (var1 != null)
            {
                var1.ReverseTweenAndDestroy();
            }
            else if (var2 != null)
            {
                var2.ReverseTweenAndDestroy();
            }
        }
    }

    public void NewTooltip(double index, string content)
    {
        tooltips[(int)index].gameObject.SetActive(true);
        tooltips[(int)index].TooltipOn(content);
    }

    public void TooltipOver(double index)
    {
        tooltips[(int)index].TooltipOff();
    }

    void TooltipTextChng_Answer(double index, int ansCnt)
    {
        string content = "정답 개수 : " + ansCnt + " / 7"; 

        tooltips[(int)index].TooltipTextChange(content);
    }

    public void AllCompleteAndMoveToNextScene()
    {
        worldTooltips[0].GetComponent<Tooltip>().TooltipOff();
        worldTooltips[1].GetComponent<Tooltip>().TooltipOff();
        worldTooltips[2].GetComponent<SpeechBubblePanel_CM>().PanelClose();
        tooltips[1].TooltipOff();
    }

    public void ChangeTooltipSprite(double hand, double spriteIndex)
    {
        tooltips[(int)hand].ChangeSprite((int)spriteIndex);
    }

    public void ShowingTooltipAnim(double hand, double anim) { tooltips[(int)hand].ShowingTooltipAnims((int)anim); }
    public void UnShowingTooltipAnim(double hand) { tooltips[(int)hand].UnShowingTooltipAnims(); }

    IEnumerator SmallTextsHighlighted()
    {
        for (int i = 0; i < texts.Length; i++) texts[i].GetComponent<HighlightEffect>().highlighted = true;

        yield return new WaitForSeconds(3f);

        for (int i = 0; i < texts.Length; i++) texts[i].GetComponent<HighlightEffect>().highlighted = false;
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

    IEnumerator RemakeAfter2Sec(int i)
    {
        yield return new WaitForSeconds(2f);

        GameObject go = Instantiate(testBlocks[i]);
        go.GetComponent<BlockMoving_CM>().speed = 0f; // �� �����̰� ���ǵ�� 0���� �������ѳ���
        go.transform.position = testBlockSpawnpos[i].transform.position;
        go.transform.name = testBlocks[i].transform.name;

        if (i < 6) go.transform.rotation = Quaternion.Euler(0, -45, 0);
        else go.transform.rotation = Quaternion.Euler(0, 45, 0);
    }

    IEnumerator StartConvWithDelay()
    {
        yield return new WaitForSeconds(0.5f);
        narrator.StartCov_7();
    }

    IEnumerator MoveScene()
    {
        scrFader.ChangeFadeImageColor(Color.black, 6f, 1f);
        scrFader.DoFadeIn();

        yield return new WaitForSeconds(0.75f);

        SceneManager.LoadScene(4);
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
        Lua.RegisterFunction("UILaserOnOff", this, SymbolExtensions.GetMethodInfo(() => UILaserOnOff((bool)false)));
        Lua.RegisterFunction("MakePlayerMove", this, SymbolExtensions.GetMethodInfo(() => MakePlayerMove((bool)false)));
        Lua.RegisterFunction("PlayClip", this, SymbolExtensions.GetMethodInfo(() => PlayClip((double)0)));
        Lua.RegisterFunction("FadeOutAndMoveScene", this, SymbolExtensions.GetMethodInfo(() => FadeOutAndMoveScene()));
        Lua.RegisterFunction("PlayNarratorAnimation", this, SymbolExtensions.GetMethodInfo(() => PlayNarratorAnimation((string)null)));
        Lua.RegisterFunction("NewQuest", this, SymbolExtensions.GetMethodInfo(() => NewQuest((string)null, (double)0, (bool)false)));
        Lua.RegisterFunction("QuestOver", this, SymbolExtensions.GetMethodInfo(() => QuestOver()));
        Lua.RegisterFunction("NewRule", this, SymbolExtensions.GetMethodInfo(() => NewRule((string)null, (string)null)));
        Lua.RegisterFunction("RuleOver", this, SymbolExtensions.GetMethodInfo(() => RuleOver()));
        Lua.RegisterFunction("EODescPanelMade", this, SymbolExtensions.GetMethodInfo(() => EODescPanelMade((double)0, (double)0, (string)null, (string)null)));
        Lua.RegisterFunction("SkipConv", this, SymbolExtensions.GetMethodInfo(() => SkipConv()));
        Lua.RegisterFunction("HighlightRemote", this, SymbolExtensions.GetMethodInfo(() => HighlightRemote((double)0)));
        Lua.RegisterFunction("EOObj3DText", this, SymbolExtensions.GetMethodInfo(() => EOObj3DText((double)0)));
        Lua.RegisterFunction("Showing3DTexts", this, SymbolExtensions.GetMethodInfo(() => Showing3DTexts((bool)false)));
        Lua.RegisterFunction("EnabledHLC", this, SymbolExtensions.GetMethodInfo(() => EnabledHLC()));
        Lua.RegisterFunction("HighlightTexts", this, SymbolExtensions.GetMethodInfo(() => HighlightTexts()));
        Lua.RegisterFunction("LPDAOnOff", this, SymbolExtensions.GetMethodInfo(() => LPDAOnOff((bool)false)));
        Lua.RegisterFunction("NewFollow", this, SymbolExtensions.GetMethodInfo(() => NewFollow((double)0, (double)0)));
        Lua.RegisterFunction("FollowDelete", this, SymbolExtensions.GetMethodInfo(() => FollowDelete((double)0)));
        Lua.RegisterFunction("NewTooltip", this, SymbolExtensions.GetMethodInfo(() => NewTooltip((double)0, (string)null)));
        Lua.RegisterFunction("TooltipOver", this, SymbolExtensions.GetMethodInfo(() => TooltipOver((double)0)));
        Lua.RegisterFunction("ShowingTooltipAnim", this, SymbolExtensions.GetMethodInfo(() => ShowingTooltipAnim((double)0, (double)0)));
        Lua.RegisterFunction("UnShowingTooltipAnim", this, SymbolExtensions.GetMethodInfo(() => UnShowingTooltipAnim((double)0)));
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
        Lua.UnregisterFunction("Showing3DTexts");
        Lua.UnregisterFunction("EnabledHLC");
        Lua.UnregisterFunction("HighlightTexts");
        Lua.UnregisterFunction("LPDAOnOff");
        Lua.UnregisterFunction("NewFollow");
        Lua.UnregisterFunction("FollowDelete");
        Lua.UnregisterFunction("NewTooltip");
        Lua.UnregisterFunction("TooltipOver");
        Lua.UnregisterFunction("ShowingTooltipAnim");
        Lua.UnregisterFunction("UnShowingTooltipAnim");
    }
    #endregion
}
