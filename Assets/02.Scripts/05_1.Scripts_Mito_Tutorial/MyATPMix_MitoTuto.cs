using BNG;
using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyATPMix_MitoTuto : MonoBehaviour
{
    public Grabbable parentGrabbable;

    public HandPose handPose;

    public bool isAdeinine;
    public bool isRibose;
    public bool isPhosphate;

    void Start()
    {

    }

    void Update()
    {
        if (parentGrabbable.SelectedHandPose)
            handPose = parentGrabbable.SelectedHandPose;
        else
            handPose = null;

        
        if (handPose)
        {
            // 아주 좋은데 부모의 하이라이트가 꺼지는 조건을 어떻게 해야할지 고민
            //transform.root.GetComponent<HighlightEffect>().highlighted = true;
            //transform.root.GetComponent<HighLightColorchange_MitoTuto>().GlowStart();
            GetComponent<HighlightEffect>().highlighted = true;
        }
        else
        {
            GetComponent<HighlightEffect>().highlighted = false;
        }
    }

    public void CheckMyItem(string tag)
    {
        switch (tag)
        {
            case "Adeinine":
                isAdeinine = true;
                break;
            case "Ribose":
                isRibose = true;
                break;
            case "Phosphate":
                isPhosphate = true;
                break;
        }
    }

    public void CheckOtherItem()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.025f);
        foreach (Collider collider in colliders)
        {
            if (collider != null)
            {
                switch (collider.transform.root.tag)
                {
                    case "Adeinine":
                        isAdeinine = true;
                        break;
                    case "Ribose":
                        isRibose = true;
                        break;
                    case "Phosphate":
                        isPhosphate = true;
                        break;
                }
            }
        }

        if (isAdeinine && isRibose && isPhosphate)
        {
            // 모든 조합이 완료되면 특정 동작을 수행합니다.
            //DialogueController_MitoTuto.Instance.ActivateDST(n); // 예: n번 다이얼로그 실행
        }
    }

    public void ResetMyItem()
    {
        isAdeinine = false;
        isRibose = false;
        isPhosphate = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.025f);
    }
}
