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
                    case "Adeinine": // ����ִ� �������� �Ƶ���
                        if (collider.CompareTag("Adeinine") && collider.transform.root.CompareTag("Ribose")) // �Ƶ���Pos�� ������� 
                        {
                            Debug.Log("�Ƶ����� �������� �Ƶ���Pos�� �浹");
                            AttachItem(item, collider.transform);
                            isAdeinine = true;
                        }
                        // �������� �ڽ����� ������ �λ꿰���� �浹
                        Transform riboseInAdeinine = item.transform.Find("Ribose");
                        if (riboseInAdeinine != null)
                        {
                            Collider[] riboseColliders = Physics.OverlapSphere(riboseInAdeinine.position, 0.025f);
                            foreach (Collider riboseCollider in riboseColliders)
                            {
                                if (riboseCollider.CompareTag("Phosphate") && riboseCollider.transform.root.CompareTag("Ribose"))
                                {
                                    Debug.Log("�������� R_PhosphatePos�� �λ꿰�� �浹");
                                    AttachItem(item, riboseCollider.transform);
                                    isPhosphate = true;
                                    break;
                                }
                            }
                        }
                        break;
                    case "Ribose": // ����ִ� �������� ������
                        if (collider.CompareTag("Ribose")) // ������Pos�� �������
                        {
                            if (collider.transform.root.CompareTag("Adeinine") && transform.CompareTag("Adeinine")) // �Ƶ����� ������Pos
                            {
                                Debug.Log("�������� �Ƶ����� ������Pos�� �浹");
                                AttachItem(item, collider.transform);
                                isRibose = true;
                            }
                            if (collider.transform.root.CompareTag("Phosphate") && transform.CompareTag("Phosphate")) // �Ƶ����� ������Pos
                            {
                                Debug.Log("�������� �λ꿰�� ������Pos�� �浹");
                                AttachItem(item, collider.transform);
                                isRibose = true;
                            }
                        }
                        break;
                    case "Phosphate": // ����ִ� �������� �λ꿰
                        if (collider.CompareTag("Phosphate") && collider.transform.root.CompareTag("Ribose")) // �λ꿰Pos�� �������
                        {
                            Debug.Log("�λ꿰�� �������� �λ꿰Pos�� �浹");
                            AttachItem(item, collider.transform);
                            isPhosphate = true;
                        }
                        // �������� �ڽ����� ������ �Ƶ��Ѱ��� �浹
                        Transform riboseInPhosphate = item.transform.Find("Ribose");
                        if (riboseInPhosphate != null)
                        {
                            Collider[] riboseColliders = Physics.OverlapSphere(riboseInPhosphate.position, 0.025f);
                            foreach (Collider riboseCollider in riboseColliders)
                            {
                                if (riboseCollider.CompareTag("Adeinine") && riboseCollider.transform.root.CompareTag("Ribose"))
                                {
                                    Debug.Log("�������� R_AdeininePos�� �Ƶ����� �浹");
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
        // ��ġ�� ȸ���� ��� ���ؾ���
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
