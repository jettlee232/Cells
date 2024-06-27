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

    public List<Collider> collidersInRange = new List<Collider>();

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

    public void CheckOtherItem(string tag)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.025f);
        foreach (Collider collider in collidersInRange)
        {
            if (collider.CompareTag("Adeinine") && !isAdeinine)
            {
                AttachItem(collider.transform, "A_RibosePos");
                isAdeinine = true;
            }
            else if (collider.CompareTag("Ribose") && !isRibose)
            {
                AttachItem(collider.transform, "R_AdeininePos", "R_PhosphatePos");
                isRibose = true;
            }
            else if (collider.CompareTag("Phosphate") && !isPhosphate)
            {
                AttachItem(collider.transform, "P_RibosePos");
                isPhosphate = true;
            }

            /* 테스트
            if (collider != null)
            {
                switch (collider.transform.root.tag)
                {
                    case "Adeinine": // 아데닌에 닿았을때
                        if (isRibose) // 들고있는 아이템이 리보스
                        {
                            Debug.Log("리보스 + 아데닌");
                        }
                        break;
                    case "Ribose": // 리보스에 닿았을때
                        if (isAdeinine) // 들고있는 아이템이 아데닌
                        {
                            Debug.Log("아데닌 + 리보스");
                        }
                        else if (isPhosphate) // 들고있는 아이템이 인산염
                        {
                            Debug.Log("인산염 + 리보스");
                        }
                        break;
                    case "Phosphate": // 인산염에 닿았을때
                        if (isRibose) // 들고있는 아이템이 리보스
                        {
                            Debug.Log("리보스 + 인산염");
                        }
                        break;
                    default:
                        ResetMyItem();
                        break;
                }
                ResetMyItem();
            }
            */
        }
    }

    private void AttachItem(Transform item, params string[] positionNames)
    {
        foreach (string posName in positionNames)
        {
            Transform pos = item.Find(posName);
            if (pos != null)
            {
                item.SetParent(pos);
                item.localPosition = Vector3.zero;
                item.localRotation = Quaternion.identity;
                return;
            }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Adeinine") || other.CompareTag("Ribose") || other.CompareTag("Phosphate"))
        {
            collidersInRange.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Adeinine") || other.CompareTag("Ribose") || other.CompareTag("Phosphate"))
        {
            collidersInRange.Remove(other);
        }
    }
}
