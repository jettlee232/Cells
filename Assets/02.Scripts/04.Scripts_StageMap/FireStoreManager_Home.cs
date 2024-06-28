using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStoreManager_Home : MonoBehaviour
{
    // Singleton & Don't Destroy On Load (DDOL)
    private static FireStoreManager_Home instance = null;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject); // �̰� ���߿�!!!
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static FireStoreManager_Home Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    // Singleton & Don't Destroy On Load (DDOL)

    [Header("CSV Text Variable")]
    public Dictionary<string, string> csvData;
    public string lang = "kr";

    void Start()
    {
        InitCSVFile();
    }

    public void InitCSVFile()
    {
        var rawData = CSVReader.Read("uiTextTestCSV"); // ���� �̸��� �ڱⰡ ���� �����̸��̶� �Ȱ����ɷ� ��
        csvData = new Dictionary<string, string>();

        foreach (var entry in rawData)
        {
            if (entry.ContainsKey("Contents") && entry.ContainsKey(lang))
            {
                string key = entry["Contents"].ToString();
                string value = entry[lang].ToString();
                csvData[key] = value;
            }
            else
            {
                Debug.LogWarning("CSV ���� ���� Contents�� ��� Į�� ����");
            }
        }

        Debug.Log("CSV ������ �ٿ� �Ϸ�. �� ���� : " + csvData.Count);
        PrintAllCSVData();
    }


    public void ChangeLang(string changingLang)
    {
        lang = changingLang;
    }

    public string ChooseLang()
    {
        return lang;
    }

    public string ReadCSV(string key) // �̰� ���� �߿���. �̰� �� ��Ծ�� �ϴϱ� �� �˾Ƶμ� ����
    {
        return csvData[key];
    }

    public void PrintAllCSVData()
    {
        if (csvData == null || csvData.Count == 0) // csvData�� ��� �ְų� �ʱ�ȭ���� ���� ���
        {
            Debug.LogError("CSV ���� �ε� �����߰ų� �������"); // ���� �޽��� ���
            return;
        }

        foreach (var entry in csvData) // csvData�� ��� Ű�� ���� ���
        {
            Debug.Log("Key: " + entry.Key + ", Value: " + entry.Value);
        }
    }
}
