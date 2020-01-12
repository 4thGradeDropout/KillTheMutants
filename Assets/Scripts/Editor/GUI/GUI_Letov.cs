using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Dying))]
public class GUI_Letov : Editor
{
    // Start is called before the first frame update
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Dying script = (Dying)target;
        if (GUILayout.Button("Убить"))
        {
            script.Die();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
