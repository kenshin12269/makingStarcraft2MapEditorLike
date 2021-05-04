using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBoundaryChecker : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform[] indicatorArr;
    public Camera camera;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.LogFormat("Width={0} Height={1}", Screen.width, Screen.height);
        //camera.ScreenPointToRay()
        var bottomLeft = GetWorldPos(new Vector3(0, 0, 0));
        var bottomRight = GetWorldPos(new Vector3(Screen.width, 0, 0));
        var topLeft = GetWorldPos(new Vector3(0, Screen.height, 0));
        var topRight = GetWorldPos(new Vector3(Screen.width, Screen.height, 0));
        indicatorArr[0].position = bottomLeft;
        indicatorArr[1].position = bottomRight;
        indicatorArr[2].position = topLeft;
        indicatorArr[3].position = topRight;
    }
    Vector3 GetWorldPos(Vector3 screenPoint)
    {
        var ray = camera.ScreenPointToRay(screenPoint);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        var isSuccess = plane.Raycast(ray, out float dist);
        if (isSuccess == false)
            throw new System.Exception("ERR");
        return ray.GetPoint(dist);
    }
}
