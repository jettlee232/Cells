using BNG;
using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyATPMix_MitoTuto : MonoBehaviour
{
    public Vector3 myPos;
    public Vector3 myRot;

    public bool isAdeinine;
    public bool isRibose;
    public bool isPhosphate;

    public void GrabItem()
    {
        transform.parent.GetComponentInParent<HighlightEffect>().highlighted = true;
        transform.parent.GetComponentInParent<HighLightColorchange_MitoTuto>().GlowStart();
        GetComponent<HighlightEffect>().highlighted = true;
    }

    public void ReleaseItem()
    {
        transform.parent.GetComponentInParent<HighlightEffect>().highlighted = false;
        transform.parent.GetComponentInParent<HighLightColorchange_MitoTuto>().GlowEnd();
        GetComponent<HighlightEffect>().highlighted = false;
    }

    public void CheckOtherItem(Grabbable item)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.025f);
        foreach (Collider collider in colliders)
        {
            if (collider != null)
            {
                switch (item.tag)
                {
                    case "Adeinine": // 들고있는 아이템이 아데닌
                        if (collider.CompareTag("Adeinine") && collider.transform.root.CompareTag("Ribose")) // 아데닌Pos에 닿았을때 
                        {
                            Debug.Log("아데닌이 리보스의 아데닌Pos에 충돌");
                            AttachItem(item, collider.transform);
                            isAdeinine = true;
                        }
                        // 리보스가 자식으로 있을때 인산염과의 충돌
                        Transform riboseInAdeinine = item.transform.Find("Ribose");
                        if (riboseInAdeinine != null)
                        {
                            Collider[] riboseColliders = Physics.OverlapSphere(riboseInAdeinine.position, 0.025f);
                            foreach (Collider riboseCollider in riboseColliders)
                            {
                                if (riboseCollider.CompareTag("Phosphate") && riboseCollider.transform.root.CompareTag("Ribose"))
                                {
                                    Debug.Log("리보스의 R_PhosphatePos와 인산염의 충돌");
                                    AttachItem(item, riboseCollider.transform);
                                    isPhosphate = true;
                                    break;
                                }
                            }
                        }
                        break;
                    case "Ribose": // 들고있는 아이템이 리보스
                        if (collider.CompareTag("Ribose")) // 리보스Pos에 닿았을때
                        {
                            if (collider.transform.root.CompareTag("Adeinine") && transform.CompareTag("Adeinine")) // 아데닌의 리보스Pos
                            {
                                Debug.Log("리보스가 아데닌의 리보스Pos에 충돌");
                                AttachItem(item, collider.transform);
                                isRibose = true;
                            }
                            if (collider.transform.root.CompareTag("Phosphate") && transform.CompareTag("Phosphate")) // 아데닌의 리보스Pos
                            {
                                Debug.Log("리보스가 인산염의 리보스Pos에 충돌");
                                AttachItem(item, collider.transform);
                                isRibose = true;
                            }
                        }
                        break;
                    case "Phosphate": // 들고있는 아이템이 인산염
                        if (collider.CompareTag("Phosphate") && collider.transform.root.CompareTag("Ribose")) // 인산염Pos에 닿았을때
                        {
                            Debug.Log("인산염이 리보스의 인산염Pos에 충돌");
                            AttachItem(item, collider.transform);
                            isPhosphate = true;
                        }
                        // 리보스가 자식으로 있을때 아데닌과의 충돌
                        Transform riboseInPhosphate = item.transform.Find("Ribose");
                        if (riboseInPhosphate != null)
                        {
                            Collider[] riboseColliders = Physics.OverlapSphere(riboseInPhosphate.position, 0.025f);
                            foreach (Collider riboseCollider in riboseColliders)
                            {
                                if (riboseCollider.CompareTag("Adeinine") && riboseCollider.transform.root.CompareTag("Ribose"))
                                {
                                    Debug.Log("리보스의 R_AdeininePos와 아데닌의 충돌");
                                    AttachItem(item, riboseCollider.transform);
                                    isAdeinine = true;
                                    break;
                                }
                            }
                        }
                        break;
                }
                ReleaseItem();
            }
        }
    }

    private void AttachItem(Grabbable item, Transform targetPos)
    {
        item.transform.SetParent(targetPos);
        // 위치와 회전을 어떻게 정해야지
        //item.transform.localPosition = myPos;
        //item.transform.localRotation = Quaternion.Euler(myRot);
        item.GetComponent<Grabbable>().enabled = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.025f);
    }

}
