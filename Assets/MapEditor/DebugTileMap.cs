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
        Subject<Vector2Int> onBlockClickedSubject = new Subject<Vector2Int>();
        IObservable<Vector2Int> OnBlockClickedObservable() => onBlockClickedSubject.AsObservable();
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
                        map[posValue.x, posValue.y].transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.white;
                    }).AddTo(map[x, y]);
                    map[x, y].AddComponent<ObservablePointerExitTrigger>().OnPointerExitAsObservable().Subscribe(pointer =>
                    {
                        map[posValue.x, posValue.y].transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 0.3f);
                    }).AddTo(map[x, y]);
                    map[x, y].AddComponent<ObservablePointerClickTrigger>().OnPointerClickAsObservable().Subscribe(pointer =>
                    {

                    }).AddTo(map[x, y]);
                }
            }
            sample.SetActive(false);

            if (boundaryController != null)
                boundaryController.Size = Size;
        }
    }
}

