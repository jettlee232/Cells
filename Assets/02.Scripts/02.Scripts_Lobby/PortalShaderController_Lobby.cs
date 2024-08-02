using Language.Lua;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PortalShaderController_Lobby : MonoBehaviour
{
    public GameObject portal;
    public Material originalMaterial;
    private Material instanceMaterial;
    private Renderer instanceRenderer;
    public bool ready = false;

    // SYS Code
    public bool capsuleVisible = true;

    // SYS Code
    public Tooltip rightHand;

    void Start()
    {
        instanceMaterial = new Material(originalMaterial);
        instanceMaterial.SetFloat("_Dissolve", 0f);

        instanceRenderer = portal.GetComponent<Renderer>();
        instanceRenderer.material = instanceMaterial;

        StartCoroutine(CheckMaterial());
    }

    // SYS Code
    public void OnLeverChange(float value)
    {
        // SYS Code <- MSY Origin
        //if (ready) { instanceMaterial.SetFloat("_Dissolve", value * 0.01f); }

        // SYS Code
        if (ready) 
        { 
            if (value == 100 && capsuleVisible == true)
            {
                if (rightHand.gameObject.activeSelf == true) rightHand.TooltipOff(); // 버그 위험성 있는 파트임 (툴팁이 다 안 꺼진 상태에서 이거 하면 위험)
                StartCoroutine(CapsuleDissolve(true));
            }
            if (value == 0 && capsuleVisible == false)
            {
                StartCoroutine(CapsuleDissolve(false));
            }
        }

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

    // SYS Code
    IEnumerator CapsuleDissolve(bool flag)
    {
        // True = Minus (-), False = Plus (+)

        if (flag == true)
        {
            Debug.Log("======Dissolve Is 100 -> 0======");

            float val = 0f;
            while (true)
            {
                val += 1f;

                instanceMaterial.SetFloat("_Dissolve", val * 0.01f);

                Debug.Log("100 -> 0 : " + val * 0.01f);

                if (val >= 100f) { instanceMaterial.SetFloat("_Dissolve", 1); capsuleVisible = false; break; }                
                
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            Debug.Log("======Dissolve Is 0 -> 100======");

            float val = 100f;
            while (true)
            {
                val -= 1f;

                instanceMaterial.SetFloat("_Dissolve", val * 0.01f);

                Debug.Log("0 -> 100 : " + val * 0.01f);


                if (val <= 0f) { instanceMaterial.SetFloat("_Dissolve", 0); capsuleVisible = true; break; }

                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
