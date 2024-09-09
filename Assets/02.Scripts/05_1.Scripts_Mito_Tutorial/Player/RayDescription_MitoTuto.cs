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

    // �̰� �߿�!!!!!!!!!!!
    public Dictionary<string, string> objDesc = new Dictionary<string, string>
    {
        { "MyATP", "�Ƶ���� ���λ�" },
        { "Cristae_Mito", "�ָ�" },
        { "ME Liquid", "�����̰���" },
        { "mitoExterior_Mito", "�ܸ�" },
        { "mitoInteriorHalf_Mito", "����" },
        { "Matrix_Mito", "����" },
        { "Adenine", "�Ƶ���" },
        { "Ribose", "������"},
        { "Phosphate", "�λ꿰" },
        { "ADP", "�Ƶ���� ���λ�" },
        { "ATP", "�Ƶ���� ���λ�" },
        { "H_Ion", "�����̿�" },
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
        // �� bool�� �����鿡 Ʈ���� ��ư�� A��ư�� �������� �� �������� �ǽð����� �ޱ�
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (right.isValid)
        {
            right.TryGetFeatureValue(CommonUsages.gripButton, out isGripPressed);
            right.TryGetFeatureValue(CommonUsages.primaryButton, out isButtonPressed);
        }

        /*
        if (canMakeRayDescription == true)
        {
            if (isButtonPressed == true) // ��ư�� ������ �ִٸ�
            {
                line.enabled = true; // ������ ���̰� �ϱ�

                CheckRay(transform.position, transform.forward, 10f); // ���� �������� ���� ������Ʈ�� ���� �˻��ϱ�           
            }
            else // Ʈ���Ű� �� ������ �ִٸ�
            {
                line.enabled = false; // ������ �� ���̰� �ϱ�    
            }
        }
        */

        if (canMakeRayDescription == true)
        {
            line.enabled = true; // ������ ���̰� �ϱ�
            if (isButtonPressed == true) // ��ư�� ������ �ִٸ�
            {
                CheckRay(transform.position, transform.forward, 10f); // ���� �������� ���� ������Ʈ�� ���� �˻��ϱ�           
            }
        }
        else
        {
            line.enabled = false; // ������ �� ���̰� �ϱ�    
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
        if (descrptionPanel != null) // �̹� ������� �ִ� �г��� �ִٸ� �� �г��� �����
        {
            DestroyDescription();
        }

        // �г� ����� ��ġ�� ���� �ֱ�
        descrptionPanel = Instantiate(laserDescriptionCanvas);

        descrptionPanel.transform.position = descriptionPanelSpawnPoint.position;
        descrptionPanel.transform.rotation = Quaternion.identity;

        // �г��� �ʱ� ũ��� �۰� �����ϱ�
        descrptionPanel.GetComponent<RectTransform>().localScale = new Vector3(0.0002f, 0.0002f, 0.0002f);

        // ������Ʈ ���� ����
        MakeDescription(go);

        glowObj = go;
    }


    public void FollowingDescription(GameObject descPanel) // �г��� �÷��̾� �ü� ���󰡰� �ϱ�
    {
        // �� �κ��� ������ ȿ���� ���� �����ǵ� ��� ��� �Ǳ� �ҵ��ϳ׿�... ���� �ʿ� ���� ������ ���� �˴ϴ�
        if (descPanel.GetComponent<RectTransform>().localScale.x < 0.002f)
        {
            descPanel.GetComponent<RectTransform>().localScale =
            new Vector3(descPanel.GetComponent<RectTransform>().localScale.x + 0.0005f,
            descPanel.GetComponent<RectTransform>().localScale.y + 0.0005f,
            descPanel.GetComponent<RectTransform>().localScale.z + 0.0005f);
        }
        //

        // �г��� ��ġ�� ������ �гν�������Ʈ�� ��ġ�� ������ ��ġ�ϵ��� ������ �ǽð� ����
        descPanel.transform.position = descriptionPanelSpawnPoint.position;
        descPanel.transform.rotation = descriptionPanelSpawnPoint.rotation;
    }

    public void DestroyDescription() // �г� ���ֱ�
    {
        if (glowObj != null)
        {
            glowObj.GetComponent<HighlightEffect>().highlighted = false;
            glowObj.GetComponent<HighLightColorchange_MitoTuto>().GlowEnd();
        }

        Destroy(descrptionPanel);
        objName = ""; // ���� ����Ű�� ������Ʈ�� ������ �˸��� ���� objName�� ����
    }

    public void MakeDescription(GameObject go) // ���� ������Ʈ�� �̸��� ������ ���� ����â �ؽ�Ʈ�� �����ϱ�
    {
        // �ϴ� �� �ڵ忡���� GameObject�� name���� �˻��ϱ� �ߴµ�, �ټ� ������ ����̴� Tag�� Layer�� �� �� ������Ʈ�� �ٸ� ���� ���̵� �� �� ����Ʈ�� ���ǽ��� ����ϱ⸦ ����

        // ���ӿ�����Ʈ�� �̸����� �˻��ؼ� ����â�� ���� ���ǽ� (���� ���� �������̵� json �������̵� ���� ���� �ʿ�)
        if (objDesc.ContainsKey(go.name))
        {
            descrptionPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = go.name;
            descrptionPanel.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = objDesc[go.name];
        }

        // ���� �г��� ����Ű�� ������Ʈ�� �̸��� ����
        objName = descrptionPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text;
    }
    */
}