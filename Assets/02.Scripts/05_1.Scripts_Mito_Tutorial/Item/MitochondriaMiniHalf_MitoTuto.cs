using BNG;
using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MitochondriaMiniHalf_MitoTuto : MonoBehaviour
{
    public bool dialogueActive = false; // 대화가 활성화되었는지 확인하는 플래그

    public bool isMitoGrabbed = false; // 미토콘드리아 작은 모형이 한번이라도 잡혔는지 여부
    //public bool isComponentGrabbed = false;
    public float grabTime; // 잡히고 나서 시간 측정
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
                grabTime = Time.time; // 현재 시간을 grabTime에 저장
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
    // 자식의 설명패널이 모두 띄워졌는지 확인
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

    // 내막 외막 막사이공간 기질 주름 설명 5개 다 보면 실행
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
            // 10초가 지났는지 확인
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
