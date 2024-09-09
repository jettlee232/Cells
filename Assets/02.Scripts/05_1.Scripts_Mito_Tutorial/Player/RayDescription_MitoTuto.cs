using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;
using DG.Tweening;

public class RayDescription_MitoTuto : MonoBehaviour
{
    public GameObject descrptionPanel;
    public Transform descriptionPanelSpawnPoint;

    public GameObject descCanvasPrefab;
    private GameObject instDescCanvas;
    public Transform descCanvasPos;
    public GameObject currentPanel;
    public GameObject glowObj;

    public LineRenderer line;
    public BNG.UIPointer uiPointer;
    public bool isGripPressed = false;
    public bool isButtonPressed = false;
    private bool wasButtonPressed;

    public bool canMakeRayDescription = true;
    public GameObject playerStatus;

    UnityEngine.XR.InputDevice right;

    // SYS Code
    [Header("Particle")]
    public ParticleSystem handPanelParticle;
    public ParticleSystem watchParticle2;
    private bool wasBButtonPressed;

    private bool isSoundPlaying = false;

    [Header("Explain Canvas Tween")]
    public Transform explainCanvas;
    public Transform firstPos;
    public Transform lastPos;
    private Tween moveTween;

    // 이거 중요!!!!!!!!!!!
    public Dictionary<string, string> objDesc = new Dictionary<string, string>
    {
        { "MyATP", "아데노신 삼인산" },
        { "Cristae_Mito", "주름" },
        { "ME Liquid", "막사이공간" },
        { "mitoExterior_Mito", "외막" },
        { "mitoInteriorHalf_Mito", "내막" },
        { "Matrix_Mito", "기질" },
        { "Adenine", "아데닌" },
        { "Ribose", "리보스"},
        { "Phosphate", "인산염" },
        { "ADP", "아데노신 이인산" },
        { "ATP", "아데노신 삼인산" },
        { "H_Ion", "수소이온" },
        { "ATPSynthase", "" }
    };


    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        uiPointer = gameObject.GetComponent<BNG.UIPointer>();
        descrptionPanel = null;

        // SYS Code        
        //handPanelParticle.Stop();
        watchParticle2.Stop();
    }

    void Update()
    {
        // 각 bool값 변수들에 트리거 버튼과 A버튼이 눌리는지 안 눌리는지 실시간으로 받기
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (right.isValid)
        {
            right.TryGetFeatureValue(CommonUsages.gripButton, out isGripPressed);
            right.TryGetFeatureValue(CommonUsages.primaryButton, out isButtonPressed);
        }

        /*
        if (canMakeRayDescription == true)
        {
            if (isButtonPressed == true) // 버튼이 눌리고 있다면
            {
                line.enabled = true; // 레이저 보이게 하기

                CheckRay(transform.position, transform.forward, 10f); // 현재 레이저에 맞은 오브젝트가 뭔지 검사하기           
            }
            else // 트리거가 안 눌리고 있다면
            {
                line.enabled = false; // 레이저 안 보이게 하기    
            }
        }
        */

        if (canMakeRayDescription == true)
        {
            line.enabled = true; // 레이저 보이게 하기
            if (isButtonPressed == true) // 버튼이 눌리고 있다면
            {
                CheckRay(transform.position, transform.forward, 10f); // 현재 레이저에 맞은 오브젝트가 뭔지 검사하기           
            }
        }
        else
        {
            line.enabled = false; // 레이저 안 보이게 하기    
        }

        wasButtonPressed = isButtonPressed;
    }

    public void CheckRay(Vector3 targetPos, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPos, direction);

        if (Physics.Raycast(ray, out RaycastHit rayHit, length))
        {
            if (rayHit.collider.CompareTag("Watch") && playerStatus != null)
            {
                playerStatus.SetActive(!playerStatus.activeSelf);
                canMakeRayDescription = !canMakeRayDescription;
                StartCoroutine(DelayToggleRayStateChange(1.5f));
            }

            ItemExplain_MitoTuto descObj = rayHit.collider.gameObject.GetComponentInParent<ItemExplain_MitoTuto>();

            if (descObj != null)
            {
                if (currentPanel != descObj.explainItemPanel)
                {
                    //instDescCanvas = Instantiate(descCanvasPrefab);
                    //instDescCanvas.transform.position = descObj.transform.position + Vector3.up * 0.5f;
                    //descCanvas.transform.SetParent(descObj.transform);
                    //descCanvas.transform.position = Vector3.zero;

                    currentPanel = descObj.explainItemPanel;
                    descObj.isDesc = true;

                    InstantiatePanel_Tween(currentPanel, descObj.gameObject);
                    canMakeRayDescription = !canMakeRayDescription;
                    StartCoroutine(DelayToggleRayStateChange(1.5f));
                }
            }
        }
    }

    public void InstantiatePanel_Tween(GameObject panel, GameObject rayhit)
    {
        isSoundPlaying = true;
        //handPanelParticle.Play();
        watchParticle2.Play();

        if (descrptionPanel != null)
        {
            glowObj.GetComponent<HighLightColorchange_MitoTuto>().GlowEnd();
            DestroyDescription();
        }

        descrptionPanel = Instantiate(panel);
        descrptionPanel.name = panel.name;
        //descrptionPanel.transform.SetParent(instDescCanvas.transform);
        descrptionPanel.transform.SetParent(descCanvasPos);
        //descrptionPanel.GetComponent<RectTransform>().position = Vector3.zero;
        descrptionPanel.GetComponent<DescriptionTween_Mito>().HLObjInit(rayhit);

        glowObj = rayhit;

        // SYS Code - Explain Canvas Move Tween
        explainCanvas.position = firstPos.position;
        moveTween = explainCanvas.DOLocalMove(lastPos.position, 1f).OnComplete(KillMoveTween);
    }

    // SYS Code
    void KillMoveTween() { moveTween.Kill(); moveTween = null; }
    
    public void DestroyDescription()
    {
        KillMoveTween();
        Destroy(descrptionPanel);
        Destroy(instDescCanvas);
        currentPanel = null;
    }

    IEnumerator DelayToggleRayStateChange(float delay)
    {
        yield return new WaitForSeconds(delay);
        canMakeRayDescription = !canMakeRayDescription;
    }

    /*
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
            glowObj.GetComponent<HighLightColorchange_MitoTuto>().GlowEnd();
        }

        Destroy(descrptionPanel);
        objName = ""; // 현재 가리키는 오브젝트가 없음을 알리기 위해 objName을 비우기
    }

    public void MakeDescription(GameObject go) // 게임 오브젝트의 이름과 종류에 따라 설명창 텍스트를 수정하기
    {
        // 일단 이 코드에서는 GameObject의 name으로 검사하긴 했는데, 다소 무식한 방법이니 Tag든 Layer든 그 외 컴포넌트의 다른 변수 값이든 좀 더 스마트한 조건식을 사용하기를 권장

        // 게임오브젝트의 이름으로 검사해서 설명창을 띄우는 조건식 (추후 서버 연동식이든 json 연동식이든 형식 변경 필요)
        if (objDesc.ContainsKey(go.name))
        {
            descrptionPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = go.name;
            descrptionPanel.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = objDesc[go.name];
        }

        // 현재 패널이 가리키는 오브젝트의 이름을 저장
        objName = descrptionPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text;
    }
    */
}