using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �������� Grabber�� �ִ� ��ũ��Ʈ
public class ItemGrab_Mito : MonoBehaviour
{
    public PlayerMoving_Mito playerMoving_Mito;
    public Grabber rightHandGrabber;
    public Grabbable item;

    void Start()
    {
        playerMoving_Mito = GetComponentInParent<PlayerMoving_Mito>();
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
                // ������ ������ ȣ��
                ItemReleaseEvent(item);
                playerMoving_Mito.SetPlayerSpeed(6.0f, 8.0f, 8.0f);
            }

            // �������� �������� �����տ��� �������� �ְ�, ������ ������ null
            if (rightHandGrabber.HeldGrabbable != null)
            {
                // ������ �׷��� ȣ��
                ItemGrabEvent(rightHandGrabber.HeldGrabbable);
                playerMoving_Mito.SetPlayerSpeed(3.0f, 4.0f, 4.0f);
            }

            item = rightHandGrabber.HeldGrabbable;
        }
    }

    private void ItemGrabEvent(Grabbable item)
    {
        Item_Mito itemMito = item.GetComponent<Item_Mito>();
        if (itemMito != null)
        {
            itemMito.ItemGrabScale(0.05f);
        }
    }

    private void ItemReleaseEvent(Grabbable item)
    {
        Item_Mito itemMito = item.GetComponent<Item_Mito>();
        if (itemMito != null)
        {
            itemMito.ResetScale();
        }

        CheckAndCreateSnapEffect(item);
    }

    private void CheckAndCreateSnapEffect(Grabbable releasedItem)
    {
        // ���� ��ġ�� �ݶ��̴����� Ȯ��
        Collider[] hitColliders = Physics.OverlapSphere(releasedItem.transform.position, 0.1f);

        foreach (var collider in hitColliders)
        {
            if (collider.GetComponent<SnapZone>() != null) // SnapZone�� �������� ����Ʈ ����
            {
                GameManager_Mito.Instance.MakeSnapEffect(releasedItem.transform.position);
                break;
            }
        }
    }
}
