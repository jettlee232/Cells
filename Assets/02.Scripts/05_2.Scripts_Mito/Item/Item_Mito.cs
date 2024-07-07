using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Mito : MonoBehaviour
{
    //float moveSpeed = 0.25f;
    float rotSpeed = 180.0f;
    //public float moveDelay = 2.0f;
    //private float destroyTime = 30.0f;

    public bool isGrabbed = false; // �÷��̾� �տ� �������� ����
    public bool isInventory = false; // �κ��丮�� �ִ��� ����

    public Vector3 OriginalScale; // ������ ��� ����

    Grabbable grabbable;

    // �±� ��ſ� enum Ÿ������ ������ ���� ����
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
        { ItemType.Phosphate, "�λ꿰" },
        { ItemType.ADP, "�Ƶ�������λ�" },
        { ItemType.ATP, "�Ƶ���Ż��λ�" },
        { ItemType.H_Ion, "�����̿�" },
        { ItemType.FREE, "�κ��丮" }
    };

    // ������ �����Ϸ� ���� ������ ����
    public void SetOriginalScale(Vector3 scale)
    {
        OriginalScale = scale;
        transform.localScale = scale;
    }

    void Start()
    {
        grabbable = GetComponent<Grabbable>();
        if (OriginalScale == Vector3.zero)
        {
            OriginalScale = transform.localScale; // ���� �������� ������ �̸� ����
        }
    }

    void Update()
    {
        isGrabbed = CheckGrab();

        if (!isGrabbed) ItemRotate();

        if (GetComponent<Rigidbody>().isKinematic) // �������� �������� ���θ� isKinematic���� Ȯ��
        {
            StartCoroutine(ActiveVesicle(0.5f));
        }
        else
        {
            StartCoroutine(DeActiveVesicle(2.0f));
        }
    }

    // �������� �׻� �����δ�
    // isGrabbed�϶��� �����̸� �ȵȴ�
    // isGrabbed�� false�� �Ǹ� �� ���� �ڿ� �����̰� �Ѵ�
    // ���⼭ Ʈ�� ��밢?
    // ����

    // �������� �������� ���� Ȯ���ؼ� ����
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

    // y������ ȸ��
    public void ItemRotate()
    {
        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime, Space.World);
    }

    /*
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
    */

    // �ٸ� ���Կ� ���� ������ �����Ҷ�
    // �ٸ� ���Կ� Ray�� �����Ҷ�
    // Ray�� ������
    // 3���� ��쿡�� ���������� �������� �ٲ�
    // SnapZone�� 407��° �� HeldItem.ResetScale();�� �ּ�ó���ϸ� �ذ��� ����?
    // �ٵ� �ڽ��� Ring�� ������
    public void ItemGrabScale(float value)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleOverTime(OriginalScale * value, 0.5f));  // 0.5�� ���� ũ�� ����
    }

    public void ResetScale()
    {
        StartCoroutine(ScaleOverTime(OriginalScale, 2.0f));  // 2�� ���� ���� ũ��� ���ƿ�
    }

    private IEnumerator ScaleOverTime(Vector3 targetScale, float duration)
    {
        Vector3 initialScale = transform.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }

    // �����۵��� ù��° �ڽ��� ��ȣ�� SetActive ���� �ڷ�ƾ
    IEnumerator ActiveVesicle(float delay)
    {
        yield return new WaitForSeconds(delay);
        transform.GetChild(0).gameObject.SetActive(true);
    }

    IEnumerator DeActiveVesicle(float delay)
    {
        yield return new WaitForSeconds(delay);
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
