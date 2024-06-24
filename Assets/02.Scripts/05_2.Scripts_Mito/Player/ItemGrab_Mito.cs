using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrab_Mito : MonoBehaviour
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
        Item_Mito itemMito = item.GetComponent<Item_Mito>();
        if (itemMito != null)
        {
            itemMito.ItemGrabScale(0.25f);
        }
    }

    private void ItemReleaseEvent(Grabbable item)
    {
        Item_Mito itemMito = item.GetComponent<Item_Mito>();
        if (itemMito != null)
        {
            itemMito.ResetScale();
        }
    }
}
