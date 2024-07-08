using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class CutSceneSkip_CM : MonoBehaviour
{
    //public BNG.MyFader_CM scrFader; // �̰Ŵ� �� ���°� ������?
    public bool isButtonPressed = false;
    UnityEngine.XR.InputDevice right;


    void Start()
    {
        Invoke("FadeOutAndMoveScene", 27f); // �̰� ���ڴ� �ƾ� ���ں��� ���ϸ� ��
    }


    void Update()
    {
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (right.isValid)
        {
            right.TryGetFeatureValue(CommonUsages.primaryButton, out isButtonPressed);
        }

        if (isButtonPressed == true)
        {
            SceneManager.LoadScene(3);
        }
    }

    public void FadeOutAndMoveScene()
    {
        StartCoroutine(MoveScene());
    }

    IEnumerator MoveScene()
    {
        //scrFader.ChangeFadeImageColor(Color.black, 6f, 1f);
        //scrFader.DoFadeIn();

        yield return new WaitForSeconds(0.75f);

        SceneManager.LoadScene(3); // Scene �ε����� �����ϸ� ��
    }
}
