using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.Networking;
using UnityEditor;
using UnityEngine.XR;
using Language.Lua;
using BNG;

public class TutorialManager_CM : MonoBehaviour
{
    [Header("Test Block Variable")]
    public GameObject[] testBlocks; // 12���� �׽�Ʈ���
    public Transform[] testBlockSpawnpos; // 12���� �׽�Ʈ��� ��ġ ��ġ
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

    [Header("Laser")]
    public BNG.UIPointer uIPointer;
    public LineRenderer lineRenderer;

    void Start()
    {
        //MakeTestBlocks();

        audioClipsArr = Resources.LoadAll<AudioClip>("");
        isInzizilMakeIt = true;
        quizCanvas.SetActive(false);

        uIPointer.enabled = true;
    }

    public void UILaserOnOff()
    {
        //uIPointer.enabled = !uIPointer.enabled;
        //lineRenderer.enabled = false;
    }

    public void QuestionByNarrator()
    {        
        quizCanvas.SetActive(true);
    }

    // ������ ������ �Ӹ� ����� -> After First Encounter
    public void MaketutorialObj_Tail()
    {
        saberVar = Instantiate(tutorialObj_Tail, spawnPos_Tail.position, Quaternion.identity);
        saberVar.name = tutorialObj_Tail.name;
    }

    public void MakeTutorialObj_Head()
    {
        GameObject go = Instantiate(tutorialObj_Head, spawnPos_Head.position, Quaternion.identity);
        go.name = tutorialObj_Head.name;
    }

    // ������ ������ �Ӹ� ��ġ�� -> After Second Conv
    public void MakeTutorialObj_Single()
    {
        GameObject go = Instantiate(tutorialObj_Single, spawnPos_Single.position, Quaternion.Euler(0, 0, 0));
        go.name = tutorialObj_Single.name;
    }

    // ������ ������ ���� ����� �ϳ� �� ����� -> After Third Conv
    public void MaketutorialObj_Double()
    {
        GameObject go = Instantiate(tutorialObj_Double, spawnPos_Double.position, Quaternion.Euler(90, 90, 0));
        go.name = tutorialObj_Double.name;
    }    

    // �׽�Ʈ ��ϵ� �� ��ġ�ϱ� -> After Fourth Conv
    public void MakeTestBlocks() // �׽�Ʈ ��ϵ� �� ��ġ�ϱ�
    {
        correctAnss = new bool[testBlocks.Length];

        for (int i = 0; i < testBlocks.Length; i++)
        {
            GameObject go = Instantiate(testBlocks[i]);
            go.GetComponent<BlockMoving_CM>().speed = 0f; // �� �����̰� ���ǵ�� 0���� �������ѳ���
            go.transform.position = testBlockSpawnpos[i].transform.position;
            go.transform.name = testBlocks[i].transform.name;

            if (i < 6) go.transform.rotation = Quaternion.Euler(0, -90, 0);
            else go.transform.rotation = Quaternion.Euler(0, 90, 0);

            correctAnss[i] = false;
        }
    }

    public void DeleteSaber()
    {
        Destroy(saberVar);
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

    // (�Ʒÿ�)���̹��� ���� ����� � ������� �˾Ƴ� ����, �����̸� �� ����� �����ϰ� ��� �� �ٽ� ����
    public void CorrectAnswer(GameObject go)
    {
        int rnd = Random.Range(0, 4);
        if (rnd == 0) PlayAudioClip(7);
        else if (rnd == 1) PlayAudioClip(14);
        else if (rnd == 2) PlayAudioClip(18);
        else if (rnd == 3) PlayAudioClip(28);

        int correctAnsCnt = 0;
        for (int i = 0; i < testBlocks.Length; i++) // (�Ʒÿ�)���̹��� ���� ����� � ������� �˾Ƴ���
        {
            if (testBlocks[i].name == go.name) // � ������� ã�Ƴ�����
            {
                correctAnss[i] = true; // �ش� ��ϰ� ������ ��ȣ�� ĭ�� ���� ó��                
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

    // (�Ʒÿ�) ���̹��� ���� ����� � ������� �˾Ƴ� ����, �����̸� �� ����� �����ϰ� ��� �� �ٽ� ����
    public void WrongAnswer(GameObject go)
    {
        int rnd = Random.Range(0, 2);
        if (rnd == 0) PlayAudioClip(34);
        else if (rnd == 1) PlayAudioClip(35);

        for (int i = 0; i < testBlocks.Length; i++) // (�Ʒÿ�)���̹��� ���� ����� � ������� �˾Ƴ���
        {
            if (testBlocks[i].name == go.name) // � ������� ã�Ƴ�����
            {
                StartCoroutine(RemakeAfter2Sec(i));
            }

            // ������ ����� ���μ��� ����
        }
    }

    public void PlayClip(double d)
    {
        // ���߿� ����
        //AudioMgr_CM.Instance.PlaySFX((int)d);
    }

    IEnumerator RemakeAfter2Sec(int i)
    {
        yield return new WaitForSeconds(2f);

        Debug.Log("�ڷ�ƾ ����, ��� �����");
            
        GameObject go = Instantiate(testBlocks[i]);
        go.GetComponent<BlockMoving_CM>().speed = 0f; // �� �����̰� ���ǵ�� 0���� �������ѳ���
        go.transform.position = testBlockSpawnpos[i].transform.position;
        go.transform.name = testBlocks[i].transform.name;

        if (i < 6) go.transform.rotation = Quaternion.Euler(0, -90, 0);
        else go.transform.rotation = Quaternion.Euler(0, 90, 0);
    }


    #region Register with Lua
    private void OnEnable()
    {
        Lua.RegisterFunction("MaketutorialObj_Tail", this, SymbolExtensions.GetMethodInfo(() => MaketutorialObj_Tail()));
        Lua.RegisterFunction("MakeTutorialObj_Head", this, SymbolExtensions.GetMethodInfo(() => MakeTutorialObj_Head()));
        Lua.RegisterFunction("MakeTutorialObj_Single", this, SymbolExtensions.GetMethodInfo(() => MakeTutorialObj_Single()));
        Lua.RegisterFunction("MaketutorialObj_Double", this, SymbolExtensions.GetMethodInfo(() => MaketutorialObj_Double()));
        Lua.RegisterFunction("MakeTestBlocks", this, SymbolExtensions.GetMethodInfo(() => MakeTestBlocks()));
        Lua.RegisterFunction("DeleteSaber", this, SymbolExtensions.GetMethodInfo(() => DeleteSaber()));
        Lua.RegisterFunction("PlayAudioClip", this, SymbolExtensions.GetMethodInfo(() => PlayAudioClip((double)0)));
        Lua.RegisterFunction("QuestionByNarrator", this, SymbolExtensions.GetMethodInfo(() => QuestionByNarrator()));
        Lua.RegisterFunction("UILaserOnOff", this, SymbolExtensions.GetMethodInfo(() => UILaserOnOff()));

        // ���� �߰��� ��
        Lua.RegisterFunction("PlayClip", this, SymbolExtensions.GetMethodInfo(() => PlayClip((double)0)));

    }

    private void OnDisable()
    {
        Lua.UnregisterFunction("MaketutorialObj_Tail");
        Lua.UnregisterFunction("MakeTutorialObj_Head");
        Lua.UnregisterFunction("MakeTutorialObj_Single");
        Lua.UnregisterFunction("MaketutorialObj_Double");
        Lua.UnregisterFunction("MakeTestBlocks");
        Lua.UnregisterFunction("DeleteSaber");
        Lua.UnregisterFunction("PlayAudioClip");
        Lua.UnregisterFunction("QuestionByNarrator");
        Lua.UnregisterFunction("UILaserOnOff");

        // ���� �߰��� ��
        Lua.UnregisterFunction("PlayClip");
    }
    #endregion
}
