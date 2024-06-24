using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class QuestManager_MitoTuto : MonoBehaviour
{
    UnityEngine.XR.InputDevice right;

    public TextMeshProUGUI questText;

    public bool playerInRange = false; // 플레이어가 범위 내에 있는지 확인하는 플래그
    public bool dialogueActive = false; // 대화가 활성화되었는지 확인하는 플래그

    public bool isABtnPressed = false;
    public bool wasABtnPressed = false;

    public GameObject npcToolTip;

    void Start()
    {
        StartCoroutine(StartDelayQuestTextReset(5.0f));
    }

    void Update()
    {
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        right.TryGetFeatureValue(CommonUsages.primaryButton, out isABtnPressed);

        if (npcToolTip.activeSelf)
        {
            playerInRange = true;
            QuestTextReset();
        }
        else
        {
            playerInRange = false;
        }

        if (playerInRange)
        {
            if (isABtnPressed && !wasABtnPressed && !dialogueActive)
            {
                DialogueController_MitoTuto.Instance.ActivateDST(3);
                dialogueActive = true;
            }
        }

        wasABtnPressed = isABtnPressed;

        if (dialogueActive)
            npcToolTip.SetActive(false);
    }

    IEnumerator StartDelayQuestTextReset(float delay)
    {
        yield return new WaitForSeconds(delay);
        HelloNpcText();
        yield return new WaitForSeconds(delay / 2.0f);
        QuestTextReset();
    }

    public void QuestTextReset()
    {
        questText.text = string.Empty;
    }

    public void HelloNpcText()
    {
        questText.text = "NPC에게 말을 걸어보자!";
    }

    public void SeeMitoText()
    {
        questText.text = "미토콘드리아 모형을 살펴보자!";
    }

    public void CanFlyText()
    {
        questText.text = "미토콘드리아 속을 비행해보자!";
    }
}
