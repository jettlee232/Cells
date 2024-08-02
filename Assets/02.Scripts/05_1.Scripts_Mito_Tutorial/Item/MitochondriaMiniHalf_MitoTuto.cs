using BNG;
using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MitochondriaMiniHalf_MitoTuto : MonoBehaviour
{
    public bool dialogueActive = false; // ��ȭ�� Ȱ��ȭ�Ǿ����� Ȯ���ϴ� �÷���

    public bool isMitoGrabbed = false; // �����ܵ帮�� ���� ������ �ѹ��̶� �������� ����
    //public bool isComponentGrabbed = false;
    public float grabTime; // ������ ���� �ð� ����
    public bool isGrabFinish = false;

    public GameObject[] components;

    public Tooltip_Mito[] tooltips;

    void Update()
    {
        if (GetComponent<Grabbable>().SelectedHandPose)
        {
            if (!isMitoGrabbed)
            {
                isMitoGrabbed = true;
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                tooltips = player.GetComponentsInChildren<Tooltip_Mito>();
                foreach (Tooltip_Mito tooltip in tooltips)
                {
                    tooltip.TooltipOff();
                }

                //GetComponent<HighlightEffect>().highlighted = true;
                grabTime = Time.time; // ���� �ð��� grabTime�� ����
                StartCoroutine(CheckGrabDuration());
            }
        }

        /* Old
        if (isMitoGrabbed && CheckComponentDesc() && !dialogueActive)
        {
            StartCoroutine(FinishExplainMito());
            dialogueActive = true;
        }
        */

        if (!dialogueActive && isGrabFinish && GetComponent<Grabbable>().SelectedHandPose == null)
        {
            StartCoroutine(FinishGrabMito());
            dialogueActive = true;
        }
    }

    /* Old
    // �ڽ��� �����г��� ��� ��������� Ȯ��
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
    */

    IEnumerator CheckGrabDuration()
    {
        while (true)
        {
            // 10�ʰ� �������� Ȯ��
            if (Time.time - grabTime >= 10f)
            {
                isGrabFinish = true;
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator FinishGrabMito()
    {
        yield return new WaitForSeconds(5.0f);
        DialogueController_MitoTuto.Instance.ActivateDST(5);
    }
}
