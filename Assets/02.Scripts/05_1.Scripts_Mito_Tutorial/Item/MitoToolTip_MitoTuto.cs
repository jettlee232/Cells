using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MitoToolTip_MitoTuto : MonoBehaviour
{
    Tooltip_Mito tooltip;

    void Start()
    {
        tooltip = GetComponent<Tooltip_Mito>();

        tooltip.TooltipOff();
    }

    public void OnMyTooltip()
    {
        tooltip.TooltipOn("");
    }
}
