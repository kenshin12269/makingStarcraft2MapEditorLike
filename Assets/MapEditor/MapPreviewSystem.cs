using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

namespace MapUtil
{
    public class MapPreviewSystem : MonoBehaviour, IListenToPostRenderCallback
    {
        [SerializeField] PostRenderCallbackReceiver postRenderCallbackReceiver;
        [SerializeField] RectTransform previewImageTrans;
        [SerializeField] Camera sourceFrustumCamera;
        [SerializeField] Camera previewCamera;
        [SerializeField] Transform targetControlCamera;
        public void SetPreviewSize(float worldRelatedSize)
        {
            previewCamera.orthographicSize = worldRelatedSize / 2.0f;
        }
        void Awake()
        {
            postRenderCallbackReceiver.Listen(this);
            previewImageTrans.gameObject.AddComponent<ObservablePointerDownTrigger>().OnPointerDownAsObservable().Subscribe(pointInfo =>
            {
                OnPreviewTouched(pointInfo.position);
            }).AddTo(this);
            previewImageTrans.gameObject.AddComponent<ObservableDragTrigger>().OnDragAsObservable().Subscribe(pointInfo =>
            {
                OnPreviewTouched(pointInfo.position);
            }).AddTo(this);
        }
        void OnPreviewTouched(Vector2 pos)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(previewImageTrans, pos, null, out var result);
            var halfSize = previewImageTrans.rect.size / 2.0f;
            var converted = result + halfSize;
            var normalized = converted / previewImageTrans.rect.size;
            normalized.x = Mathf.Clamp(normalized.x, 0.0f, 1.0f);
            normalized.y = Mathf.Clamp(normalized.y, 0.0f, 1.0f);

            var ray = previewCamera.ViewportPointToRay(normalized);
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            var isSuccess = plane.Raycast(ray, out float dist);
            if (isSuccess == false)
                return;
            var worldPos = ray.GetPoint(dist);
            targetControlCamera.position = worldPos;
            //            Debug.LogFormat("Norm={0} WorldPos={1}", normalized, worldPos);
        }
        public void OnPostRender()
        {
            var bottomLeft = GetWorldPos(new Vector3(0, 0, 0));
            var bottomRight = GetWorldPos(new Vector3(Screen.width, 0, 0));
            var topLeft = GetWorldPos(new Vector3(0, Screen.height, 0));
            var topRight = GetWorldPos(new Vector3(Screen.width, Screen.height, 0));
            bottomLeft += new Vector3(0, 10, 0);
            bottomRight += new Vector3(0, 10, 0);
            topLeft += new Vector3(0, 10, 0);
            topRight += new Vector3(0, 10, 0);


            GLDrawUtil.Begin();
            GLDrawUtil.DrawLine(bottomLeft, topLeft, Color.yellow);
            GLDrawUtil.DrawLine(topLeft, topRight, Color.yellow);
            GLDrawUtil.DrawLine(topRight, bottomRight, Color.yellow);
            GLDrawUtil.DrawLine(bottomRight, bottomLeft, Color.yellow);
            GLDrawUtil.End();
        }
        Vector3 GetWorldPos(Vector3 screenPoint)
        {
            var ray = sourceFrustumCamera.ScreenPointToRay(screenPoint);
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            var isSuccess = plane.Raycast(ray, out float dist);
            if (isSuccess == false)
            {
                return Vector3.zero;
            }
            return ray.GetPoint(dist);
        }
    }
}
