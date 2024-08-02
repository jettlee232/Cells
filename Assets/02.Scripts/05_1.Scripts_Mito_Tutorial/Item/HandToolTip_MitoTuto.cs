using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandToolTip_MitoTuto : MonoBehaviour
{
    public bool isLeftHand = true;

    Tooltip_Mito tooltip;

    void Start()
    {
        tooltip = GetComponent<Tooltip_Mito>();

        tooltip.TooltipOff();

        if (isLeftHand)
        {
            StartCoroutine(LeftHandStartToolTip());
        }
        else
        {
            StartCoroutine(RightHandStartToolTip());
        }
    }

    IEnumerator LeftHandStartToolTip()
    {
        yield return new WaitForSeconds(4.5f);
        tooltip.TooltipOn("왼쪽 조이스틱으로 이동해보자!");
        yield return new WaitForSeconds(4.5f);
        tooltip.TooltipOff();
    }

    IEnumerator RightHandStartToolTip()
    {
        yield return new WaitForSeconds(4.5f);
        tooltip.TooltipOn("오른쪽 조이스틱으로 좌우회전도 가능해요!");
        yield return new WaitForSeconds(4.5f);
        tooltip.TooltipOff();
    }
}
