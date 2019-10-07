using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EasyImport_Decorations))]
public class GUI_EasyImport_Decorations : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EasyImport_Decorations script = (EasyImport_Decorations)target;
        if (GUILayout.Button("DoMagic"))
        {
            //SHOOOOOOOOOOOOOOO?!!?!?!
        }
    }
}
