using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrab_MitoTuto : MonoBehaviour
{
    public Grabber handGrabber;
    public Grabbable item;

    void Start()
    {
        handGrabber = GetComponent<Grabber>();
    }

    void Update()
    {
        CheckItemGrabbedOrReleased();
    }

    private void CheckItemGrabbedOrReleased()
    {
        // 아무것도 들고있지 않을때는 false (null != null)
        // 아이템을 잡았을때는 true (item != null) 이후에 item 변수에 item 저장
        // 아이템을 놓았을때는 true (null != item) 이후에 item 변수에 null 저장
        if (handGrabber.HeldGrabbable != item)
        {
            // 아이템을 놓았을경우 오른손에는 아이템이 없고, 아이템 변수는 item
            if (item != null)
            {
                // 아이템 놓기
                ItemReleaseEvent(item);
            }

            // 아이템을 잡았을경우 오른손에는 아이템이 있고, 아이템 변수는 null
            if (handGrabber.HeldGrabbable != null)
            {
                // 아이템 그랩
                ItemGrabEvent(handGrabber.HeldGrabbable);
            }

            item = handGrabber.HeldGrabbable;
        }
    }

    private void ItemGrabEvent(Grabbable item)
    {
        MyATPMix_MitoTuto[] myATPMixArray = item.GetComponentsInChildren<MyATPMix_MitoTuto>();
        foreach (MyATPMix_MitoTuto myATPMix in myATPMixArray)
        {
            if (myATPMix != null)
            {
                myATPMix.GrabItem();
            }
        }
        
    }

    private void ItemReleaseEvent(Grabbable item)
    {
        MyATPMix_MitoTuto[] myATPMixArray = item.GetComponentsInChildren<MyATPMix_MitoTuto>();
        foreach (MyATPMix_MitoTuto myATPMix in myATPMixArray)
        {
            if (myATPMix != null)
            {
                myATPMix.CheckOtherItem(item);
            }
        }
    }
}
