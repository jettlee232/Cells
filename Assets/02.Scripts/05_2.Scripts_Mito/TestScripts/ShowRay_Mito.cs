using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRay_Mito : MonoBehaviour
{
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
        Vector3 rayEnd = rayStart + (transform.forward * 1.0f);

        // LineRenderer�� �� ������Ʈ
        lineRenderer.SetPosition(0, rayStart);
        lineRenderer.SetPosition(1, rayEnd);
    }
}
