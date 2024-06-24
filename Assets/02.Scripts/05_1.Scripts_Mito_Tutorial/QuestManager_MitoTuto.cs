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

    public bool playerInRange = false; // �÷��̾ ���� ���� �ִ��� Ȯ���ϴ� �÷���
    public bool dialogueActive = false; // ��ȭ�� Ȱ��ȭ�Ǿ����� Ȯ���ϴ� �÷���

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
        questText.text = "NPC���� ���� �ɾ��!";
    }

    public void SeeMitoText()
    {
        questText.text = "�����ܵ帮�� ������ ���캸��!";
    }

    public void CanFlyText()
    {
        questText.text = "�����ܵ帮�� ���� �����غ���!";
    }
}
