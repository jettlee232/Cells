using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemExplain_MitoTuto : MonoBehaviour
{
    public GameObject explainItemPanel;

    //public bool isGrab = false;
    public bool isDesc = false;

    private void Update()
    {
        switch (gameObject.tag)
        {
            case "Adenine":
                if (isDesc)
                {
                    QuestManager_MitoTuto.Instance.isAdenine = true;
                }
                break;
            case "Ribose":
                if (isDesc)
                {
                    QuestManager_MitoTuto.Instance.isRibose = true;
                }
                break;
            case "Phosphate":
                if (isDesc)
                {
                    QuestManager_MitoTuto.Instance.isPhosphate = true;
                }
                break;
        }
    }
}
