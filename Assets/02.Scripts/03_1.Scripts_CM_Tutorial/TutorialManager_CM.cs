using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager_CM : MonoBehaviour
{
    public GameObject[] testBlocks; // 12���� �׽�Ʈ���
    //public GameObject[] testBlocksVar; // 12���� �׽�Ʈ���
    public Transform[] testBlockSpawnpos; // 12���� �׽�Ʈ��� ��ġ ��ġ
    public int curCorrectAns; // ���� ���� ����
    public int correctAns; // ���� ����
    public bool[] correctAnss; // ���� ���� üũ

    void Start()
    {
        MakeTestBlocks();
    }

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
}
