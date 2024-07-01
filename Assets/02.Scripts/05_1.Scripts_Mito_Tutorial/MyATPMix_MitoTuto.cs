using BNG;
using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyATPMix_MitoTuto : MonoBehaviour
{
    public GameObject[] itemPrefabs;

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
                    case "Adenine": // ����ִ� �������� �Ƶ���
                        if (collider.CompareTag("Adenine")) // �Ƶ���Pos�� ������� 
                        {
                            //AttachItem(item, collider.transform);
                            if (collider.transform.root.CompareTag("Ribose"))
                            {
                                Debug.Log("�Ƶ����� �������� �Ƶ���Pos�� �浹");
                                MixItem(item, collider.gameObject, 0);
                            }

                            if (collider.transform.root.CompareTag("Interim"))
                            {
                                Debug.Log("�Ƶ����� ������+�λ꿰�� �Ƶ���Pos�� �浹");
                                MixItem(item, collider.gameObject, 1);
                                QuestManager_MitoTuto.Instance.CheckMyATP();
                            }
                        }

                        // �������� �ڽ����� ������ �λ꿰���� �浹 // ���߿�
                        
                        break;
                    case "Ribose": // ����ִ� �������� ������
                        if (collider.CompareTag("Ribose")) // ������Pos�� �������
                        {
                            if (collider.transform.root.CompareTag("Adenine") && transform.CompareTag("Adenine")) // �Ƶ����� ������Pos
                            {
                                Debug.Log("�������� �Ƶ����� ������Pos�� �浹");
                                //AttachItem(item, collider.transform);
                                MixItem(item, collider.gameObject, 0);
                            }
                            if (collider.transform.root.CompareTag("Phosphate") && transform.CompareTag("Phosphate")) // �λ꿰�� ������Pos
                            {
                                Debug.Log("�������� �λ꿰�� ������Pos�� �浹");
                                //AttachItem(item, collider.transform);
                                MixItem(item, collider.gameObject, 1);
                            }
                        }
                        break;
                    case "Phosphate": // ����ִ� �������� �λ꿰
                        if (collider.CompareTag("Phosphate")) // �λ꿰Pos�� �������
                        {
                            //AttachItem(item, collider.transform);
                            if (collider.transform.root.CompareTag("Ribose"))
                            {
                                Debug.Log("�λ꿰�� �������� �λ꿰Pos�� �浹");
                                MixItem(item, collider.gameObject, 0);
                            }
                            if (collider.transform.root.CompareTag("Interim"))
                            {
                                Debug.Log("�λ꿰�� �Ƶ���+�������� �λ꿰Pos�� �浹");
                                MixItem(item, collider.gameObject, 1);
                                QuestManager_MitoTuto.Instance.CheckMyATP();
                            }
                        }

                        // �������� �ڽ����� ������ �Ƶ��Ѱ��� �浹// ���߿�

                        break;
                    case "Interim": // ����ִ� �������� �߰������, �Ƶ���+������ or ������+�λ꿰
                        if (collider.CompareTag("Ribose")) // ������Pos�� �������
                        {
                            if (collider.transform.root.CompareTag("Adenine") && transform.CompareTag("Adenine")) // �Ƶ����� ������Pos
                            {
                                Debug.Log("������+�λ꿰�� �Ƶ����� ������Pos�� �浹");
                                MixItem(item, collider.gameObject, 0);
                                QuestManager_MitoTuto.Instance.CheckMyATP();
                            }
                            if (collider.transform.root.CompareTag("Phosphate") && transform.CompareTag("Phosphate")) // �λ꿰�� ������Pos
                            {
                                Debug.Log("�Ƶ���+�������� �λ꿰�� ������Pos�� �浹");
                                MixItem(item, collider.gameObject, 0);
                                QuestManager_MitoTuto.Instance.CheckMyATP();
                            }
                        }
                        break;
                }
                if (item)
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

    private void MixItem(Grabbable grabItem, GameObject colItem, int index)
    {
        ReleaseItem();
        GameObject newItem = Instantiate(itemPrefabs[index], grabItem.transform.position, Quaternion.identity);

        Destroy(grabItem.gameObject);
        Destroy(colItem.transform.root.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.025f);
    }

}
