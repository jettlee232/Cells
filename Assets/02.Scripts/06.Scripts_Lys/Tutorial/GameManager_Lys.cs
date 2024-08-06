using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class GameManager_Lys : MonoBehaviour
{
    public static GameManager_Lys instance;
    UnityEngine.XR.InputDevice right;
    UnityEngine.XR.InputDevice left;

    [Header("Settings")]
    public GameObject player;
    public GameObject playerCam;
    public GameObject uiPointer;
    public GameObject NPC;
    public GameObject TutorialManager;
    public GameObject ToolTip;
    public GameObject[] DieEffect;   // CD�� 0, DP�� 1, ES�� 2
    public float rotateSpeed = 30f;
    public float GunSpeed = 30f;
    public float RocketSpeed = 15f;

    [Header("Timers")]
    public float minSelectTime = 0.5f;         // �ѹ� UI�� �� �� �ٽ� UI�� Ű���� �ν��� ������ ������ �ð� (���Ҹ��� �غ��� ��)

    private bool tuto1 = false;
    private bool tuto2 = false;
    private bool tuto3 = false;
    private bool movable = true;
    private bool selectable = true;
    private bool talkable = true;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
    }

    private void Start()
    {
        ToolTip.GetComponent<Tooltip>().TooltipOff();
    }

    #region �÷��̾�, NPC
    public GameObject GetPlayer() { return player; }
    public GameObject GetPlayerCam() { return playerCam; }
    public GameObject GetNPC() { return NPC; }
    public void Tuto1End() { tuto1 = true; }
    public bool GetTuto1() { return tuto1; }
    public bool GetTalkable() { return talkable; }
    public void EnableTalk() { talkable = true; }
    public void DisableTalk() { talkable = false; }
    public bool GetMovable() { return movable; }
    public void EnableMove() { movable = true; player.GetComponent<PlayerMoving_Lys>().EnableFly(); }
    public void DisableMove() { movable = false; player.GetComponent<PlayerMoving_Lys>().DisableFly(); }
    public void RemoveSelect() { uiPointer.GetComponent<LaserPointer_Lys>().DestroyDescription(); }
    public GameObject GetUIPointer() { return uiPointer; }
    public void MovePlayer() { TutorialManager.GetComponent<TutorialManager_Lys>().movePlayer(); }
    public GameObject GetTutorialManager() { return TutorialManager; }
    public void HideLine() { uiPointer.GetComponent<LaserPointer_Lys>().HideLine(); }
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

    #region ��, ���Ϸ�ó źȯ
    public float GetGunBulletSpeed() { return GunSpeed; }
    public float GetRocketBulletSpeed() { return RocketSpeed; }
    public GameObject GetHittedEffect(int num) { return DieEffect[num]; }
    #endregion

    #region ����
    public void ShowShootToolTip() { ToolTip.GetComponent<Tooltip>().TooltipOn("Ʈ���� ��ư�� ���� �߻�!"); }
    public void HideToolTip() { ToolTip.GetComponent<Tooltip>().TooltipOff(); }
    #endregion

    //public GameObject GetTutorialManager() { return TutorialManager; }

    public float GetRotateSpeed() { return rotateSpeed; }

    public void MoveScene(string sceneName)
    {
        StartCoroutine(cMoveScene(sceneName));
    }

    IEnumerator cMoveScene(string sceneName)
    {
        float timer = 0f;

        Color blackColor = new Color(0f, 0f, 0f, 1f);
        Color transparentColor = new Color(0f, 0f, 0f, 0f);

        GameObject BlackPanel = UIManager_Lys.instance.GetBlackPanel();
        float fadeOutTimer = UIManager_Lys.instance.GetFadeOutTimer();

        BlackPanel.gameObject.SetActive(true);

        BlackPanel.GetComponent<Image>().color = transparentColor;
        while (true)
        {
            if (BlackPanel.GetComponent<Image>().color.a >= 0.99999f) { BlackPanel.GetComponent<Image>().color = blackColor; break; }
            timer += Time.deltaTime;
            BlackPanel.GetComponent<Image>().color = Color.Lerp(transparentColor, blackColor, timer / fadeOutTimer);
            yield return null;
        }
        yield return new WaitForSeconds(0.75f);

        SceneManager.LoadScene(sceneName);
    }
}
