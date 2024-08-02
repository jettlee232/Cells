using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GunController_Lys_Game : MonoBehaviour
{
    [Header("Positions")]
    public Transform firePos;
    public float maxDistance = 50f;

    [Header("Effects")]
    public Material lineMaterial;
    public GameObject shootEffect;
    public GameObject hitEffect;
    //public GameObject shootSound;

    private Ray ray;
    private RaycastHit hit;
    private bool triggerValue;
    private bool oldTriggerValue;
    private bool hitted;
    private GameObject lineObject;
    private LineRenderer lineRenderer;

    UnityEngine.XR.InputDevice right;

    int enemyLayer;

    private void OnEnable()
    {
        enemyLayer = 1 << LayerMask.NameToLayer("Enemy");

        lineObject = new GameObject("LineObject");
        lineRenderer = lineObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.03f;
        lineRenderer.endWidth = 0.03f;
        lineRenderer.material = lineMaterial;
    }
    private void OnDisable()
    {
        Destroy(lineObject);
    }

    void Update()
    {
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        right.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue);
        ray = new Ray(firePos.position, firePos.forward);
        hitted = Physics.Raycast(ray, out hit, maxDistance, enemyLayer);

        if (triggerValue && !oldTriggerValue)
        {
            Instantiate(shootEffect, firePos.position, Quaternion.identity);
            if (hitted)
            {
                Instantiate(hitEffect, hit.point, Quaternion.identity);
                hit.collider.gameObject.GetComponent<EnemyCommon_Lys_Game>().Die();
            }
        }

        if (hitted)
        {
            lineRenderer.SetPosition(0, firePos.position);
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            lineRenderer.SetPosition(0, firePos.position);
            lineRenderer.SetPosition(1, firePos.position + firePos.forward * maxDistance);
        }

        oldTriggerValue = triggerValue;
    }
}
