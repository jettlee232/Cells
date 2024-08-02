using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GetMovement_Lobby : MonoBehaviour
{
    private InputDevice leftController;
    private bool isJoystickActive = true;
    private float joystickActiveTime = 0f;
    private float thresholdTime = .4f;

    public GameManager_Lobby gameMgr;

    [Header("Tooltips")]
    public Tooltip[] tooltips;

    [Header("FollowPanels")]
    public UIManager_Lobby uIManager;

    void Update()
    {
        leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        Vector2 joystickValue;

        if (leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out joystickValue))
        {
            if (isJoystickActive == true)
            {
                if (joystickValue != Vector2.zero)
                {
                    joystickActiveTime += Time.deltaTime;
                    if (joystickActiveTime >= thresholdTime)
                    {
                        isJoystickActive = false; // 타이머 중지            
                    }
                }
            }                            
            else
            {
                tooltips[0].TooltipOff();
                uIManager.HideMoveTutorial();
                gameMgr.UnShowingTooltipAnim(0);
                this.enabled = false;
            }
        }
    }

    /*
    private IEnumerator JoystickTimer()
    {
        while (isJoystickActive)
        {
            joystickActiveTime += Time.deltaTime;

            if (joystickActiveTime >= thresholdTime)
            {
                Debug.Log("DDD");
                joystickActiveTime = 0f; // 타이머 초기화
                isJoystickActive = false; // 타이머 중지
                yield break;
            }

            yield return null;
        }
    }
    */
}
