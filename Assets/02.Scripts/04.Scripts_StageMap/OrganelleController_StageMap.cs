using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrganelleController_StageMap : MonoBehaviour
{
    public GameObject OC_UI;
    public string OC_Name = "�̸��̸��̸�";   // �ӽ�
    public string OC_Description = "������������������ ������������������ ������������������ ������������������";   // �ӽ�
    public bool showUI = false;

    private GameObject myUI = null;
    private GameObject player = null;

    public float minScale = 0.2f; // �ּ� ũ��
    public float maxScale = 1f; // �ִ� ũ��
    public float minDistance = 1f; // �ּ� �Ÿ�
    public float maxDistance = 50f; // �ִ� �Ÿ�

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(cGenerateUI());
    }

    IEnumerator cGenerateUI()
    {
        myUI = Instantiate(OC_UI, Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform);

        while (myUI = null) { yield return new WaitForSeconds(0.02f); }
        myUI.transform.GetChild(0).gameObject.GetComponent<Text>().text = OC_Name;
        myUI.transform.GetChild(1).gameObject.GetComponent<Text>().text = OC_Description;
    }

    public void fShowUI()
    {
        OC_UI.gameObject.SetActive(true);
        StartCoroutine(cShowUI());
    }
    IEnumerator cShowUI()
    {
        while (true)
        {
            // ���⿡ �� ���� ������ �׷� �� �����ؾ� ��
            transform.LookAt(player.transform);

            RaycastHit hit;
            float distance = Vector3.Distance(transform.position, player.transform.position);
            OC_UI.transform.position = Camera.main.WorldToScreenPoint(this.gameObject.transform.position);
            if ((distance >= 50f) || (Physics.Raycast(player.transform.position, (this.transform.position - player.transform.position).normalized, out hit, distance))) { OC_UI.gameObject.SetActive(false); }
            else
            {
                OC_UI.gameObject.SetActive(true);
                float scale = Mathf.Clamp(1 - (distance / maxDistance), minScale, maxScale);
                OC_UI.gameObject.transform.localScale = new Vector3(scale, scale, scale);
            }



            if (showUI == false) { break; }
            yield return new WaitForSeconds(0.02f);
        }
        OC_UI.gameObject.SetActive(false);
    }
}
