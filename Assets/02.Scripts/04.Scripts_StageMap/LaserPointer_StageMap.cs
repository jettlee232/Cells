using HighlightPlus;
using Language.Lua;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR;

public class LaserPointer_StageMap : MonoBehaviour
{
    public BNG.UIPointer uiPointer; // ������ ������Ʈ
    public bool isTriggerPressed = false; // Ʈ���Ű� �������� �� ��������
    public bool isButtonPressed = false; // ������ A��ư�� �������� �� ��������
    public float maxDistance;

    public GameObject obj = null;
    private GameObject descPanel = null;
    private int descObjLayer;
    private GameObject player = null;
    private Camera mainCam = null;
    private GameObject NPC = null;

    UnityEngine.XR.InputDevice right; // ������ ��Ʈ�ѷ� ���¸� �޴� ����

    public GameObject glowObj;
    private HighlightEffect highlightEffect;

    void Start()
    {
        uiPointer = gameObject.GetComponent<BNG.UIPointer>(); // UI ������Ʈ �ޱ�
        descPanel = UIManager_StageMap.instance.GetDesc();
        obj = null;
        descObjLayer = 1 << LayerMask.NameToLayer("DescObj");
        player = GameManager_StageMap.instance.GetPlayer();
        mainCam = GameManager_StageMap.instance.GetPlayerCam().GetComponent<Camera>();
        NPC = GameManager_StageMap.instance.GetNPC();
    }

    void Update()
    {
        // �� bool�� �����鿡 Ʈ���� ��ư�� A��ư�� �������� �� �������� �ǽð����� �ޱ�        
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        right.TryGetFeatureValue(CommonUsages.triggerButton, out isTriggerPressed);
        right.TryGetFeatureValue(CommonUsages.primaryButton, out isButtonPressed);

        if (isTriggerPressed) // Ʈ���Ű� ������ �ִٸ�
        {
            uiPointer.HidePointerIfNoObjectsFound = false; // ������ ���̰� �ϱ�
            CheckRay(transform.position, transform.forward, 10f); // ���� �������� ���� ������Ʈ�� ���� �˻��ϱ�
        }
        else { uiPointer.HidePointerIfNoObjectsFound = true; }

        if (descPanel.activeSelf) // ���� ����â�� ������� ���¶��
        {
            FollowingDescription(UIManager_StageMap.instance.GetDesc()); // ���� ������� ����â�� �� �ü��� ������� �ϱ�
            if (!CheckSight()) { DestroyDescription(); }
        }
    }

    public void CheckRay(Vector3 targetPos, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPos, direction.normalized);

        if (Physics.Raycast(ray, out RaycastHit rayHit, length))
        {
            if (rayHit.collider.gameObject.layer == LayerMask.NameToLayer("DescObj"))
            {
                if (obj != rayHit.collider.gameObject)
                {
                    obj = rayHit.collider.gameObject;

                    highlightEffect = obj.GetComponent<HighlightEffect>();
                    if (highlightEffect != null)
                    {
                        highlightEffect.highlighted = true;
                        rayHit.collider.gameObject.GetComponent<HighLightColorchange_SM>().GlowStart();
                    }

                    InstantiatePanel(obj);
                }
            }
            else if (rayHit.collider.gameObject.CompareTag("NPC"))
            {
                if (GameManager_StageMap.instance.secondCon)
                {
                    // �ι�° ��ȭ ���� ����
                    NPC.GetComponent<SelectDialogue_StageMap>().ActivateDST2();
                }
                else { NPC.GetComponent<SelectDialogue_StageMap>().ActivateDST1(); }
            }
        }
    }

    public void InstantiatePanel(GameObject go)
    {
        if (descPanel.activeSelf) { DestroyDescription(); }

        UIManager_StageMap.instance.OnDesc();
        descPanel.GetComponent<RectTransform>().localScale = Vector3.one * 0.00125f;
        MakeDescription(go);

        glowObj = go;
    }

    public void FollowingDescription(GameObject descPanel) // �г��� �÷��̾� �ü� ���󰡰� �ϱ�
    {
        if (descPanel.GetComponent<RectTransform>().localScale.x < 0.00125f)
        {
            descPanel.GetComponent<RectTransform>().localScale =
            new Vector3(descPanel.GetComponent<RectTransform>().localScale.x + 0.00001f,
            descPanel.GetComponent<RectTransform>().localScale.y + 0.00001f,
            descPanel.GetComponent<RectTransform>().localScale.z + 0.00001f);
        }
    }

    public void DestroyDescription() // �г� ���ֱ�
    {
        UIManager_StageMap.instance.OffDesc();
        obj = null;
    }

    public void MakeDescription(GameObject go) // ���� ������Ʈ�� �̸��� ������ ���� ����â �ؽ�Ʈ�� �����ϱ�
    {
        descPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = go.GetComponent<DescObj_StageMap>().GetName();
        descPanel.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = go.GetComponent<DescObj_StageMap>().GetDesc();
        descPanel.transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => GameManager_StageMap.instance.MoveScene(go.GetComponent<DescObj_StageMap>().GetSceneName()));
    }

    private bool CheckSight()
    {
        if (obj == null || mainCam == null || obj.transform == null) { DestroyDescription(); return false; }
        else
        {
            Vector3 viewportPos = mainCam.WorldToViewportPoint(obj.transform.position);

            bool isInView = viewportPos.z > 0f && (viewportPos.x > 0f && viewportPos.x < 1f) && (viewportPos.y > 0f && viewportPos.y < 1f);

            Vector3 closest = obj.GetComponent<Collider>().ClosestPoint(player.transform.position);
            Vector3 vDistance = (closest - player.transform.position);
            bool isClose = (vDistance.magnitude <= maxDistance) ? true : false;

            if (isInView && isClose) { return true; }
            else { return false; }
        }
    }
}
