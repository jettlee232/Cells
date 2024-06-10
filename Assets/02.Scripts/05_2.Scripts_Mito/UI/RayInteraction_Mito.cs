using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayInteraction_Mito : MonoBehaviour
{
    public InventoryUI_Mito inventoryUI;
    private LineRenderer lineRenderer;

    void Start()
    {
        // LineRenderer 컴포넌트 추가 및 설정
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
    }

    void Update()
    {
        Vector3 rayStart = transform.position;
        Vector3 rayEnd = rayStart + (transform.forward * 10.0f);

        // LineRenderer의 점 업데이트
        lineRenderer.SetPosition(0, rayStart);
        lineRenderer.SetPosition(1, rayEnd);

        // Raycast로 아이템 감지
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // 스냅존(각 슬롯)에서 아이템의 정보를 가져옴
            SnapZone snapZone = hit.collider.GetComponent<SnapZone>();
            if (snapZone != null && snapZone.HeldItem != null)
            {
                inventoryUI.UpdateCurrentItemText(snapZone.HeldItem);
            }
            else
            {
                inventoryUI.ClearCurrentItemText();
            }
        }
    }
}
