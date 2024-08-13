using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class LaserPointer_Lys_Game : MonoBehaviour
{
    private BNG.UIPointer uiPointer; // ������ ������Ʈ
    private LineRenderer lineRenderer;
    public bool HideWeapon = false;
    public GameObject aiUI;
    public GameObject mapUI;

    private GameObject player;

    private bool active = false;        // ai â, �� �̵� â Ȱ��ȭ �Ǿ��ִ���... true�� �� �� �ϳ��� ������
    private bool oldActive = false;

    void Start()
    {
        uiPointer = gameObject.GetComponent<BNG.UIPointer>(); // UI ������Ʈ �ޱ�
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        player = GameManager_Lys_Game.instance.GetPlayer();
    }

    void Update()
    {
        if (aiUI.activeSelf || mapUI.activeSelf) { active = true; }
        else { active = false; }

        if (active && !oldActive) { SetPointerActive(); }
        else if (!active && oldActive) { SetPointerInActive(); }

        oldActive = active;
    }

    void SetPointerActive()
    {
        lineRenderer.enabled = true;
        uiPointer.enabled = true;
        uiPointer.HidePointerIfNoObjectsFound = false;
        if (HideWeapon)
        {
            player.GetComponent<WeaponController_Lys>().HideWeapon();
            player.GetComponent<WeaponController_Lys>().enabled = false;
        }
    }
    void SetPointerInActive()
    {
        uiPointer.HidePointerIfNoObjectsFound = true;
        uiPointer.enabled = false;
        lineRenderer.enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        if (HideWeapon)
        {
            player.GetComponent<WeaponController_Lys>().ShowWeapon();
            player.GetComponent<WeaponController_Lys>().enabled = true;
        }
    }
}
