using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Mito : MonoBehaviour
{
    float moveSpeed = 0.25f;
    float rotSpeed = 180.0f;
    public float moveDelay = 2.0f;
    private float destroyTime = 30.0f;

    public bool isGrabbed = false;

    public Transform mainCameraTransform;

    Grabbable grabbable;

    public enum ItemType
    {
        Adenine,
        Ribose,
        Phosphate,
        ADP,
        ATP,
        H_Ion,
        FREE
    }

    public ItemType type;

    private static readonly Dictionary<ItemType, string> itemTypeNames = new Dictionary<ItemType, string>
    {
        { ItemType.Adenine, "�Ƶ���" },
        { ItemType.Ribose, "������" },
        { ItemType.Phosphate, "�λ�" },
        { ItemType.ADP, "ADP" },
        { ItemType.ATP, "ATP" },
        { ItemType.H_Ion, "�����̿�" },
        { ItemType.FREE, "����" }
    };

    void Start()
    {
        grabbable = GetComponent<Grabbable>();
    }

    void Update()
    {
        //isGrabbed = grabbable.SelectedHandPose ? true : false;
        isGrabbed = CheckGrab();

        if (!isGrabbed)
           ItemMove();

        //ItemLock();
    }

    // �������� �׻� �����δ�
    // isGrabbed�϶��� �����̸� �ȵȴ�
    // isGrabbed�� false�� �Ǹ� �� ���� �ڿ� �����̰� �Ѵ�
    // ���⼭ Ʈ�� ��밢?

    public bool CheckGrab()
    {
        if (grabbable.RemoteGrabbing)
        {
            return true;
        }

        if (grabbable.SelectedHandPose)
            return true;
        else
        {
            //yield return new WaitForSeconds(moveDelay);
            return false;
        }
    }

    public void ItemMove()
    {
        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime, Space.World);
    }

    public void ItemLock()
    {
        if (transform.parent.GetComponent<SnapZone>())
        {
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
        }
    }

    public string GetItemTypeName()
    {
        if (itemTypeNames.TryGetValue(type, out string name))
        {
            return name;
        }
        return "�� �� ����";
    }
}
