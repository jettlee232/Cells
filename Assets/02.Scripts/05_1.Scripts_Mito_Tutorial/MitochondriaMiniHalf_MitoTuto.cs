using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MitochondriaMiniHalf_MitoTuto : MonoBehaviour
{
    public bool dialogueActive = false; // ��ȭ�� Ȱ��ȭ�Ǿ����� Ȯ���ϴ� �÷���

    void Update()
    {
        if (GetComponent<Grabbable>().SelectedHandPose && !dialogueActive)
        {
            FinishExplainMito();
            dialogueActive = true;
        }
    }

    // ���� ���� �ܸ� �����̰��� ���� 3�� �� ���� ����
    public void FinishExplainMito()
    {
        // ���� ����
        DialogueController_MitoTuto.Instance.ActivateDST(5);
    }
}
