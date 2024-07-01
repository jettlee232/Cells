using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class GameManager_StageMap : MonoBehaviour
{
    public static GameManager_StageMap instance;
    UnityEngine.XR.InputDevice right;
    UnityEngine.XR.InputDevice left;

    [Header("Settings")]
    public GameObject player;
    public GameObject playerCam;
    public GameObject uiPointer;
    public GameObject NPC;
    public GameObject TutorialManager;

    [Header("Timers")]
    public float minSelectTime = 0.5f;         // �ѹ� UI�� �� �� �ٽ� UI�� Ű���� �ν��� ������ ������ �ð� (���Ҹ��� �غ��� ��)

    private bool firstEnd = false;
    private bool secondEnd = false;
    private bool secondCon = false;
    private bool movable = true;
    private bool selectable = true;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
    }

    #region �÷��̾�, NPC
    public GameObject GetPlayer() { return player; }
    public GameObject GetPlayerCam() { return playerCam; }
    public GameObject GetNPC() { return NPC; }
    public void FirstEnd() { firstEnd = true; }
    public bool GetFirstEnd() { return firstEnd; }
    public void ClearTutorial() { UIManager_StageMap.instance.SetQuest("NPC���� ���ư���!"); secondCon = true; }
    public void SecondEnd() { secondEnd = true; }
    public bool GetSecondCon() { return secondCon; }
    public bool GetMovable() { return movable; }
    public void EnableMove() { movable = true; player.GetComponent<PlayerMoving_StageMap>().EnableFly(); }
    public void DisableMove() { movable = false; player.GetComponent<PlayerMoving_StageMap>().DisableFly(); }
    public void RemoveSelect() { uiPointer.GetComponent<LaserPointer_StageMap>().DestroyDescription(); }
    public GameObject GetUIPointer() { return uiPointer; }
    #endregion

    #region UI �� -> ���ο� UI ����

    public void WaitForNewUI() { StartCoroutine(WaitUITimer()); }
    public bool GetSelectable() { return selectable; }
    IEnumerator WaitUITimer()
    {
        selectable = false;
        yield return new WaitForSeconds(minSelectTime);
        selectable = true;
    }
    public void SetSelectable(bool select) { selectable = select; }

    #endregion

    public GameObject GetTutorialManager() { return TutorialManager; }

    public void MoveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
