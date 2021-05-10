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
    Color newColor;
    void OnGUI()
    {
        Event e = Event.current;

        // GUI.Button(new Rect(10, 10, 100, 100), "Hello");
        // if (e.type == EventType.Repaint)
        //     GUI.skin.button.Draw(new Rect(10, 10, 100, 100), false, false, false, false);

        // int controlID = GUIUtility.GetControlID("Test.Test".GetHashCode(), FocusType.Passive, new Rect(10, 10, 100, 100));
        // var typeOfControl = Event.current.GetTypeForControl(controlID);

        // Debug.LogFormat("GotEvent={0} ControlEvent={1} Cnt={2}", e.type, typeOfControl, Event.GetEventCount());


        // return;

        // if (e.type == EventType.ContextClick)
        // {
        //     var menu = new GenericMenu();
        //     menu.AddItem(new GUIContent("Hello"), true, () => Debug.LogFormat("Hello"));
        //     menu.AddSeparator("");
        //     menu.AddItem(new GUIContent("Togepi"), false, () => Debug.LogFormat("Hello222"));
        //     menu.ShowAsContext();
        // }
        // //Debug.LogFormat("EventType={0}", e.type.ToString());
        // GUILayout.Space(8);
        // //        Debug.LogFormat("Tool={0}", Tools.current);
        // //Debug.LogFormat("Position={0}", this.position);
        // using (new GUILayout.HorizontalScope())
        // {
        //     var listOfTools = new List<GUIContent>();
        //     listOfTools.Add(new GUIContent("111"));
        //     listOfTools.Add(new GUIContent("222"));
        //     listOfTools.Add(new GUIContent("333"));
        //     int toolBarIndex = selectedToolBar;
        //     EditorGUI.BeginChangeCheck();
        //     toolBarIndex = GUILayout.Toolbar(toolBarIndex, listOfTools.ToArray());
        //     if (EditorGUI.EndChangeCheck())
        //     {
        //         // Debug.LogFormat("Changed?={0}", toolBarIndex);
        //         selectedToolBar = toolBarIndex;
        //     }
        // }

        // using (new GUILayout.VerticalScope("box"))
        // {
        //     EditorGUILayout.LabelField("One");
        //     EditorGUILayout.LabelField("Two");
        // }

        // Rect position = GUILayoutUtility.GetRect(MyGUI.TempContent("Hello"), GUI.skin.button);
        MyGUI.Button(new Rect(10, 10, 100, 100), MyGUI.TempContent("Hello"), GUI.skin.button);
        MyGUI.Button(new Rect(50, 50, 100, 100), MyGUI.TempContent("Hello"), GUI.skin.button);
        // newColor = EditorGUI.ColorField(new Rect(10, 10, 100, 50), GUIContent.none, newColor, true, true, false);
        // Repaint();
        // if (Event.current.type != EventType.Repaint && Event.current.type != EventType.Layout)
        // Debug.LogFormat("cur={0}", Event.current.type);
        // Debug.LogFormat("IsPressed={0}", isPressed);
        // GUILayout.Button("OkayBtn");
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

    }
}

public static class MyGUI
{
    private static readonly GUIContent s_TempContent = new GUIContent();
    public static GUIContent TempContent(string text)
    {
        s_TempContent.text = text;
        s_TempContent.image = null;
        s_TempContent.tooltip = null;
        return s_TempContent;
    }

    private static readonly int s_ButtonHint = "MyGUI.Button".GetHashCode();
    public static bool Button(Rect position, GUIContent label, GUIStyle style)
    {
        int controlID = GUIUtility.GetControlID(s_ButtonHint, FocusType.Passive, position);
        bool result = false;
        var typeOfControl = Event.current.GetTypeForControl(controlID);
        // var anotherTypeOfControl = Event.current.GetTypeForControl(1132);
        // Debug.LogFormat("Event={0} TypeForControl={1} Another={2}", Event.current.type, typeOfControl, anotherTypeOfControl);


        // if (Event.current.type != EventType.Repaint && Event.current.type != EventType.Layout)
        //     Debug.LogFormat("ControlID={0} tof={1} cur={2}", controlID, typeOfControl, Event.current.type);
        switch (typeOfControl)
        {
            case EventType.MouseDown:
                if (GUI.enabled && position.Contains(Event.current.mousePosition))
                {
                    Debug.LogFormat("ControlID={0} tof={1} cur={2} rect={3} hot={4} true", controlID, typeOfControl, Event.current.type, position, GUIUtility.hotControl);
                    GUIUtility.hotControl = controlID;
                    // Event.current.Use();
                }
                else
                {
                    Debug.LogFormat("ControlID={0} tof={1} cur={2} rect={3} hot={4} false", controlID, typeOfControl, Event.current.type, position, GUIUtility.hotControl);
                }
                break;
            // case EventType.MouseDrag:
            //     if (GUIUtility.hotControl == controlID)
            //     {
            //         Event.current.Use();
            //     }
            //     break;
            case EventType.MouseUp:
                if (GUIUtility.hotControl == controlID)
                {
                    Debug.LogFormat("ControlID={0} tof={1} cur={2} rect={3} hot={4} hotControl", controlID, typeOfControl, Event.current.type, position, GUIUtility.hotControl);
                    GUIUtility.hotControl = 0;
                    // Event.current.Use();
                    if (position.Contains(Event.current.mousePosition))
                    {
                        result = true;
                    }
                }
                else
                {
                    Debug.LogFormat("ControlID={0} tof={1} cur={2} rect={3} hot={4} Not Hot Control", controlID, typeOfControl, Event.current.type, position, GUIUtility.hotControl);
                }
                break;
            // case EventType.KeyDown:
            //     {
            //         if (GUIUtility.hotControl == controlID)
            //         {
            //             if (Event.current.keyCode == KeyCode.Escape)
            //             {
            //                 GUIUtility.hotControl = 0;
            //                 Event.current.Use();
            //             }
            //         }
            //     }
            //     break;
            case EventType.Repaint:
                {
                    style.Draw(position, label, controlID, false, position.Contains(Event.current.mousePosition));
                    result = controlID == GUIUtility.hotControl && position.Contains(Event.current.mousePosition);
                }
                break;
                // default:
                //     if (Event.current.type != EventType.Repaint && Event.current.type != EventType.Layout)
                //         Debug.LogFormat("ControlID={0} Default tof={1} cur={2} rect={3} hot={4} Not Hot Control", controlID, typeOfControl, Event.current.type, position, GUIUtility.hotControl);
                //     break;
        }
        return result;
    }

    public static bool Button(Rect position, string label, GUIStyle style)
    {
        return Button(position, TempContent(label), style);
    }
    public static bool Button(Rect position, string label)
    {
        return Button(position, label, GUI.skin.button);
    }
}