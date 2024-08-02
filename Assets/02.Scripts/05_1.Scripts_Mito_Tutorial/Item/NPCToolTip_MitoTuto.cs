using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCToolTip_MitoTuto : MonoBehaviour
{
    PlayerMoving_Mito playerMoving_Mito;

    float MaxViewDistance = 4.0f;

    //SpeechBubblePanel_CM bubblePanelMito;
    public bool checkActive = false;

    //Transform lookAt;

    Tooltip_Mito tooltip;

    void Start()
    {
        playerMoving_Mito = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoving_Mito>();
        tooltip = GetComponent<Tooltip_Mito>();
        //lookAt = Camera.main.transform;
        //bubblePanelMito = GetComponent<SpeechBubblePanel_CM>();
        //tooltip.TooltipOff();
        StartCoroutine(StartTooltip());

        Debug.Log(Camera.main.transform.name);
    }

    void Update()
    {
        UpdateNPCTooltip();
    }

    public void UpdateNPCTooltip()
    {
        /*
        if (lookAt)
        {
            transform.LookAt(Camera.main.transform);
        }
        else if (Camera.main != null)
        {
            lookAt = Camera.main.transform;
        }
        else if (Camera.main == null)
        {
            return;
        }
        */

        if (Vector3.Distance(transform.position, Camera.main.transform.position) <= MaxViewDistance)
        {
            if (!checkActive)
            {
                //bubblePanelMito.gameObject.SetActive(true);
                //bubblePanelMito.PanelOpen("A버튼을 눌러 NPC와 대화");
                //tooltip.TooltipOn("A버튼을 눌러봐!");
                if (!playerMoving_Mito.flyable)
                    tooltip.TooltipTextChange("A버튼을 눌러봐!");
                else
                    tooltip.TooltipTextChange("충분히 살펴봤으면 A버튼을 눌러봐!");
                checkActive = true;
            }
        }
        else
        {
            if (checkActive)
            {
                //bubblePanelMito.PanelClose();
                if (!playerMoving_Mito.flyable)
                    tooltip.TooltipTextChange("이쪽이야~!");
                else
                    tooltip.TooltipTextChange("천천히 구경해봐~!");
                checkActive = false;
            }
        }
    }

    IEnumerator StartTooltip()
    {
        yield return new WaitForSeconds(4.5f);
        tooltip.TooltipOn("이쪽이야~!");
    }

    /*
    IEnumerator OpenPanelCoroutine()
    {
        //bubblePanelMito.PanelOpen("A버튼을 눌러 NPC와 대화");
        yield return new WaitForSeconds(0.5f);
        checkActive = true;
    }

    IEnumerator ClosePanelCoroutine()
    {
        //bubblePanelMito.PanelClose();
        yield return new WaitForSeconds(0.5f);
        checkActive = false;
    }
    */
}