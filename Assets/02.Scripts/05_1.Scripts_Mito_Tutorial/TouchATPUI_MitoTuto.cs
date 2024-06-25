using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TouchATPUI_MitoTuto : MonoBehaviour
{
    public GameObject explainPanel;
    public TextMeshProUGUI explainText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Matrix"))
        {
            explainPanel.SetActive(true);
            explainText.text = "����";
        }

        if (other.CompareTag("Cristae"))
        {
            explainPanel.SetActive(true);
            explainText.text = "�ָ�";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        explainPanel.SetActive(false);
        explainText.text = "";
    }
}
