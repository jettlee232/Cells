using BNG;
using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyATPMix_MitoTuto : MonoBehaviour
{
    public Vector3 myPos;
    public Vector3 myRot;

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
                        }

                        // �������� �ڽ����� ������ �λ꿰���� �浹
                        
                        break;
                    case "Ribose": // ����ִ� �������� ������
                        if (collider.CompareTag("Ribose")) // ������Pos�� �������
                        {
                            if (collider.transform.root.CompareTag("Adeinine") && transform.CompareTag("Adeinine")) // �Ƶ����� ������Pos
                            {
                                Debug.Log("�������� �Ƶ����� ������Pos�� �浹");
                                //AttachItem(item, collider.transform);
                            }
                            if (collider.transform.root.CompareTag("Phosphate") && transform.CompareTag("Phosphate")) // �λ꿰�� ������Pos
                            {
                                Debug.Log("�������� �λ꿰�� ������Pos�� �浹");
                                //AttachItem(item, collider.transform);
                            }
                        }
                        break;
                    case "Phosphate": // ����ִ� �������� �λ꿰
                        if (collider.CompareTag("Phosphate") && collider.transform.root.CompareTag("Ribose")) // �λ꿰Pos�� �������
                        {
                            Debug.Log("�λ꿰�� �������� �λ꿰Pos�� �浹");
                            AttachItem(item, collider.transform);
                        }

                        // �������� �ڽ����� ������ �Ƶ��Ѱ��� �浹
                        
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
        QuestManager_MitoTuto.Instance.CheckMyATP();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.025f);
    }

}
