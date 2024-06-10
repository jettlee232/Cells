using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using System.ComponentModel;

public class TutorialManager_CM : MonoBehaviour
{
    [Header("Test Block Variable")]
    public GameObject[] testBlocks; // 12���� �׽�Ʈ���
    //public GameObject[] testBlocksVar; // 12���� �׽�Ʈ���
    public Transform[] testBlockSpawnpos; // 12���� �׽�Ʈ��� ��ġ ��ġ
    public int curCorrectAns; // ���� ���� ����
    public int correctAns; // ���� ����
    public bool[] correctAnss; // ���� ���� üũ

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


    // ������ ������ �Ӹ� ����� -> After First Encounter
    public void MakeTestSaberSS()
    {
        saberVar = Instantiate(testSaberSS, testSaberSSSpawnPos.position, Quaternion.identity);        
    }

    public void MakeTestHeadWF()
    {
        Instantiate(testHeadWF, testSaberWFSpawnPos.position, Quaternion.identity);
    }

    // ������ ������ �Ӹ� ��ġ�� -> After Second Conv
    public void MakeTestSaberSSWF()
    {
        Instantiate(testSaberSSWF, testSaberSSSpawnPos.position, Quaternion.Euler(90, 0, 0));
    }

    // ������ ������ ���� ����� �ϳ� �� ����� -> After Third Conv
    public void MakeTestSSWFComplete()
    {
        Instantiate(testSaberSSWFComplete, testSaberWFSpawnPos.position, Quaternion.Euler(0, 0, 0));
    }    

    // �׽�Ʈ ��ϵ� �� ��ġ�ϱ� -> After Fourth Conv
    public void MakeTestBlocks() // �׽�Ʈ ��ϵ� �� ��ġ�ϱ�
    {
        //testBlocksVar = new GameObject[testBlocks.Length];
        correctAnss = new bool[testBlocks.Length];

        for (int i = 0; i < testBlocks.Length; i++)
        {
            //testBlocksVar[i] = Instantiate(testBlocks[i]);
            //testBlocksVar[i].transform.position = testBlockSpawnpos[i].transform.position;

            GameObject go = Instantiate(testBlocks[i]);
            go.GetComponent<BlockMoving_CM>().speed = 0f; // �� �����̰� ���ǵ�� 0���� �������ѳ���
            go.transform.position = testBlockSpawnpos[i].transform.position;
            go.transform.name = testBlocks[i].transform.name;
            correctAnss[i] = false;

            //go.GetComponent<BlockMoving_CM>().speed = 0f; // �� �����̰� ���ǵ�� 0���� �������ѳ���
        }
    }

    public void DeleteSaber()
    {
        Destroy(saberVar);
    }

    // (�Ʒÿ�)���̹��� ���� ����� � ������� �˾Ƴ� ����, �����̸� �� ����� �����ϰ� ��� �� �ٽ� ����
    public void CorrectAnswer(GameObject go) 
    {
        Debug.Log(go);

        int correctAnsCnt = 0;

        for (int i = 0; i < testBlocks.Length; i++) // (�Ʒÿ�)���̹��� ���� ����� � ������� �˾Ƴ���
        {
            if (testBlocks[i].name == go.name) // � ������� ã�Ƴ�����
            {
                correctAnss[i] = true; // �ش� ��ϰ� ������ ��ȣ�� ĭ�� ���� ó��                
                StartCoroutine(RemakeAfter2Sec(i));

                //StartCoroutine(Disable2SecAndRemake(go)); // 2�ʵ��� ��Ȱ��ȭ�ϰ� �ٽ� �����
            }

            if (correctAnss[i] == true) // ���� ���� ���� ���� ���� ����
            {
                correctAnsCnt++;
            }
        }

        if (correctAnsCnt > 6) // ���� ������ 6���� �Ѿ��
        {
            // ���� �������� �̵�
        }
    }

    // (�Ʒÿ�) ���̹��� ���� ����� � ������� �˾Ƴ� ����, �����̸� �� ����� �����ϰ� ��� �� �ٽ� ����
    public void WrongAnswer(GameObject go)
    {
        for (int i = 0; i < testBlocks.Length; i++) // (�Ʒÿ�)���̹��� ���� ����� � ������� �˾Ƴ���
        {
            if (testBlocks[i].name == go.name) // � ������� ã�Ƴ�����
            {
                StartCoroutine(RemakeAfter2Sec(i));

                //StartCoroutine(Disable2SecAndRemake(go)); // 2�ʵ��� ��Ȱ��ȭ�ϰ� �ٽ� �����
            }

            // ������ ����� ���μ��� ����
        }
    }

    IEnumerator RemakeAfter2Sec(int i)
    {
        yield return new WaitForSeconds(2f);

        Debug.Log("�ڷ�ƾ ����, ��� �����");

        GameObject go = Instantiate(testBlocks[i]);
        go.GetComponent<BlockMoving_CM>().speed = 0f; // �� �����̰� ���ǵ�� 0���� �������ѳ���
        go.transform.position = testBlockSpawnpos[i].transform.position;
        go.transform.name = testBlocks[i].transform.name;

        //go.GetComponent<BlockMoving_CM>().speed = 0f; // �� �����̰� ���ǵ�� 0���� �������ѳ���
    }

    IEnumerator Disable2SecAndRemake(GameObject go)
    {
        // ��ƼŬ �� ȿ���� ����

        // ��� ��Ȱ��ȭ�ϱ�
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
