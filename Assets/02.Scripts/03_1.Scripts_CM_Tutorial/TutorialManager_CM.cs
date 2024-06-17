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
    public GameObject[] testBlocks; // 12개의 테스트블록
    public Transform[] testBlockSpawnpos; // 12개의 테스트블록 배치 위치
    public int curCorrectAns; // 현재 정답 개수
    public int correctAns; // 정답 개수
    public bool[] correctAnss; // 정답 개수 체크

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

    // 인지질 막대기랑 머리 만들기 -> After First Encounter
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

    // 인지질 막대기랑 머리 합치기 -> After Second Conv
    public void MakeTutorialObj_Single()
    {
        GameObject go = Instantiate(tutorialObj_Single, spawnPos_Single.position, Quaternion.Euler(0, 0, 0));
        go.name = tutorialObj_Single.name;
    }

    // 인지질 이중층 구조 만들게 하나 더 만들기 -> After Third Conv
    public void MaketutorialObj_Double()
    {
        GameObject go = Instantiate(tutorialObj_Double, spawnPos_Double.position, Quaternion.Euler(90, 90, 0));
        go.name = tutorialObj_Double.name;
    }    

    // 테스트 블록들 다 배치하기 -> After Fourth Conv
    public void MakeTestBlocks() // 테스트 블록들 다 배치하기
    {
        correctAnss = new bool[testBlocks.Length];

        for (int i = 0; i < testBlocks.Length; i++)
        {
            GameObject go = Instantiate(testBlocks[i]);
            go.GetComponent<BlockMoving_CM>().speed = 0f; // 못 움직이게 스피드는 0으로 고정시켜놓기
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

    // (훈련용)세이버에 닿은 블록이 어떤 블록인지 알아낸 다음, 정답이면 그 블록을 삭제하고 잠시 후 다시 생성
    public void CorrectAnswer(GameObject go)
    {
        int rnd = Random.Range(0, 4);
        if (rnd == 0) PlayAudioClip(7);
        else if (rnd == 1) PlayAudioClip(14);
        else if (rnd == 2) PlayAudioClip(18);
        else if (rnd == 3) PlayAudioClip(28);

        int correctAnsCnt = 0;
        for (int i = 0; i < testBlocks.Length; i++) // (훈련용)세이버에 닿은 블록이 어떤 블록인지 알아내기
        {
            if (testBlocks[i].name == go.name) // 어떤 블록인지 찾아냈으면
            {
                correctAnss[i] = true; // 해당 블록과 동일한 번호의 칸에 정답 처리                
                StartCoroutine(RemakeAfter2Sec(i));                
            }

            if (correctAnss[i] == true) // 지금 까지 맞춘 정답 개수 세기
            {
                correctAnsCnt++;
            }
        }

        if (correctAnsCnt > 6) // 정답 개수가 6개가 넘어가면
        {
            // 다음 스텝으로 이동
            isInzizilMakeIt = false;
            narrator.StartCov_6(); // 마지막 ㅇㅇ
        }
    }

    // (훈련용) 세이버에 닿은 블록이 어떤 블록인지 알아낸 다음, 오답이면 그 블록을 삭제하고 잠시 후 다시 생성
    public void WrongAnswer(GameObject go)
    {
        int rnd = Random.Range(0, 2);
        if (rnd == 0) PlayAudioClip(34);
        else if (rnd == 1) PlayAudioClip(35);

        for (int i = 0; i < testBlocks.Length; i++) // (훈련용)세이버에 닿은 블록이 어떤 블록인지 알아내기
        {
            if (testBlocks[i].name == go.name) // 어떤 블록인지 찾아냈으면
            {
                StartCoroutine(RemakeAfter2Sec(i));
            }

            // 오답일 경우의 프로세스 실행
        }
    }

    public void PlayClip(double d)
    {
        // 나중에 ㄱㄱ
        //AudioMgr_CM.Instance.PlaySFX((int)d);
    }

    IEnumerator RemakeAfter2Sec(int i)
    {
        yield return new WaitForSeconds(2f);

        Debug.Log("코루틴 종료, 블록 재생성");
            
        GameObject go = Instantiate(testBlocks[i]);
        go.GetComponent<BlockMoving_CM>().speed = 0f; // 못 움직이게 스피드는 0으로 고정시켜놓기
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

        // 새로 추가한 거
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

        // 새로 추가한 거
        Lua.UnregisterFunction("PlayClip");
    }
    #endregion
}
