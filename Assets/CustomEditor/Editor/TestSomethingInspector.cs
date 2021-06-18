using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestSomething))]
public class TestSomethingInspector : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        TestSomething tmp = target as TestSomething;
        tmp.buttonStyle = GUI.skin.button;
        // EditorGUILayout.ObjectField(GUI.skin.button.normal.background, typeof(Texture2D));
        tmp.mySkin = GUI.skin;
        if (GUILayout.Button("Copy") == true)
        {
            AssetDatabase.CreateAsset(GUI.skin, "Assets/myskin.guiskin");
            AssetDatabase.Refresh();
        }
        // Debug.LogFormat("Background={0}",GUI.skin.button.normal.background)
        base.OnInspectorGUI();

        // serializedObject.Update();

        // var buttonStyleProp = serializedObject.FindProperty("buttonStyle");
        // Debug.LogFormat("WTF={0}", buttonStyleProp.GetType().Name);
        // //        EditorGUILayout.PropertyField(serializedObject.FindProperty("buttonStyle"))

        // serializedObject.ApplyModifiedProperties();
    }
}
