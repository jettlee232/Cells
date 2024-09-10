using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivateCam_Homew : MonoBehaviour
{
    public GameObject[] xrCams;
    
    public Tooltip tooltip;
    public Animator anim;

    public ButtonTween_Home btnTH1;
    public ButtonTween_Home btnTH2;

    public void ActivateXRCam8()
    {
        for (int i = 0; i < xrCams.Length - 1; i++)
        {
            Destroy(xrCams[i]);
        }
        xrCams[xrCams.Length - 1].SetActive(true);

        
    }

    public void ShowUI()
    {
        tooltip.gameObject.SetActive(true);
        tooltip.TooltipOn("<color=#ff7373>메뉴</color>를 가리켜 <color=#ff7373>A 버튼</color>을 눌러봐!");

        //anim.enabled = false;

        btnTH1.DoButtonTween();
        btnTH2.DoButtonTween();
    }
}
