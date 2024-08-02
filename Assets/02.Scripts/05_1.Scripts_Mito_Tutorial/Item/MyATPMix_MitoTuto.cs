using BNG;
using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아데닌 리보스 인산염의 조합Pos에 있는 스크립트
public class MyATPMix_MitoTuto : MonoBehaviour
{
    public GameObject[] itemPrefabs; // 생성될 아이템

    public Vector3 myPos;
    public Vector3 myRot;

    public bool isMixed = false;

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

    private void OnTriggerEnter(Collider other)
    {
        if (isMixed) return;

        GameObject item = other.gameObject;
        if (item != null)
        {
            TriggerCheckOtherItem(item);
        }
    }

    public void TriggerCheckOtherItem(GameObject item) // 태그를 좀 더 세밀하게
    {
        switch (item.tag) // 트리거로 부딪힌 아이템의 태그 검사
        {
            case "Adenine": // 부딪힌 아이템이 아데닌Pos
                if (transform.CompareTag("Ribose")) // 나는 아데닌의 리보스Pos
                {
                    if (item.transform.root.CompareTag("Ribose"))
                    {
                        Debug.Log("아데닌의 리보스Pos가 리보스의 아데닌Pos와 충돌");
                        MixItem(item, gameObject, 0);
                    }

                    if (item.transform.root.CompareTag("Interim"))
                    {
                        Debug.Log("아데닌의 리보스Pos가 리보스+인산염의 아데닌Pos와 충돌");
                        MixItem(item, gameObject, 1);
                        QuestManager_MitoTuto.Instance.CheckMyATP();
                    }
                }
                
                break;
            case "Ribose": // 부딪힌 아이템이 리보스Pos
                if (transform.root.CompareTag("Phosphate") && item.transform.root.CompareTag("Adenine"))
                {
                    Debug.Log("리보스의 아데닌Pos가 아데닌의 리보스Pos와 충돌");
                    MixItem(item, gameObject, 0);
                }

                if (transform.root.CompareTag("Adenine") && item.transform.root.CompareTag("Phosphate"))
                {
                    Debug.Log("리보스의 아데닌Pos가 인산염의 리보스Pos와 충돌");
                    MixItem(item, gameObject, 0);
                }
                break;
            case "Phosphate": // 부딪힌 아이템이 인산염Pos
                if (transform.CompareTag("Ribose")) // 나는 인산염의 리보스Pos
                {
                    if (item.transform.root.CompareTag("Ribose"))
                    {
                        Debug.Log("인산염의 리보스Pos가 리보스의 인산염Pos와 충돌");
                        MixItem(item, gameObject, 0);
                    }

                    if (item.transform.root.CompareTag("Interim"))
                    {
                        Debug.Log("인산염의 리보스Pos가 아데닌+리보스의 인산염Pos와 충돌");
                        MixItem(item, gameObject, 1);
                        QuestManager_MitoTuto.Instance.CheckMyATP();
                    }
                }

                break;
        }
        //if (item)
        //    ReleaseItem();

    }

    /*
    // 플레이어 Grabber에서 아이템을 놓을때 호출
    // 이었는데 닿을때 즉시 조합으로 변경중
    public void CheckOtherItem(Grabbable item)
    {
        // 버그 : 겹친채로 빠르게 따닥해서 놓으면 2개 생김
        // 수정하면 버그가 없어지겠지 아마도
        // 놓는 순간 주변의 콜라이더를 체크
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.025f);
        foreach (Collider collider in colliders)
        {
            if (collider != null)
            {
                switch (item.tag) // 태그 검사
                {
                    case "Adenine": // 들고있는 아이템이 아데닌
                        if (collider.CompareTag("Adenine")) // 아데닌Pos에 닿았을때 
                        {
                            //AttachItem(item, collider.transform);
                            if (collider.transform.root.CompareTag("Ribose")) // 리보스의
                            {
                                Debug.Log("아데닌이 리보스의 아데닌Pos에 충돌");
                                MixItem(item, collider.gameObject, 0);
                            }

                            if (collider.transform.root.CompareTag("Interim")) // 중간 결과물의
                            {
                                Debug.Log("아데닌이 리보스+인산염의 아데닌Pos에 충돌");
                                MixItem(item, collider.gameObject, 1);
                                QuestManager_MitoTuto.Instance.CheckMyATP();
                            }
                        }

                        // 리보스가 자식으로 있을때 인산염과의 충돌 // 나중에

                        break;
                    case "Ribose": // 들고있는 아이템이 리보스
                        if (collider.CompareTag("Ribose")) // 리보스Pos에 닿았을때
                        {
                            if (collider.transform.root.CompareTag("Adenine") && transform.CompareTag("Adenine")) // 아데닌의
                            {
                                Debug.Log("리보스가 아데닌의 리보스Pos에 충돌");
                                //AttachItem(item, collider.transform);
                                MixItem(item, collider.gameObject, 0);
                            }
                            if (collider.transform.root.CompareTag("Phosphate") && transform.CompareTag("Phosphate")) // 인산염의
                            {
                                Debug.Log("리보스가 인산염의 리보스Pos에 충돌");
                                //AttachItem(item, collider.transform);
                                MixItem(item, collider.gameObject, 0);
                            }
                        }
                        break;
                    case "Phosphate": // 들고있는 아이템이 인산염
                        if (collider.CompareTag("Phosphate")) // 인산염Pos에 닿았을때
                        {
                            //AttachItem(item, collider.transform);
                            if (collider.transform.root.CompareTag("Ribose")) // 리보스의
                            {
                                Debug.Log("인산염이 리보스의 인산염Pos에 충돌");
                                MixItem(item, collider.gameObject, 0);
                            }
                            if (collider.transform.root.CompareTag("Interim")) // 중간 결과물의
                            {
                                Debug.Log("인산염이 아데닌+리보스의 인산염Pos에 충돌");
                                MixItem(item, collider.gameObject, 1);
                                QuestManager_MitoTuto.Instance.CheckMyATP();
                            }
                        }

                        // 리보스가 자식으로 있을때 아데닌과의 충돌// 나중에

                        break;
                    case "Interim": // 들고있는 아이템이 중간결과물, 아데닌+리보스 or 리보스+인산염
                        if (collider.CompareTag("Ribose")) // 리보스Pos에 닿았을때
                        {
                            if (collider.transform.root.CompareTag("Adenine") && transform.CompareTag("Adenine")) // 아데닌의 리보스Pos
                            {
                                Debug.Log("리보스+인산염이 아데닌의 리보스Pos에 충돌");
                                MixItem(item, collider.gameObject, 0);
                                QuestManager_MitoTuto.Instance.CheckMyATP();
                            }
                            if (collider.transform.root.CompareTag("Phosphate") && transform.CompareTag("Phosphate")) // 인산염의 리보스Pos
                            {
                                Debug.Log("아데닌+리보스가 인산염의 리보스Pos에 충돌");
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
    */

    /*
    private void AttachItem(Grabbable item, Transform targetPos)
    {
        item.transform.SetParent(targetPos);
        // 위치와 회전을 어떻게 정해야지
        //item.transform.localPosition = myPos;
        //item.transform.localRotation = Quaternion.Euler(myRot);
        item.GetComponent<Grabbable>().enabled = false;
        QuestManager_MitoTuto.Instance.CheckMyATP();
    }
    */

    // 아이템을 붙이는 척, 두개 다 없애고 새로운 아이템 생성
    private void MixItem(GameObject grabItem, GameObject colItem, int index)
    {
        isMixed = true;

        ReleaseItem();
        GameObject newItem = Instantiate(itemPrefabs[index], grabItem.transform.position, Quaternion.identity);

        MixEffect(grabItem.transform.position, Quaternion.identity);
        Destroy(grabItem.transform.root.gameObject);
        Destroy(colItem.transform.root.gameObject);
    }

    private void MixEffect(Vector3 pos, Quaternion rot)
    {
        Instantiate(QuestManager_MitoTuto.Instance.mixEffect, pos, rot);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, 0.025f);
    //}

}
