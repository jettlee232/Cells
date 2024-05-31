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

        // 1안        
        if (isPressed == true)
        {
            //uiPointer.HidePointerIfNoObjectsFound = false; // 레이저는 감추고 다른 버튼으로 디스크립션을 띄우는 방식으로 재설계해야할듯
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

            // 별도의 조건식 검사가 필요함 (설명을 띄워야 되는 오브젝트에 대한 검사)
            if (rayHit.collider.gameObject.layer == LayerMask.NameToLayer("DescObj"))
            {
                // 레이저로 맞출경우의 상호작용
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
        // 게임오브젝트의 이름으로 검사해서 설명창을 띄우는 조건식 (추후 서버 연동식이든 json 연동식이든 형식 변경 필요)
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
