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
        tooltip.TooltipOn("메뉴에 레이저를 갖다대서\n트리거 버튼으로 클릭해보세요!");

        //anim.enabled = false;

        btnTH1.DoButtonTween();
        btnTH2.DoButtonTween();
    }
}
