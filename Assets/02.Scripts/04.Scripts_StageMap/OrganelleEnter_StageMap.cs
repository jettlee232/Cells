using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class OrganelleEnter_StageMap : MonoBehaviour
{
    public GameObject pointer;
    private BNG.UIPointer uiPointer; // ������ ������Ʈ

    public bool isTriggerPressed = false; // Ʈ���Ű� �������� �� ��������
    public bool isButtonPressed = false; // ������ A��ư�� �������� �� ��������
    private bool enter = false;

    UnityEngine.XR.InputDevice right; // ������ ��Ʈ�ѷ� ���¸� �޴� ����

    private void Start()
    {
        uiPointer = pointer.GetComponent<BNG.UIPointer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���⼭ Ŭ���� �ϸ� �������������� SetObj(this);�� �ҷ��´�
        enter = true;
        StartCoroutine(CheckClick());
        pointer.GetComponent<LaserPointer_StageMap>().InObj();
    }

    private void OnTriggerExit(Collider other)
    {
        enter = false;
        pointer.GetComponent<LaserPointer_StageMap>().OutObj();
    }

    IEnumerator CheckClick()
    {
        while (true)
        {
            if (!enter) break;
            right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            right.TryGetFeatureValue(CommonUsages.triggerButton, out isTriggerPressed);
            right.TryGetFeatureValue(CommonUsages.primaryButton, out isButtonPressed);

            if (isTriggerPressed) // Ʈ���Ű� ������ �ִٸ�
            {
                uiPointer.HidePointerIfNoObjectsFound = false; // ������ ���̰� �ϱ�
                if (GameManager_StageMap.instance.GetMovable() && GameManager_StageMap.instance.GetSelectable()) { pointer.GetComponent<LaserPointer_StageMap>().SetObj(this.gameObject); }
            }
            else { uiPointer.HidePointerIfNoObjectsFound = true; }
            yield return null;
        }
    }
}