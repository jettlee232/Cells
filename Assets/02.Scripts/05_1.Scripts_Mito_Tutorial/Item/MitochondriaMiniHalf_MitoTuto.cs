using BNG;
using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MitochondriaMiniHalf_MitoTuto : MonoBehaviour
{
    public bool dialogueActive = false; // ��ȭ�� Ȱ��ȭ�Ǿ����� Ȯ���ϴ� �÷���

    public bool isMitoGrabbed = false;
    public bool isComponentGrabbed = false;

    public GameObject[] components;

    void Update()
    {
        if (GetComponent<Grabbable>().SelectedHandPose)
        {
            isMitoGrabbed = true;
            GetComponent<HighlightEffect>().highlighted = true;
        }

        if (isMitoGrabbed && CheckComponentDesc() && !dialogueActive)
        {
            StartCoroutine(FinishExplainMito());
            dialogueActive = true;
        }
    }

    public bool CheckComponentDesc()
    {
        foreach (var component in components)
        {
            ItemExplain_MitoTuto item = component.GetComponent<ItemExplain_MitoTuto>();

            if (item != null)
            {
                if (!item.isDesc)
                    return false;
            }
        }
        return true;
    }

    // ���� �ܸ� �����̰��� ���� �ָ� ���� 5�� �� ���� ����
    IEnumerator FinishExplainMito()
    {
        yield return new WaitForSeconds(0.5f);
        DialogueController_MitoTuto.Instance.ActivateDST(5);
    }
}
