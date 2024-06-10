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
        // LineRenderer ������Ʈ �߰� �� ����
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

        // LineRenderer�� �� ������Ʈ
        lineRenderer.SetPosition(0, rayStart);
        lineRenderer.SetPosition(1, rayEnd);

        // Raycast�� ������ ����
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // ������(�� ����)���� �������� ������ ������
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
