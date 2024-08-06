using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ʃ�丮���� �����г��� �ִ� �����۸��� �ִ� �Լ�
public class ItemExplain_MitoTuto : MonoBehaviour
{
    public GameObject explainItemPanel; // �����ۺ� �����г�

    Grabbable grab;

    public bool isGrab = false; // �������� ����
    public bool isDesc = false; // ����â�� ���Դ��� ����

    private void Start()
    {
        grab = GetComponent<Grabbable>();
    }

    private void Update()
    {
        if (grab != null && grab.SelectedHandPose)
            isGrab = true;

        // ������ �����۸��� �����ִ� ������ ����Ʈ�Ŵ���(QM)�� ��� ������� �ұ� �ߴµ�
        // �ϴ� QM�� ������ �̸� �����ְ� �������� �������� ���� QM�� �������� �ٲٴ� ������� ������
        switch (gameObject.tag)
        {
            case "Adenine":
                if (isGrab && isDesc)
                {
                    QuestManager_MitoTuto.Instance.isAdenine = true;
                }
                break;
            case "Ribose":
                if (isGrab && isDesc)
                {
                    QuestManager_MitoTuto.Instance.isRibose = true;
                }
                break;
            case "Phosphate":
                if (isGrab && isDesc)
                {
                    QuestManager_MitoTuto.Instance.isPhosphate = true;
                }
                break;
            case "ATP":
                if (isGrab && isDesc)
                {
                    QuestManager_MitoTuto.Instance.isATP = true;
                }
                break;
        }
    }
}
