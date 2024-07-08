using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATPMixTablePos_Mito : MonoBehaviour
{
    public enum PosType { In, Out }
    public PosType posType;

    public ATPSynthaseInActive_Mito atpSynthaseInActive;

    private void Start()
    {
        atpSynthaseInActive = GetComponentInParent<ATPSynthaseInActive_Mito>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            atpSynthaseInActive.ShowUIPanel(posType);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            atpSynthaseInActive.HideUIPanel(posType);
        }
    }
}
