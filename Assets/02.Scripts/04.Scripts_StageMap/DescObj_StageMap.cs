using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescObj_StageMap : MonoBehaviour
{
    public string Name;
    public string Desc;
    public string SceneName;

    public string GetName() { return Name; }
    public string GetDesc() { return Desc; }
    public string GetSceneName() { return SceneName; }
}
