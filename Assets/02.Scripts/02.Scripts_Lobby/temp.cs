using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class temp : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float upSpeed = 10f;
    public float downSpeed = 10f;
    public float gravityLimit = 30f;
    //public Transform dirStandard;

    private Ray ray;
    private RaycastHit hit;
    //private CharacterController cc;
    private Rigidbody rb;
    private Transform mainCamera;

    private float antigravity = 0f;
    private float gravity = 0f;
    private bool goUp = false;
    private bool oldGoUp = false;
    private bool goDown = false;
    private bool oldGoDown = false;
    private bool keepDown = false;
    private float startDownTime = 0f;
    private float lastDownTime = 0f;
    private float shortDownTime = 0.5f;

    UnityEngine.XR.InputDevice right;

    void Start()
    {
        //cc = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        getUp();
        //Vector3 mov = new Vector3(0f, antigravity, 0f);
        //cc.Move(mov * Time.deltaTime);
        //transform.Translate(mov * Time.deltaTime * upSpeed);
    }

    private void getUp()
    {
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        right.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out goUp);

        if (goUp) { rb.velocity = Vector3.up * 5f; Debug.Log("GoUp 들어옴"); }

        //if (goUp && !oldGoUp)
        //{
        //    // 키 입력 GetDown과 같은 역할
        //    Debug.Log("버튼 눌렷다");
        //}
        //if (!goUp && oldGoUp)
        //{
        //    // 키 입력 GetUp과 같은 역할
        //    Debug.Log("버튼 뗏다");
        //}
        //oldGoUp = goUp;
    }

    IEnumerator cGoUp()
    {
        float timer = 0f;
        while (antigravity <= upSpeed)
        {
            if (goDown || !goUp) { StartCoroutine(cStopUp()); break; }
            timer += Time.deltaTime;
            antigravity = Mathf.Lerp(0f, upSpeed, timer / 0.3f);
            yield return new WaitForSeconds(0.02f);
        }
        antigravity = upSpeed;
    }
    IEnumerator cStopUp()
    {
        float tempantigravity = antigravity;
        float timer = 0f;
        float totalTime = (antigravity > upSpeed) ? 0.3f : (tempantigravity / upSpeed * 0.3f);
        while (antigravity <= 0.1f)
        {
            timer += Time.deltaTime;
            antigravity = Mathf.Lerp(tempantigravity, 0f, timer / totalTime);
            yield return new WaitForSeconds(0.02f);
        }
        antigravity = 0f;
    }
}
