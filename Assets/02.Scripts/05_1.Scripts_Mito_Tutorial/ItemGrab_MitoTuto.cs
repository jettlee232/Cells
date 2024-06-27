using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrab_MitoTuto : MonoBehaviour
{
    public Grabber handGrabber;
    public Grabbable item;

    void Start()
    {
        handGrabber = GetComponent<Grabber>();
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
        if (handGrabber.HeldGrabbable != item)
        {
            // �������� ��������� �����տ��� �������� ����, ������ ������ item
            if (item != null)
            {
                // ������ ����
                ItemReleaseEvent(item);
            }

            // �������� �������� �����տ��� �������� �ְ�, ������ ������ null
            if (handGrabber.HeldGrabbable != null)
            {
                // ������ �׷�
                ItemGrabEvent(handGrabber.HeldGrabbable);
            }

            item = handGrabber.HeldGrabbable;
        }
    }

    private void ItemGrabEvent(Grabbable item)
    {
        MyATPMix_MitoTuto[] myATPMixArray = item.GetComponentsInChildren<MyATPMix_MitoTuto>();
        foreach (MyATPMix_MitoTuto myATPMix in myATPMixArray)
        {
            if (myATPMix != null)
            {
                myATPMix.GrabItem();
            }
        }
        
    }

    private void ItemReleaseEvent(Grabbable item)
    {
        MyATPMix_MitoTuto[] myATPMixArray = item.GetComponentsInChildren<MyATPMix_MitoTuto>();
        foreach (MyATPMix_MitoTuto myATPMix in myATPMixArray)
        {
            if (myATPMix != null)
            {
                myATPMix.CheckOtherItem(item);
            }
        }
    }
}
