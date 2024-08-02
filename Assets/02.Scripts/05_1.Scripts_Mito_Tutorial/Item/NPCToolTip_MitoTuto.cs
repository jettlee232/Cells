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
                //bubblePanelMito.PanelOpen("A��ư�� ���� NPC�� ��ȭ");
                //tooltip.TooltipOn("A��ư�� ������!");
                if (!playerMoving_Mito.flyable)
                    tooltip.TooltipTextChange("A��ư�� ������!");
                else
                    tooltip.TooltipTextChange("����� ��������� A��ư�� ������!");
                checkActive = true;
            }
        }
        else
        {
            if (checkActive)
            {
                //bubblePanelMito.PanelClose();
                if (!playerMoving_Mito.flyable)
                    tooltip.TooltipTextChange("�����̾�~!");
                else
                    tooltip.TooltipTextChange("õõ�� �����غ�~!");
                checkActive = false;
            }
        }
    }

    IEnumerator StartTooltip()
    {
        yield return new WaitForSeconds(4.5f);
        tooltip.TooltipOn("�����̾�~!");
    }

    /*
    IEnumerator OpenPanelCoroutine()
    {
        //bubblePanelMito.PanelOpen("A��ư�� ���� NPC�� ��ȭ");
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