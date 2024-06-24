using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class TutorialManager_MitoTuto : MonoBehaviour
{
    PlayerMoving_Mito playerMoving_Mito;
    public GameObject playerWall;
    public GameObject mapWall;
    public GameObject miniHalfMito;

    void Start()
    {
        playerMoving_Mito = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoving_Mito>();

        if (playerMoving_Mito != null )
        {
            SetPlayerSpeed(3.0f, 15.0f, 15.0f);
        }
    }

    public void PlayDialogue(int n)
    {
        DialogueController_MitoTuto.Instance.ActivateDST(n);
    }

    public void SetPlayerSpeed(float move, float up, float down)
    {
        playerMoving_Mito.moveSpeed = move;
        playerMoving_Mito.upSpeed = up;
        playerMoving_Mito.downSpeed = down;
    }

    public void TogglePlayerWall()
    {
        playerWall.SetActive(!playerWall.activeSelf);
    }

    public void ToggleMapWall()
    {
        mapWall.SetActive(!mapWall.activeSelf);
    }

    public void ToggleFlyable()
    {
        playerMoving_Mito.flyable = !playerMoving_Mito.flyable;
    }

    public void ToggleMiniHalfMito()
    {
        miniHalfMito.SetActive(!miniHalfMito.activeSelf);
    }

    public void LookAtMito()
    {
        playerMoving_Mito.transform.eulerAngles = new Vector3(0, 90.0f, 0);
    }

    #region Register with Lua
    private void OnEnable()
    {
        Lua.RegisterFunction("PlayDialogue", this, SymbolExtensions.GetMethodInfo(() => PlayDialogue((int)0)));
        Lua.RegisterFunction("SetPlayerSpeed", this, SymbolExtensions.GetMethodInfo(() => SetPlayerSpeed((float)0, (float)0, (float)0)));
        Lua.RegisterFunction("TogglePlayerWall", this, SymbolExtensions.GetMethodInfo(() => TogglePlayerWall()));
        Lua.RegisterFunction("ToggleMapWall", this, SymbolExtensions.GetMethodInfo(() => ToggleMapWall()));
        Lua.RegisterFunction("ToggleFlyable", this, SymbolExtensions.GetMethodInfo(() => ToggleFlyable()));
        Lua.RegisterFunction("ToggleMiniHalfMito", this, SymbolExtensions.GetMethodInfo(() => ToggleMiniHalfMito()));
        Lua.RegisterFunction("LookAtMito", this, SymbolExtensions.GetMethodInfo(() => LookAtMito()));
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction("PlayDialogue");
        Lua.UnregisterFunction("SetPlayerSpeed");
        Lua.UnregisterFunction("TogglePlayerWall");
        Lua.UnregisterFunction("ToggleMapWall");
        Lua.UnregisterFunction("ToggleFlyable");
        Lua.UnregisterFunction("ToggleMiniHalfMito");
        Lua.UnregisterFunction("LookAtMito");
    }
    #endregion
}
