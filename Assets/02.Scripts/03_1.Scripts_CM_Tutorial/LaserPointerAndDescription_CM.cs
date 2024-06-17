using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;


public class LaserPointerAndDescription_CM : MonoBehaviour
{
    public GameObject descrptionPanel = null;
    public GameObject laserDescriptionCanvas;
    public Transform descriptionPanelSpawnPoint;
    public string objName;

    public BNG.UIPointer uiPointer;
    public bool isTriggerPressed = false;
    public bool isButtonPressed = false;

    UnityEngine.XR.InputDevice right;

    public GameObject glowObj;

    void Start()
    {        
        uiPointer = gameObject.GetComponent<BNG.UIPointer>();
        descrptionPanel = null;

        objName = "";        
    }

    void Update()
    {
        // 각 bool값 변수들에 트리거 버튼과 A버튼이 눌리는지 안 눌리는지 실시간으로 받기
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        right.TryGetFeatureValue(CommonUsages.triggerButton, out isTriggerPressed);
        right.TryGetFeatureValue(CommonUsages.primaryButton, out isButtonPressed);

        if (isTriggerPressed == true) // 트리거가 눌리고 있다면
        {
            uiPointer.HidePointerIfNoObjectsFound = false; // 레이저 보이게 하기

            CheckRay(transform.position, transform.forward, 10f); // 현재 레이저에 맞은 오브젝트가 뭔지 검사하기           
        }
        else // 트리거가 안 눌리고 있다면
        {
            uiPointer.HidePointerIfNoObjectsFound = true; // 레이저 안 보이게 하기    
        }

        if (descrptionPanel != null) // 현재 설명창이 안 만들어진 상태라면
        {
            FollowingDescription(descrptionPanel); // 현재 만들어진 설명창이 내 시선을 따라오게 하기
        }
        if (isButtonPressed == true && descrptionPanel != null) // 현재 설명창이 만들어진 상태이고 A버튼이 눌린 상태라면
        {
            DestroyDescription(); // 현재 만들어진 설명창을 없애기
        }
    }

    public void CheckRay(Vector3 targetPos, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPos, direction);

        if (Physics.Raycast(ray, out RaycastHit rayHit, length))
        {
            // 오른손 손가락 전방으로 레이저를 쏴서 DescObj라는 Layer를 가진 오브젝트라면...
            if (rayHit.collider.gameObject.layer == LayerMask.NameToLayer("DescObj"))
            {                
                // 현재 설명창이 가리키는 오브젝트의 이름과 레이저가 맞은 오브젝트의 이름도 다르다면...
                // (설명창이 닫힌 상태라면 objName에는 아무것도 안 적혀있을거임)
                if (objName != rayHit.collider.gameObject.name)
                {
                    objName = rayHit.collider.gameObject.name; // objName에다 레이저에 맞은 오브젝트의 이름을 넣기

                    //glowObj = rayHit.collider.gameObject;
                    rayHit.collider.gameObject.GetComponent<HighlightEffect>().highlighted = true;
                    rayHit.collider.gameObject.GetComponent<HighLightColorchange_CM>().GlowStart();

                    InstantiatePanel(rayHit.collider.gameObject); // 패널 만들기
                }
            }
        }
    }

    public void InstantiatePanel(GameObject go)
    {
        if (descrptionPanel != null) // 이미 만들어져 있는 패널이 있다면 그 패널은 지우기
        {
            DestroyDescription();
        }

        // 패널 만들고 위치랑 각도 주기
        descrptionPanel = Instantiate(laserDescriptionCanvas);

        descrptionPanel.transform.position = descriptionPanelSpawnPoint.position;
        descrptionPanel.transform.rotation = Quaternion.identity;

        // 패널의 초기 크기는 작게 설정하기
        descrptionPanel.GetComponent<RectTransform>().localScale = new Vector3(0.0002f, 0.0002f, 0.0002f);

        // 오브젝트 설명 띄우기
        MakeDescription(go);

        glowObj = go;
    }

    
    public void FollowingDescription(GameObject descPanel) // 패널이 플레이어 시선 따라가게 하기
    {
        // 이 부분은 동적인 효과를 위해 넣은건데 사실 없어도 되긴 할듯하네요... 보고 필요 없다 싶으면 빼도 됩니다
        if (descPanel.GetComponent<RectTransform>().localScale.x < 0.002f)
        {
            descPanel.GetComponent<RectTransform>().localScale =
            new Vector3(descPanel.GetComponent<RectTransform>().localScale.x + 0.0005f,
            descPanel.GetComponent<RectTransform>().localScale.y + 0.0005f,
            descPanel.GetComponent<RectTransform>().localScale.z + 0.0005f);
        }
        //
        
        // 패널의 위치와 각도가 패널스폰포인트의 위치와 각도와 일치하도록 강제로 실시간 고정
        descPanel.transform.position = descriptionPanelSpawnPoint.position;
        descPanel.transform.rotation = descriptionPanelSpawnPoint.rotation;
    }

    public void DestroyDescription() // 패널 없애기
    {
        if (glowObj != null)
        {
            glowObj.GetComponent<HighlightEffect>().highlighted = false;
            glowObj.GetComponent<HighLightColorchange_CM>().GlowEnd();
        }
                       
        Destroy(descrptionPanel);
        objName = ""; // 현재 가리키는 오브젝트가 없음을 알리기 위해 objName을 비우기
    }

    public void MakeDescription(GameObject go) // 게임 오브젝트의 이름과 종류에 따라 설명창 텍스트를 수정하기
    {
        // 일단 이 코드에서는 GameObject의 name으로 검사하긴 했는데, 다소 무식한 방법이니 Tag든 Layer든 그 외 컴포넌트의 다른 변수 값이든 좀 더 스마트한 조건식을 사용하기를 권장

        // 게임오브젝트의 이름으로 검사해서 설명창을 띄우는 조건식 (추후 서버 연동식이든 json 연동식이든 형식 변경 필요)
        if (go.name == "Cube1")
        {
            descrptionPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Cube1";
            descrptionPanel.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "My Name Is Cube 1";
        }
        else if (go.name == "Cube2")
        {
            descrptionPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Cube2";
            descrptionPanel.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "My Name Is Cube 2";
        }

        // 현재 패널이 가리키는 오브젝트의 이름을 저장
        objName = descrptionPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text;
    }
}
