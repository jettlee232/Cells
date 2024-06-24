using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager_MitoTuto : MonoBehaviour
{
    public TextMeshProUGUI questText;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void QuestTextReset()
    {
        questText.text = string.Empty;
    }
}
