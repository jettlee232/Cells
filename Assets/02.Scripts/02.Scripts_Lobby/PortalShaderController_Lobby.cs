using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalShaderController_Lobby : MonoBehaviour
{
    public GameObject portal;
    public Material originalMaterial;
    private Material instanceMaterial;
    private Renderer instanceRenderer;
    public bool ready = false;

    void Start()
    {
        instanceMaterial = new Material(originalMaterial);
        instanceMaterial.SetFloat("_Dissolve", 0f);

        instanceRenderer = portal.GetComponent<Renderer>();
        instanceRenderer.material = instanceMaterial;

        StartCoroutine(CheckMaterial());
    }

    public void OnLeverChange(float value)
    {
        if (ready) { instanceMaterial.SetFloat("_Dissolve", value * 0.01f); }

        if (instanceMaterial.GetFloat("_Dissolve") >= 0.7f) { portal.GetComponent<CapsuleCollider>().isTrigger = true; }
        else { portal.GetComponent<CapsuleCollider>().isTrigger = false; }
    }

    IEnumerator CheckMaterial()
    {
        while (true)
        {
            if (instanceMaterial == null) { yield return null; }
            else { ready = true; yield break; }
        }
    }
}
