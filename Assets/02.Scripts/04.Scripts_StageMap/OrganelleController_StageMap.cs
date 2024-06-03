using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrganelleController_StageMap : MonoBehaviour
{
    public GameObject OC_UI;
    public string OC_Name = "이름이름이름";   // 임시
    public string OC_Description = "설명설명설명설명설명설명설명설명 설명설명설명설명설명설명설명설명 설명설명설명설명설명설명설명설명 설명설명설명설명설명설명설명설명";   // 임시
    public bool showUI = false;

    private GameObject myUI = null;
    private GameObject player = null;
    private GameObject playerCam = null;
    private Camera mainCam;

    public float minScale = 0.5f; // 최소 크기
    public float maxScale = 0.7f; // 최대 크기
    public float minDistance = 1f; // 최소 거리
    public float maxDistance = 20f; // 최대 거리

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).gameObject;
        playerCam = player.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        mainCam = playerCam.GetComponent<Camera>();
        StartCoroutine(cGenerateUI());
    }

    IEnumerator cGenerateUI()
    {
        myUI = Instantiate(OC_UI, Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform);

        while (myUI == null) { yield return new WaitForSeconds(0.02f); }
        myUI.transform.GetChild(0).gameObject.GetComponent<Text>().text = OC_Name;
        myUI.transform.GetChild(1).gameObject.GetComponent<Text>().text = OC_Description;
    }

    public void fShowUI()
    {
        myUI.gameObject.SetActive(true);
        showUI = true;
        StartCoroutine(cShowUI());
    }
    public void fHideUI() { showUI = false; }
    IEnumerator cShowUI()
    {
        while (true)
        {
            RaycastHit hit;
            Vector3 closest = this.gameObject.GetComponent<Collider>().ClosestPoint(player.transform.position);
            Vector3 vDistance = (closest - player.transform.position);
            Vector3 vNDistance = vDistance.normalized;
            float fDistance = vDistance.magnitude;

            //if ((distance >= 50f) || (Physics.Raycast(player.transform.position, (this.transform.position - player.transform.position).normalized, out hit, distance))) { myUI.gameObject.SetActive(false); }

            Vector3 viewportPos = mainCam.WorldToViewportPoint(this.transform.position);
            //bool isInView = viewportPos.z > 0.15f && (viewportPos.x > 0.15f && viewportPos.x < 0.85f) && (viewportPos.y > 0.15f && viewportPos.y < 0.85f);
            bool isInView = viewportPos.z > 0f && (viewportPos.x > 0f && viewportPos.x < 1f) && (viewportPos.y > 0f && viewportPos.y < 1f);

            Bounds bounds = this.gameObject.GetComponent<Renderer>().bounds;
            Vector3 minPoint = mainCam.WorldToViewportPoint(new Vector3(bounds.min.x, bounds.min.y, bounds.min.z));
            Vector3 maxPoint = mainCam.WorldToViewportPoint(new Vector3(bounds.max.x, bounds.max.y, bounds.max.z));

            //if (isInView && (maxPoint.x <= 0.15f || minPoint.x >= 0.85f || maxPoint.y <= 0.15f || minPoint.y >= 0.85f)) isInView = false;

            if (fDistance >= 20f || !isInView) { myUI.gameObject.SetActive(false); }
            else
            {
                float scale = 0f;
                myUI.gameObject.SetActive(true);
                scale = (Mathf.Clamp(1 - (fDistance / maxDistance), minScale, maxScale)) * 0.0002f;
                myUI.gameObject.transform.localScale = new Vector3(scale, scale, scale);
            }
            myUI.transform.rotation = playerCam.transform.rotation;
            //myUI.transform.position = playerCam.transform.position + 0.3f * playerCam.transform.forward + GetAddVector(minPoint, maxPoint);
            myUI.transform.position = playerCam.transform.position + 0.3f * playerCam.transform.forward;

            if (showUI == false) { break; }
            yield return new WaitForSeconds(0.02f);
        }
        myUI.gameObject.SetActive(false);
    }

    private Vector3 GetLocalYAxis(Vector3 dir)
    {
        Vector3 right = Vector3.Cross(dir, Vector3.up);
        if (right == Vector3.zero) { right = Vector3.Cross(dir, Vector3.forward); }
        Vector3 localUp = Vector3.Cross(right, dir);

        return localUp.normalized;
    }

    private Vector3 GetAddVector(Vector3 minPoint, Vector3 maxPoint)
    {
        Vector3 addDir = Vector3.zero;

        float width = maxPoint.x - minPoint.x;
        float height = maxPoint.y - minPoint.y;
        bool plusX = maxPoint.x + minPoint.x >= 0.5f ? true : false;
        bool plusY = maxPoint.y + minPoint.y >= 0.5f ? true : false;

        if (width <= 0.7 && height <= 0.7f)
        {
            if (plusX)
            {
                if (plusY) { addDir = 0.02f * (-playerCam.transform.up - playerCam.transform.right); }
                else { addDir = 0.02f * (playerCam.transform.up - playerCam.transform.right); }
            }
            else
            {
                if (plusY) { addDir = 0.02f * (-playerCam.transform.up + playerCam.transform.right); }
                else { addDir = 0.02f * (playerCam.transform.up + playerCam.transform.right); }
            }
        }
        return addDir;
    }
}
