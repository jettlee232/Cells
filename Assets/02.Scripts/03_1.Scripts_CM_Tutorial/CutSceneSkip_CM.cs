using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class CutSceneSkip_CM : MonoBehaviour
{
    //public BNG.MyFader_CM scrFader; // 이거는 안 쓰는게 나을듯?
    public bool isButtonPressed = false;
    UnityEngine.XR.InputDevice right;


    void Start()
    {
        Invoke("FadeOutAndMoveScene", 27f); // 이거 숫자는 컷씬 숫자보고 정하면 됨
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

        SceneManager.LoadScene(3); // Scene 인덱스도 수정하면 됨
    }
}
