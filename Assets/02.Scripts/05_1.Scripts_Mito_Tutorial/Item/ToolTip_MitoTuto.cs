using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip_MitoTuto : MonoBehaviour
{
    float MaxViewDistance = 4.0f;

    SpeechBubblePanel_CM bubblePanelMito;
    bool checkActive = false;

    Transform lookAt;

    void Start()
    {
        lookAt = Camera.main.transform;
        bubblePanelMito = GetComponent<SpeechBubblePanel_CM>();
    }

    void Update()
    {
        UpdateTooltipPosition();
    }

    public void UpdateTooltipPosition()
    {
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

        if (Vector3.Distance(transform.position, Camera.main.transform.position) <= MaxViewDistance)
        {
            if (!checkActive)
            {
                //bubblePanelMito.gameObject.SetActive(true);
                bubblePanelMito.PanelOpen("A버튼을 눌러 NPC와 대화");
                checkActive = true;
            }
        }
        else
        {
            if (checkActive)
            {
                bubblePanelMito.PanelClose();
                checkActive = false;
            }
        }
    }

    IEnumerator OpenPanelCoroutine()
    {
        bubblePanelMito.PanelOpen("A버튼을 눌러 NPC와 대화");
        yield return new WaitForSeconds(0.5f);
        checkActive = true;
    }

    IEnumerator ClosePanelCoroutine()
    {
        bubblePanelMito.PanelClose();
        yield return new WaitForSeconds(0.5f);
        checkActive = false;
    }
}