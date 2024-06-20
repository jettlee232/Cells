using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayInteraction_Mito : MonoBehaviour
{
    //public InventoryUI_Mito inventoryUI;
    public Grabber rightHandGrabber;

    Ray ray;
    RaycastHit hit;
    public Grabbable item;

    void Update()
    {
        item = rightHandGrabber?.HeldGrabbable;

        // Raycast�� ������ ����
        ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit))
        {
            // ������(�� ����)���� �������� ������ ������
            //SnapZone snapZone = hit.collider.GetComponent<SnapZone>();
            GrabAction grabAction = hit.collider.GetComponent<GrabAction>();

            //if (snapZone != null && snapZone.HeldItem != null && hit.collider.CompareTag("Inventory"))
            //{
            //    inventoryUI.UpdateCurrentItemText(snapZone.HeldItem);
            //}
            //else
            //{
            //    inventoryUI.ClearCurrentItemText();
            //}

            if (GetComponentInParent<HandController>().GripAmount >= 1.0f && !item)
            {
                if (grabAction)
                    RaycastItemDetach(grabAction);
            }
        }
    }

    // Ray�� �հ��� ����ִ� ������ �����ϱ�
    // �������� Grabber���� OnReleaseEvent�� ȣ��
    public void RaycastItemSnap()
    {
        ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit))
        {
            SnapZone snapZone = hit.collider.GetComponent<SnapZone>();

            if (snapZone && item)
            {
                //item.GetComponent<Rigidbody>().isKinematic = true;
                //item.transform.localPosition = Vector3.zero;
                //item.transform.localEulerAngles = Vector3.zero;

                // �������� ���� MaxDropTime �ȿ� SnapZone ��ó�� ������
                // �ڵ����� �����Ǵ� ����� �����ӿ�ũ�� �־���.
                // ���߿� �̵��ð��� �����ϸ� �ɵ�?
                item.transform.localPosition = snapZone.transform.position;
                item.transform.localEulerAngles = snapZone.transform.localEulerAngles;

                //snapZone.GrabGrabbable(item);

                //snapZone.OnSnapEvent.Invoke(item);
                // ���� ȣ���ؼ� ����Ǵ� �̺�Ʈ��
                // �̰� ȣ���ϸ� ���Կ� �����鼭 �̺�Ʈ�� ����Ǵ°Ŷ� 2���� �Ǵ°ǰ� ����?
            }
        }
    }

    // Ray�� �հ��� ������ ������ ��������
    // Update���� GripAmount Ȯ���ؼ� ����
    public void RaycastItemDetach(GrabAction grabAction)
    {
        // gripamount�� üũ?

        grabAction.OnGrabEvent.Invoke(rightHandGrabber);
    }
}