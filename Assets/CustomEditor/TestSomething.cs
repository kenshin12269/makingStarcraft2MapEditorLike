using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSomething : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public GUIStyle buttonStyle;
    public GUISkin mySkin;
    void OnGUI()
    {
        buttonStyle = GUI.skin.button;
    }
}
