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

        // Raycast로 아이템 감지
        ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit))
        {
            // 스냅존(각 슬롯)에서 아이템의 정보를 가져옴
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

            // 조건 수정 필요
            // 그립을 꾹 누르고있을때 Ray가 닿아도 스냅됨
            if (GetComponentInParent<HandController>().GripAmount >= 1.0f && !item)
            {
                if (grabAction)
                    RaycastItemDetach(grabAction);
            }
        }
    }

    // 버그 : 맨 처음에 하는 원거리 스냅이 안됨
    // Ray로 들고있는 아이템 먼곳에 스냅하기
    // 오른손의 Grabber에서 OnReleaseEvent로 호출
    public void RaycastItemSnap()
    {
        ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit))
        {
            SnapZone snapZone = hit.collider.GetComponent<SnapZone>();
            InventorySlot_Mito inventorySlot_Mito = hit.collider.GetComponent<InventorySlot_Mito>();

            if (snapZone && item)
            {
                //item.GetComponent<Rigidbody>().isKinematic = true;
                //item.transform.localPosition = Vector3.zero;
                //item.transform.localEulerAngles = Vector3.zero;

                // 아이템을 놓고 MaxDropTime 안에 SnapZone 근처에 있으면
                // 자동으로 스냅되는 기능이 프레임워크에 있었다.
                // 나중에 이동시간을 조절하면 될듯?
                Debug.Log($"원거리스냅: {item.name} in slot: {inventorySlot_Mito.slotType}");

                item.transform.localPosition = snapZone.transform.position;
                item.transform.localEulerAngles = snapZone.transform.localEulerAngles;
                //GameManager_Mito.Instance.MakeSnapEffect(snapZone.transform.position);

                //snapZone.GrabGrabbable(item);

                //snapZone.OnSnapEvent.Invoke(item);
                // 내가 호출해서 실행되는 이벤트랑
                // 이걸 호출하면 슬롯에 붙으면서 이벤트가 실행되는거랑 2개가 되는건가 설마?
            }
        }
    }

    // Ray로 먼곳의 아이템 손으로 가져오기
    // Update에서 GripAmount 확인해서 실행
    public void RaycastItemDetach(GrabAction grabAction)
    {
        // gripamount로 체크?

        grabAction.OnGrabEvent.Invoke(rightHandGrabber);
    }
}