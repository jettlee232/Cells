using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrab_MitoTuto : MonoBehaviour
{
    public Grabber rightHandGrabber;
    public Grabbable item;

    void Start()
    {
        rightHandGrabber = GetComponent<Grabber>();
    }

    void Update()
    {
        CheckItemGrabbedOrReleased();
    }

    private void CheckItemGrabbedOrReleased()
    {
        // �ƹ��͵� ������� �������� false (null != null)
        // �������� ��������� true (item != null) ���Ŀ� item ������ item ����
        // �������� ���������� true (null != item) ���Ŀ� item ������ null ����
        if (rightHandGrabber.HeldGrabbable != item)
        {
            // �������� ��������� �����տ��� �������� ����, ������ ������ item
            if (item != null)
            {
                // ������ ����
                ItemReleaseEvent(item);
            }

            // �������� �������� �����տ��� �������� �ְ�, ������ ������ null
            if (rightHandGrabber.HeldGrabbable != null)
            {
                // ������ �׷�
                ItemGrabEvent(rightHandGrabber.HeldGrabbable);
            }

            item = rightHandGrabber.HeldGrabbable;
        }
    }

    private void ItemGrabEvent(Grabbable item)
    {
        MyATPMix_MitoTuto myATPMix = item.GetComponentInChildren<MyATPMix_MitoTuto>();
        string tag = item.tag;
        if (myATPMix != null)
        {
            Debug.Log(tag);
            myATPMix.CheckMyItem(item.tag);
        }
    }

    private void ItemReleaseEvent(Grabbable item)
    {
        MyATPMix_MitoTuto myATPMix = item.GetComponentInChildren<MyATPMix_MitoTuto>();
        if (myATPMix != null)
        {
            myATPMix.CheckOtherItem();

        }
    }
}
