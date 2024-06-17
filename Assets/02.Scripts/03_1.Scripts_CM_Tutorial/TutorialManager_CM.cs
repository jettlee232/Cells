using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using System.ComponentModel;

public class TutorialManager_CM : MonoBehaviour
{
    [Header("Test Block Variable")]
    public GameObject[] testBlocks; // 12개의 테스트블록
    //public GameObject[] testBlocksVar; // 12개의 테스트블록
    public Transform[] testBlockSpawnpos; // 12개의 테스트블록 배치 위치
    public int curCorrectAns; // 현재 정답 개수
    public int correctAns; // 정답 개수
    public bool[] correctAnss; // 정답 개수 체크

    [Header("Test Saber Variable")]
    public Transform testSaberSSSpawnPos;
    public Transform testSaberWFSpawnPos;
    public GameObject testSaberSS;
    public GameObject testHeadWF;
    public GameObject testSaberSSWF;
    public GameObject testSaberSSWFComplete;
    public GameObject saberVar;

    [Header("Narrator Mgr")]
    public NarratorDialogueHub_CM_Tutorial narrator;
    public DialogueSystemController dsc;


    void Start()
    {
        //MakeTestBlocks();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            
        }
    }


    // 인지질 막대기랑 머리 만들기 -> After First Encounter
    public void MakeTestSaberSS()
    {
        saberVar = Instantiate(testSaberSS, testSaberSSSpawnPos.position, Quaternion.identity);        
    }

    public void MakeTestHeadWF()
    {
        Instantiate(testHeadWF, testSaberWFSpawnPos.position, Quaternion.identity);
    }

    // 인지질 막대기랑 머리 합치기 -> After Second Conv
    public void MakeTestSaberSSWF()
    {
        Instantiate(testSaberSSWF, testSaberSSSpawnPos.position, Quaternion.Euler(90, 0, 0));
    }

    // 인지질 이중층 구조 만들게 하나 더 만들기 -> After Third Conv
    public void MakeTestSSWFComplete()
    {
        Instantiate(testSaberSSWFComplete, testSaberWFSpawnPos.position, Quaternion.Euler(0, 0, 0));
    }    

    // 테스트 블록들 다 배치하기 -> After Fourth Conv
    public void MakeTestBlocks() // 테스트 블록들 다 배치하기
    {
        //testBlocksVar = new GameObject[testBlocks.Length];
        correctAnss = new bool[testBlocks.Length];

        for (int i = 0; i < testBlocks.Length; i++)
        {
            //testBlocksVar[i] = Instantiate(testBlocks[i]);
            //testBlocksVar[i].transform.position = testBlockSpawnpos[i].transform.position;

            GameObject go = Instantiate(testBlocks[i]);
            go.GetComponent<BlockMoving_CM>().speed = 0f; // 못 움직이게 스피드는 0으로 고정시켜놓기
            go.transform.position = testBlockSpawnpos[i].transform.position;
            go.transform.name = testBlocks[i].transform.name;
            correctAnss[i] = false;

            //go.GetComponent<BlockMoving_CM>().speed = 0f; // 못 움직이게 스피드는 0으로 고정시켜놓기
        }
    }

    public void DeleteSaber()
    {
        Destroy(saberVar);
    }

    // (훈련용)세이버에 닿은 블록이 어떤 블록인지 알아낸 다음, 정답이면 그 블록을 삭제하고 잠시 후 다시 생성
    public void CorrectAnswer(GameObject go) 
    {
        Debug.Log(go);

        int correctAnsCnt = 0;

        for (int i = 0; i < testBlocks.Length; i++) // (훈련용)세이버에 닿은 블록이 어떤 블록인지 알아내기
        {
            if (testBlocks[i].name == go.name) // 어떤 블록인지 찾아냈으면
            {
                correctAnss[i] = true; // 해당 블록과 동일한 번호의 칸에 정답 처리                
                StartCoroutine(RemakeAfter2Sec(i));

                //StartCoroutine(Disable2SecAndRemake(go)); // 2초동안 비활성화하고 다시 만들기
            }

            if (correctAnss[i] == true) // 지금 까지 맞춘 정답 개수 세기
            {
                correctAnsCnt++;
            }
        }

        if (correctAnsCnt > 6) // 정답 개수가 6개가 넘어가면
        {
            // 다음 스텝으로 이동
        }
    }

    // (훈련용) 세이버에 닿은 블록이 어떤 블록인지 알아낸 다음, 오답이면 그 블록을 삭제하고 잠시 후 다시 생성
    public void WrongAnswer(GameObject go)
    {
        for (int i = 0; i < testBlocks.Length; i++) // (훈련용)세이버에 닿은 블록이 어떤 블록인지 알아내기
        {
            if (testBlocks[i].name == go.name) // 어떤 블록인지 찾아냈으면
            {
                StartCoroutine(RemakeAfter2Sec(i));

                //StartCoroutine(Disable2SecAndRemake(go)); // 2초동안 비활성화하고 다시 만들기
            }

            // 오답일 경우의 프로세스 실행
        }
    }

    IEnumerator RemakeAfter2Sec(int i)
    {
        yield return new WaitForSeconds(2f);

        Debug.Log("코루틴 종료, 블록 재생성");

        GameObject go = Instantiate(testBlocks[i]);
        go.GetComponent<BlockMoving_CM>().speed = 0f; // 못 움직이게 스피드는 0으로 고정시켜놓기
        go.transform.position = testBlockSpawnpos[i].transform.position;
        go.transform.name = testBlocks[i].transform.name;

        //go.GetComponent<BlockMoving_CM>().speed = 0f; // 못 움직이게 스피드는 0으로 고정시켜놓기
    }

    IEnumerator Disable2SecAndRemake(GameObject go)
    {
        // 파티클 및 효과음 생성

        // 잠시 비활성화하기
        go.SetActive(false);
        yield return new WaitForSeconds(2f);
        go.SetActive(true);

        Destroy(go);
    }

    #region Register with Lua
    private void OnEnable()
    {
        Lua.RegisterFunction("MakeTestSaberSS", this, SymbolExtensions.GetMethodInfo(() => MakeTestSaberSS()));
        Lua.RegisterFunction("MakeTestHeadWF", this, SymbolExtensions.GetMethodInfo(() => MakeTestHeadWF()));
        Lua.RegisterFunction("MakeTestSaberSSWF", this, SymbolExtensions.GetMethodInfo(() => MakeTestSaberSSWF()));
        Lua.RegisterFunction("MakeTestSSWFComplete", this, SymbolExtensions.GetMethodInfo(() => MakeTestSSWFComplete()));
        Lua.RegisterFunction("MakeTestBlocks", this, SymbolExtensions.GetMethodInfo(() => MakeTestBlocks()));
        Lua.RegisterFunction("DeleteSaber", this, SymbolExtensions.GetMethodInfo(() => DeleteSaber()));        
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction("MakeTestSaberSS");
        Lua.UnregisterFunction("MakeTestHeadWF");
        Lua.UnregisterFunction("MakeTestSaberSSWF");
        Lua.UnregisterFunction("MakeTestSSWFComplete");
        Lua.UnregisterFunction("MakeTestBlocks");
        Lua.UnregisterFunction("DeleteSaber");
    }
    #endregion
}
