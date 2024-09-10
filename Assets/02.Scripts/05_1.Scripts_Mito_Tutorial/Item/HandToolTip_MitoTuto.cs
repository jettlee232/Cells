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
        tooltip.TooltipOn("<color=#ff7373>���̽�ƽ</color>���� ��������!");
        yield return new WaitForSeconds(4.5f);
        tooltip.TooltipOff();
    }

    IEnumerator RightHandStartToolTip()
    {
        yield return new WaitForSeconds(4.5f);
        tooltip.TooltipOn("<color=#ff7373>�¿�ȸ��</color>�� ������!");
        yield return new WaitForSeconds(4.5f);
        tooltip.TooltipOff();
    }
}
