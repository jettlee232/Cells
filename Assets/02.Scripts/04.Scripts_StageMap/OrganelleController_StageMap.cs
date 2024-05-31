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

    public float minScale = 0.5f; // 최소 크기
    public float maxScale = 0.7f; // 최대 크기
    public float minDistance = 1f; // 최소 거리
    public float maxDistance = 20f; // 최대 거리

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).gameObject;
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
            // 여기에 머 방향 돌리고 그런 거 구현해야 함
            RaycastHit hit;
            Vector3 closest = this.gameObject.GetComponent<Collider>().ClosestPoint(player.transform.position);
            Vector3 vDistance = (closest - player.transform.position);
            Vector3 vNDistance = vDistance.normalized;
            float fDistance = vDistance.magnitude;
            bool tooClose = false;
            
            //Vector3.up * (myUI.GetComponent<RectTransform>().rect.height)
            //if ((distance >= 50f) || (Physics.Raycast(player.transform.position, (this.transform.position - player.transform.position).normalized, out hit, distance))) { myUI.gameObject.SetActive(false); }
            if (fDistance < 1f) { tooClose = true; }
            if (fDistance >= 20f) { myUI.gameObject.SetActive(false); }
            else
            {
                float scale = 0f;
                myUI.gameObject.SetActive(true);
                if (tooClose) { scale = 0.005f * maxScale; }
                else { scale = (Mathf.Clamp(1 - (fDistance / maxDistance), minScale, maxScale)) * 0.005f; }
                myUI.gameObject.transform.localScale = new Vector3(scale, scale, scale);
            }

            if (tooClose)
            {
                myUI.transform.position = closest + (GetLocalYAxis(vNDistance) * (myUI.GetComponent<RectTransform>().rect.height) * (myUI.gameObject.transform.localScale.x));
            }
            else { myUI.transform.position = closest - vNDistance + (GetLocalYAxis(vNDistance) * (myUI.GetComponent<RectTransform>().rect.height) * (myUI.gameObject.transform.localScale.x)); }

            myUI.transform.LookAt(player.transform);
            myUI.transform.Rotate(0, 180, 0);

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
}
