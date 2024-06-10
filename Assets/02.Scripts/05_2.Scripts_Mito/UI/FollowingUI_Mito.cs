using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class FollowingUI_Mito : MonoBehaviour
{
    public GameObject descrptionPanel = null; // 설명창 오브젝트를 받는 변수
    public GameObject inventoryUI; // 설명창 프리팹
    public Transform descriptionPanelSpawnPoint; // 설명창을 띄우는 위치
    public string objName; // 설명창이 가리키는 오브젝트의 이름

    UnityEngine.XR.InputDevice left;
    UnityEngine.XR.InputDevice right; // 오른손 컨트롤러 상태를 받는 변수

    void Start()
    {
        descrptionPanel = null;
        objName = "";
    }

    void Update()
    {
        left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        bool inventoryButtonPressed;
        left.TryGetFeatureValue(CommonUsages.secondaryButton, out inventoryButtonPressed);
        if (inventoryButtonPressed)
        {
            ToggleInventoryUI();
        }

        if (descrptionPanel != null) // 현재 설명창이 만들어진 상태라면
        {
            FollowingDescription(descrptionPanel); // 현재 만들어진 설명창이 내 시선을 따라오게 하기
        }

    }

    private void ToggleInventoryUI()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
    }

    public void InstantiatePanel(GameObject go)
    {
        if (descrptionPanel != null) // 이미 만들어져 있는 패널이 있다면 그 패널은 지우기
        {
            DestroyDescription();
        }

        // 패널 만들고 위치랑 각도 주기
        descrptionPanel = Instantiate(inventoryUI);

        descrptionPanel.transform.position = descriptionPanelSpawnPoint.position;
        descrptionPanel.transform.rotation = Quaternion.identity;

        // 패널의 초기 크기는 작게 설정하기
        //descrptionPanel.GetComponent<RectTransform>().localScale = new Vector3(0.0002f, 0.0002f, 0.0002f);

        // 오브젝트 설명 띄우기
        //MakeDescription(go);
    }


    public void FollowingDescription(GameObject descPanel) // 패널이 플레이어 시선 따라가게 하기
    {
        // 이 부분은 동적인 효과를 위해 넣은건데 사실 없어도 되긴 할듯하네요... 보고 필요 없다 싶으면 빼도 됩니다
        //if (descPanel.GetComponent<RectTransform>().localScale.x < 0.002f)
        //{
        //    descPanel.GetComponent<RectTransform>().localScale =
        //    new Vector3(descPanel.GetComponent<RectTransform>().localScale.x + 0.0005f,
        //    descPanel.GetComponent<RectTransform>().localScale.y + 0.0005f,
        //    descPanel.GetComponent<RectTransform>().localScale.z + 0.0005f);
        //}
        //

        // 패널의 위치와 각도가 패널스폰포인트의 위치와 각도와 일치하도록 강제로 실시간 고정
        descPanel.transform.position = descriptionPanelSpawnPoint.position;
        descPanel.transform.rotation = descriptionPanelSpawnPoint.rotation;
    }

    public void DestroyDescription() // 패널 없애기
    {
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
