using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(LoginManager_MT))]
public class LoginManagerEditor_MT : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("This script is responsible for connecting to Photon servers.", MessageType.Info);

        LoginManager_MT loginManager = (LoginManager_MT)target;
        if (GUILayout.Button("Connect Anonymously"))
        {
            loginManager.ConnectToPhotonServer();
        }
    }
}
#endif
