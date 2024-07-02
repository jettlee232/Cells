using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemExplain_MitoTuto : MonoBehaviour
{
    public GameObject explainItemPanel;

    Grabbable grab;

    public bool isGrab = false;
    public bool isDesc = false;

    private void Start()
    {
        grab ??= GetComponent<Grabbable>();
    }

    private void Update()
    {
        if (grab && grab.SelectedHandPose)
            isGrab = true;

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
