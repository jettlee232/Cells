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

    public bool isGrabbed = false; // 플레이어 손에 잡혔는지 여부
    public bool isInventory = false; // 인벤토리에 있는지 여부

    public Vector3 OriginalScale;

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
        { ItemType.Adenine, "아데닌" },
        { ItemType.Ribose, "리보스" },
        { ItemType.Phosphate, "인산염" },
        { ItemType.ADP, "아데노신이인산" },
        { ItemType.ATP, "아데노신삼인산" },
        { ItemType.H_Ion, "수소이온" },
        { ItemType.FREE, "인벤토리" }
    };

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
            OriginalScale = transform.localScale;
        }
    }

    void Update()
    {
        isGrabbed = CheckGrab();

        if (!isGrabbed) ItemRotate();

        if (GetComponent<Rigidbody>().isKinematic)
        {
            StartCoroutine(ActiveVesicle(0.5f));
        }
        else
        {
            StartCoroutine(DeActiveVesicle(2.0f));
        }
    }

    // 아이템은 항상 움직인다
    // isGrabbed일때는 움직이면 안된다
    // isGrabbed가 false가 되면 한 몇초 뒤에 움직이게 한다
    // 여기서 트윈 사용각?

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

    public void ItemRotate()
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
        return "알 수 없음";
    }

    // 다른 슬롯에 직접 손으로 스냅할때
    // 다른 슬롯에 Ray로 스냅할때
    // Ray로 꺼낼때
    // 3가지 경우에서 순간적으로 스케일이 바뀜
    // SnapZone의 407번째 줄 HeldItem.ResetScale();를 주석처리하면 해결은 가능
    public void ItemGrabScale(float value)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleOverTime(OriginalScale * value, 0.5f));  // 0.5초 동안 크기 변경
    }

    public void ResetScale()
    {
        StartCoroutine(ScaleOverTime(OriginalScale, 2.0f));  // 2초 동안 원래 크기로 돌아옴
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
