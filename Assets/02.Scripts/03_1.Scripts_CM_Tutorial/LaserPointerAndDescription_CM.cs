using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;


public class LaserPointerAndDescription_CM : MonoBehaviour
{
    public GameObject descrptionPanel = null;
    public GameObject laserDescriptionCanvas;

    public BNG.UIPointer uiPointer;

    private LineRenderer rendo;

    UnityEngine.XR.InputDevice right;
    UnityEngine.XR.InputDevice left;

    public bool isPressed = false;

    void Start()
    {        
        uiPointer = gameObject.GetComponent<BNG.UIPointer>();

        rendo = gameObject.GetComponent<LineRenderer>();

        descrptionPanel = null;
    }
    
    void Update()
    {
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        right.TryGetFeatureValue(CommonUsages.triggerButton, out isPressed);

        // 1��        
        if (isPressed == true)
        {
            //uiPointer.HidePointerIfNoObjectsFound = false; // �������� ���߰� �ٸ� ��ư���� ��ũ������ ���� ������� �缳���ؾ��ҵ�
            FloatingDescription(transform.position, transform.forward, 5f);
        }
        else 
        { 
            //uiPointer.HidePointerIfNoObjectsFound = true;
            DestroyDescription();
        }
    }
    

    public void FloatingDescription(Vector3 targetPos, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPos, direction);
        //Vector3 endPos = targetPos + (direction * length);

        if (Physics.Raycast(ray, out RaycastHit rayHit, length))
        {
            //endPos = rayHit.point;

            // ������ ���ǽ� �˻簡 �ʿ��� (������ ����� �Ǵ� ������Ʈ�� ���� �˻�)
            if (rayHit.collider.gameObject.layer == LayerMask.NameToLayer("DescObj"))
            {
                // �������� �������� ��ȣ�ۿ�
                if (descrptionPanel == null)
                {
                    descrptionPanel = Instantiate(laserDescriptionCanvas);
                    descrptionPanel.transform.position = new Vector3(rayHit.collider.gameObject.transform.position.x + 0.5f,
                        rayHit.collider.gameObject.transform.position.y + 0.5f,
                        rayHit.collider.gameObject.transform.position.z);

                    MakeDescription(rayHit.collider.gameObject);
                    //descrptionPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = rayHit.collider.gameObject.name;
                }
            }
        }

        //rendo.SetPosition(0, targetPos);
        //rendo.SetPosition(1, endPos);
    }

    public void DestroyDescription()
    {
        Destroy(descrptionPanel);
    }

    public void MakeDescription(GameObject go)
    {
        // ���ӿ�����Ʈ�� �̸����� �˻��ؼ� ����â�� ���� ���ǽ� (���� ���� �������̵� json �������̵� ���� ���� �ʿ�)
        if (go.name == "Cube1")
        {
            descrptionPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Cube1";
            descrptionPanel.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "mucha gracias aficon esto es para vosotoros";
        }
        else if (go.name == "Cube2")
        {
            descrptionPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Cube2";
            descrptionPanel.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "siuuuuuuuuuuu";
        }
    }
}
