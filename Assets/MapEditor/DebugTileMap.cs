using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

namespace MapUtil
{
    public class DebugTileMap : MonoBehaviour
    {
        [SerializeField] GameObject sample;
        [SerializeField] Transform parent;
        public Vector2Int Size;
        Vector2Int beforeSize = Vector2Int.zero;
        GameObject[,] map = new GameObject[0, 0];
        [SerializeField] BoundaryController boundaryController;
        Subject<Vector2Int> onDragSubject = new Subject<Vector2Int>();
        public IObservable<Vector2Int> OnDragObservable() => onDragSubject.AsObservable();
        Subject<Tuple<Vector2Int, bool>> onPointerEnterExitSubject = new Subject<Tuple<Vector2Int, bool>>();
        public IObservable<Tuple<Vector2Int, bool>> OnPointerEnterExitSubject() => onPointerEnterExitSubject.AsObservable();
        bool isDragging = false;
        static Vector2Int ValidateSize(Vector2Int value)
        {
            if (value.x < 0)
                value.x = 0;
            if (value.y < 0)
                value.y = 0;
            return value;
        }
        void Update()
        {
            var tmpSize = ValidateSize(Size);
            if (beforeSize == Size)
                return;
            beforeSize = Size;

            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    GameObject.Destroy(map[x, y]);
                }
            }
            map = new GameObject[Size.x, Size.y];
            var halfSize = new Vector3((float)Size.x / 2.0f, 0, (float)Size.y / 2.0f);
            sample.SetActive(true);
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    Vector2Int posValue = new Vector2Int(x, y);
                    map[x, y] = GameObject.Instantiate(sample);
                    map[x, y].name = $"{x}:{y}";
                    map[x, y].transform.SetParent(parent, false);
                    map[x, y].transform.localPosition = new Vector3(x, 0, y) - halfSize;
                    map[x, y].AddComponent<ObservablePointerEnterTrigger>().OnPointerEnterAsObservable().Subscribe(pointer =>
                    {
                        if (isDragging == true)
                        {
                            onDragSubject.OnNext(posValue);
                        }
                        map[posValue.x, posValue.y].transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.white;
                        onPointerEnterExitSubject.OnNext(new Tuple<Vector2Int, bool>(posValue, true));
                    }).AddTo(map[x, y]);
                    map[x, y].AddComponent<ObservablePointerExitTrigger>().OnPointerExitAsObservable().Subscribe(pointer =>
                    {
                        map[posValue.x, posValue.y].transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 0.3f);
                        onPointerEnterExitSubject.OnNext(new Tuple<Vector2Int, bool>(posValue, false));
                    }).AddTo(map[x, y]);


                    map[x, y].AddComponent<ObservablePointerDownTrigger>().OnPointerDownAsObservable().Subscribe(pointer =>
                    {
                        isDragging = true;
                        onDragSubject.OnNext(posValue);
                    }).AddTo(map[x, y]);
                    map[x, y].AddComponent<ObservablePointerUpTrigger>().OnPointerUpAsObservable().Subscribe(pointer =>
                    {
                        isDragging = false;
                    }).AddTo(map[x, y]);
                    // map[x, y].AddComponent<ObservableBeginDragTrigger>().OnBeginDragAsObservable().Subscribe(pointer =>
                    // {
                    //     isDragging = true;
                    // }).AddTo(map[x, y]);
                    // map[x, y].AddComponent<ObservableEndDragTrigger>().OnEndDragAsObservable().Subscribe(pointer =>
                    // {
                    //     isDragging = false;
                    // }).AddTo(map[x, y]);
                }
            }
            sample.SetActive(false);

            if (boundaryController != null)
                boundaryController.Size = Size;
        }
        public Vector3 GetPosFromIdx(Vector2Int idx)
        {
            float xLen = map.GetLength(0);
            float yLen = map.GetLength(1);
            // Vector3 realPos = new Vector3(idx.x,0,idx.y);
            float xHalfLen = xLen / 2.0f;
            float yHalfLen = yLen / 2.0f;
            float xOffset = -xHalfLen + 0.5f + idx.x;
            float yOffset = -yHalfLen + 0.5f + idx.y;
            return new Vector3(xOffset, 0, yOffset);
        }
    }
}

