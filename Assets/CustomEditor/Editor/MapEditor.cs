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
    GUISkin mySkin;
    void OnEnable()
    {
        minSize = k_EditorWindowMinimumSize;
        titleContent = new GUIContent("MapEditor");
        SceneView.duringSceneGui += OnSceneGUI;
        Undo.undoRedoPerformed += UndoRedoPerformed;
        // wantsMouseMove = true;
        //EditorGUIUtility.TrIconContent(IconUtility.GetIcon)
        Selection.selectionChanged += OnSelectionChanged;
        mySkin = AssetDatabase.LoadAssetAtPath<GUISkin>("Assets/CustomEditor/MySkin.guiskin");
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

    void OnGUI()
    {
        GUI.skin.button.Draw(new Rect(10, 10, 100, 100), "This Default Style button emits EventType.Repaint when cursor enters", false, false, false, false);
        mySkin.button.Draw(new Rect(100, 10, 100, 100), "This Custom Style button does not", false, false, false, false);
    }
    void DoCustomButton(Rect position, string name)
    {
        int controlID = GUIUtility.GetControlID("TmpBtn".GetHashCode(), FocusType.Passive, position);
        switch (Event.current.GetTypeForControl(controlID))
        {
            case EventType.Repaint:
                {
                    GUI.skin.button.Draw(position, GUIContent.none, controlID, false, position.Contains(Event.current.mousePosition));
                    // Debug.LogFormat("ButtonType={0}", GUI.skin.button.GetType().ToString());
                }
                break;
            default:
                // Debug.LogFormat("Event={0}", Event.current.type);
                break;
        }
    }
    void DoMyWindow(int windowID)
    {
        // GUI.Button(new Rect(0, 0, 100, 100), "Btn");
        // for (int i = 0; i < 50; i++)
        GUILayout.Button("One");
        // GUILayout.Button("Two");
    }
    bool isDragging = false;
    Vector2 startPos;
    Vector2 endPos;
    public static string GetColor(int value)
    {
        System.Random rand = new System.Random(value * 1324);
        var aCol = rand.Next(0, int.MaxValue);
        Color32 c = default;
        c.b = (byte)((aCol) & 0xFF);
        c.g = (byte)((aCol >> 8) & 0xFF);
        c.r = (byte)((aCol >> 16) & 0xFF);
        // c.r = 0;
        // c.g = 100;
        // c.b = 150;
        c.a = 255;
        // c.a = (byte)((aCol >> 24) & 0xFF);
        return string.Format("{0}{1}{2}FF", c.r.ToString("X"), c.g.ToString("X"), c.b.ToString("X"));
    }
    void MovableSphere(Vector3 position, float radius)
    {
        var controlID = GUIUtility.GetControlID("CustomControl".GetHashCode(), FocusType.Passive);
        var handleEventType = Event.current.GetTypeForControl(controlID);
        switch (handleEventType)
        {
            case EventType.Layout:
            case EventType.MouseMove:
                {
                    var screenPos = Handles.matrix.MultiplyPoint(position);
                    //This
                    HandleUtility.AddControl(controlID, HandleUtility.DistanceToCircle(position, radius));
                }
                break;
            case EventType.MouseDown:
                if (HandleUtility.nearestControl == controlID && GUIUtility.hotControl == 0)
                {
                    GUIUtility.hotControl = controlID;
                }
                break;
            case EventType.MouseDrag:
                if (GUIUtility.hotControl == controlID)
                {
                    Event.current.Use();
                }
                break;
            case EventType.MouseUp:
                {
                    if (GUIUtility.hotControl == controlID)
                    {
                        GUIUtility.hotControl = 0;
                    }
                }
                break;
            case EventType.Repaint:
                {
                    if (HandleUtility.nearestControl == controlID && GUIUtility.hotControl == 0)
                    {
                        var beforeColor = Handles.color;
                        Handles.color = Color.red;
                        Handles.SphereHandleCap(controlID, position, Quaternion.identity, radius, handleEventType);
                        Handles.color = beforeColor;
                    }
                    else
                    {
                        Handles.SphereHandleCap(controlID, position, Quaternion.identity, radius, handleEventType);
                    }
                }
                break;
        }
    }
    void LogFormat(EventType eventType, string str, params object[] args)
    {
        var convertedColor = GetColor((int)eventType);
        Debug.LogFormat("<color=#{0}>{1}</color>\t:{2}", convertedColor, eventType, string.Format(str, args));
    }
    [SerializeField] Vector3 handlePos;
    void OnSceneGUI(SceneView sceneView)
    {
        //Debug.LogFormat("CAlled");
        // var controlID = GUIUtility.GetControlID("CustomControl".GetHashCode(), FocusType.Passive);
        // var handleEventType = Event.current.GetTypeForControl(controlID);
        // LogFormat(handleEventType, "{0}", Event.current.mousePosition);

        // MovableSphere(new Vector3(0, 0, 0), 0.5f);
        // MovableSphere(new Vector3(3, 0, 0), 0.5f);

        // EditorGUI.BeginChangeCheck();
        // var newHandlePos = Handles.Slider(handlePos, Vector3.right);
        // if (EditorGUI.EndChangeCheck() == true)
        // {
        //     Undo.RecordObject(this, "TEst");
        //     handlePos = newHandlePos;
        // }
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
        Debug.LogFormat("Event={0} TypeForControl={1}", Event.current.type, typeOfControl);


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