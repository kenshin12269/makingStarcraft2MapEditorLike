using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class MapEditor : EditorWindow
{
    [MenuItem("Tools/MyEditor")]
    static void MenuInitEditor()
    {
        EditorWindow.GetWindow<MapEditor>().Show();
    }

    static readonly Vector2 k_EditorWindowMinimumSize = new Vector2(320, 180);

    void OnEnable()
    {
        minSize = k_EditorWindowMinimumSize;
        titleContent = new GUIContent("MapEditor");
        SceneView.duringSceneGui += OnSceneGUI;
        Undo.undoRedoPerformed += UndoRedoPerformed;
        //wantsMouseMove = true;
        //EditorGUIUtility.TrIconContent(IconUtility.GetIcon)
        Selection.selectionChanged += OnSelectionChanged;
    }
    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        Undo.undoRedoPerformed -= UndoRedoPerformed;
        Selection.selectionChanged -= OnSelectionChanged;
    }
    void OnDestroy()
    {
        Debug.LogFormat("Destroyed");
    }
    int selectedToolBar;
    void OnGUI()
    {
        Event e = Event.current;
        if (e.type == EventType.ContextClick)
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Hello"), true, () => Debug.LogFormat("Hello"));
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Togepi"), false, () => Debug.LogFormat("Hello222"));
            menu.ShowAsContext();
        }
        //Debug.LogFormat("EventType={0}", e.type.ToString());
        GUILayout.Space(8);
        //Debug.LogFormat("Position={0}", this.position);
        using (new GUILayout.HorizontalScope())
        {
            var listOfTools = new List<GUIContent>();
            listOfTools.Add(new GUIContent("111"));
            listOfTools.Add(new GUIContent("222"));
            listOfTools.Add(new GUIContent("333"));
            int toolBarIndex = selectedToolBar;
            EditorGUI.BeginChangeCheck();
            toolBarIndex = GUILayout.Toolbar(toolBarIndex, listOfTools.ToArray());
            if (EditorGUI.EndChangeCheck())
            {
                Debug.LogFormat("Changed?={0}", toolBarIndex);
                selectedToolBar = toolBarIndex;
            }
        }
    }
    void OnSceneGUI(SceneView sceneView)
    {
        //Debug.LogFormat("CAlled");
    }
    void UndoRedoPerformed()
    {
        Debug.Log("UndoPerformed");
    }
    void OnSelectionChanged()
    {
        // We want to delete deselected gameObjects from this.m_Hovering
        // var toDelete = new List<GameObject>();
        // var selectionGameObjects = Selection.gameObjects;

        // foreach (var hovering in m_Hovering.Keys)
        //     if (!selectionGameObjects.Contains(hovering))
        //         toDelete.Add(hovering);

        // foreach (var go in toDelete)
        //     m_Hovering.Remove(go);

        // m_IgnoreDrag.Clear();
    }
}
