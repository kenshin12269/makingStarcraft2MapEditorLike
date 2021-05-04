using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MapUtil
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Camera orthoCamera;
        [SerializeField] Transform rotationRoot;
        public float movingScale = 0.1f;
        public Vector2 MovableBoundarySize = new Vector2(3, 3);
        void Awake()
        {

        }
        void LateUpdate()
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                float scrollDelta = Input.mouseScrollDelta.y;
                scrollDelta *= -1;
                if (scrollDelta < 0)
                {
                    if (orthoCamera.orthographicSize > 3)
                        orthoCamera.orthographicSize += scrollDelta;
                }
                else
                {
                    orthoCamera.orthographicSize += scrollDelta;
                }
            }
            if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
            {
                if (IsPointerOverUIObject(Input.mousePosition) == false)
                    StartCoroutine(ProcessRightMouse());
            }
        }
        IEnumerator ProcessRightMouse()
        {
            var beforeMousePos = Input.mousePosition;
            bool isBtnUp = false;
            do
            {
                yield return null;
                var deltaMousePos = beforeMousePos - Input.mousePosition;
                beforeMousePos = Input.mousePosition;

                //MovableBoundarySize
                //Debug.LogFormat("Input={0}", Input.GetKey(KeyCode.LeftControl));
                if (Input.GetKey(KeyCode.LeftControl) == false)
                {
                    //이거작업해야함..
                    var coordSwapDelta = new Vector3(deltaMousePos.x, 0, deltaMousePos.y);
                    var forwardVec = transform.forward * deltaMousePos.y;
                    var rightVec = transform.right * deltaMousePos.x;
                    transform.localPosition += forwardVec * movingScale;
                    transform.localPosition += rightVec * movingScale;
                    // forwardVec = Vector3.ProjectOnPlane(forwardVec, Vector3.up);


                    // transform.localPosition += coordSwapDelta * movingScale;

                    var movableBoundarySizeHalf = MovableBoundarySize / 2.0f;
                    var curPos = transform.localPosition;
                    curPos.x = Mathf.Clamp(curPos.x, movableBoundarySizeHalf.x * -1, movableBoundarySizeHalf.x);
                    curPos.z = Mathf.Clamp(curPos.z, movableBoundarySizeHalf.y * -1, movableBoundarySizeHalf.y);
                    transform.localPosition = curPos;
                }
                else
                {
                    // var deltaQuaternion = Quaternion.Euler(deltaMousePos.y / 2.0f, 0, -1 * deltaMousePos.x / 2.0f);
                    // rotationRoot.localRotation *= deltaQuaternion;

                    rotationRoot.localRotation *= Quaternion.Euler(deltaMousePos.y / 2.0f, 0, 0);
                    //rotationRoot.localRotation *= Quaternion.AngleAxis(-1 * deltaMousePos.x / 2.0f, Vector3.forward);
                    transform.localRotation *= Quaternion.Euler(0, -1 * deltaMousePos.x / 2.0f, 0);

                    // var curEuler = rotationRoot.localRotation.eulerAngles;
                    // curEuler += coordSwapDelta / 2.0f;
                    // rotationRoot.localRotation = Quaternion.Euler(curEuler);
                    // Debug.LogFormat("Euler={0}", curEuler);
                }
                isBtnUp = false;
                if (Input.GetMouseButtonUp(1) == true)
                {
                    isBtnUp = true;
                }
                if (Input.GetMouseButtonUp(2) == true)
                {
                    isBtnUp = true;
                }
            } while (isBtnUp == false);
        }
        public bool IsPointerOverUIObject(Vector2 touchPos)
        {
            PointerEventData eventDataCurrentPosition
                = new PointerEventData(EventSystem.current);

            eventDataCurrentPosition.position = touchPos;

            List<RaycastResult> results = new List<RaycastResult>();


            EventSystem.current
            .RaycastAll(eventDataCurrentPosition, results);

            return results.Count > 0;
        }
    }
}