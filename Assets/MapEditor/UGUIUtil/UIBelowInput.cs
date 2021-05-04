using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.EventSystems;

namespace MapUtil
{
    public struct UIBelowInputData
    {
        public UIBelowInputState State;
        public Vector3 Position;
    }
    public enum UIBelowInputState
    {
        NotPressed,
        PressStarted,
        Pressing,
        Released
    }
    public class UIBelowInput : MonoBehaviour
    {
        UIBelowInputState state = UIBelowInputState.NotPressed;
        Vector3 pos;
        Subject<UIBelowInputData> subject = new Subject<UIBelowInputData>();
        public IObservable<UIBelowInputData> OnInputObservable() => subject.AsObservable();
        // void Start()
        // {
        //     OnInputObservable().Subscribe(input =>
        //     {
        //         Debug.LogFormat("Input State={0} Pos={1}", input.State, input.Position);
        //     });
        // }
        void Update()
        {
            switch (state)
            {
                case UIBelowInputState.NotPressed:
                    {
#if !UNITY_ANDROID && !UNITY_IOS
                        if (Input.GetMouseButtonDown(0))
                        {
                            // Debug.LogFormat("called?");
                            if (IsPointerOverUIObject(Input.mousePosition) == false)
                            {
                                state = UIBelowInputState.Pressing;
                                subject.OnNext(new UIBelowInputData { State = UIBelowInputState.PressStarted, Position = Input.mousePosition });
                            }
                        }
                        else
                        {
                            subject.OnNext(new UIBelowInputData { State = UIBelowInputState.NotPressed, Position = Input.mousePosition });
                        }
#else
                        if (Input.touchCount > 0)
                        {
                            if (IsPointerOverUIObject(Input.mousePosition) == false)
                            {
                                state = UIBelowInputState.Pressing;
                                var touch = Input.GetTouch(0);
                                subject.OnNext(new UIBelowInputData { State = UIBelowInputState.PressStarted, Position = touch.position });
                            }
                        }
#endif
                    }
                    break;
                case UIBelowInputState.Pressing:
                    {
                        bool isReleased = false;
#if !UNITY_ANDROID && !UNITY_IOS
                        if (Input.GetMouseButtonUp(0))
                        {
                            state = UIBelowInputState.NotPressed;
                            subject.OnNext(new UIBelowInputData { State = UIBelowInputState.Released });
                            isReleased = true;
                        }
#else
                        if (Input.touchCount <= 0)
                        {
                            state = UIBelowInputState.NotPressed;
                            subject.OnNext(new UIBelowInputData { State = UIBelowInputState.Released });
                            isReleased = true;
                        }
#endif
                        if (isReleased == false)
                        {
                            if (Input.GetMouseButton(0))
                                subject.OnNext(new UIBelowInputData { State = UIBelowInputState.Pressing, Position = Input.mousePosition });
                            if (Input.touchCount > 0)
                                subject.OnNext(new UIBelowInputData { State = UIBelowInputState.Pressing, Position = Input.GetTouch(0).position });
                        }
                    }
                    break;
            }
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
