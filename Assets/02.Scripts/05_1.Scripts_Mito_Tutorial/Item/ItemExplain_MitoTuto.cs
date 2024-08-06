using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 튜토리얼의 설명패널이 있는 아이템마다 있는 함수
public class ItemExplain_MitoTuto : MonoBehaviour
{
    public GameObject explainItemPanel; // 아이템별 설명패널

    Grabbable grab;

    public bool isGrab = false; // 잡혔는지 여부
    public bool isDesc = false; // 설명창이 나왔는지 여부

    private void Start()
    {
        grab = GetComponent<Grabbable>();
    }

    private void Update()
    {
        if (grab != null && grab.SelectedHandPose)
            isGrab = true;

        // 원래는 아이템마다 갖고있는 변수를 퀘스트매니저(QM)로 쏘는 방식으로 할까 했는데
        // 일단 QM이 변수를 미리 갖고있고 아이템의 변수값에 따라 QM의 변수값을 바꾸는 방식으로 구현함
        switch (gameObject.tag)
        {
            case "Adenine":
                if (isGrab && isDesc)
                {
                    QuestManager_MitoTuto.Instance.isAdenine = true;
                }
                break;
            case "Ribose":
                if (isGrab && isDesc)
                {
                    QuestManager_MitoTuto.Instance.isRibose = true;
                }
                break;
            case "Phosphate":
                if (isGrab && isDesc)
                {
                    QuestManager_MitoTuto.Instance.isPhosphate = true;
                }
                break;
            case "ATP":
                if (isGrab && isDesc)
                {
                    QuestManager_MitoTuto.Instance.isATP = true;
                }
                break;
        }
    }
}
