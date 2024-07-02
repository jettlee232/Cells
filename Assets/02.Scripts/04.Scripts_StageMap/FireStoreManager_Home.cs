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
            //DontDestroyOnLoad(gameObject); // 이건 나중에!!!
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
        var rawData = CSVReader.Read("uiTextTestCSV"); // 파일 이름은 자기가 만든 파일이름이랑 똑같은걸로 ㄲ
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
                Debug.LogWarning("CSV 파일 내에 Contents나 언어 칼럼 없음");
            }
        }

        Debug.Log("CSV 데이터 다운 완료. 총 갯수 : " + csvData.Count);
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

    public string ReadCSV(string key) // 이게 제일 중요함. 이거 잘 써먹어야 하니깐 잘 알아두셈 ㅇㅇ
    {
        return csvData[key];
    }

    public void PrintAllCSVData()
    {
        if (csvData == null || csvData.Count == 0) // csvData가 비어 있거나 초기화되지 않은 경우
        {
            Debug.LogError("CSV 파일 로드 실패했거나 비어있음"); // 오류 메시지 출력
            return;
        }

        foreach (var entry in csvData) // csvData의 모든 키와 값을 출력
        {
            Debug.Log("Key: " + entry.Key + ", Value: " + entry.Value);
        }
    }
}
