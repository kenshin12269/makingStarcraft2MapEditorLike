using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace MapUtil
{
    public class CliffDrawSystem : MonoBehaviour
    {
        [SerializeField] CustomToggleGroup menuToggleGroup;
        [SerializeField] CustomToggleGroup cliffOperationToggleGroup;
        [SerializeField] CustomToggleGroup brushOperationToggleGroup;
        [SerializeField] UIBelowInput belowInput;
        [SerializeField] TileRaycaster tileRaycaster;
        [SerializeField] TileRenderer tileRenderer;
        CommandSystem commandSystem;
        TileData tileData;
        string selectedCliffOperation;
        string selectedBrush;
        IDisposable inputObservable;
        void Awake()
        {
            menuToggleGroup.OnToggleFireObservable().Subscribe(name =>
            {
                this.enabled = name == "Cliff";
            }).AddTo(this);
            cliffOperationToggleGroup.OnToggleFireObservable().Subscribe(name =>
            {
                selectedCliffOperation = name;
            }).AddTo(this);
            brushOperationToggleGroup.OnToggleFireObservable().Subscribe(name =>
            {
                selectedBrush = name;
            }).AddTo(this);
        }
        void OnEnable()
        {
            Debug.Log("CliffDraw Enabled");
            inputObservable = belowInput.OnInputObservable().Subscribe(OnInputOccured);
        }
        void OnDisable()
        {
            Debug.Log("CliffDraw Disabled");
            inputObservable.Dispose();
            inputObservable = null;
        }
        public void Init(TileData tileData, CommandSystem commandSystem)
        {
            this.tileData = tileData;
            this.commandSystem = commandSystem;
        }
        Vector2Int beforeTileIdx;
        int startedHeight = -1;
        CliffUpCommand cliffUpCommand;
        CliffDownCommand cliffDownCommand;
        void OnInputOccured(UIBelowInputData inputData)
        {
            switch (inputData.State)
            {
                case UIBelowInputState.NotPressed:
                    {
                    }
                    break;
                case UIBelowInputState.PressStarted:
                    {
                        var tileCast = tileRaycaster.TileCast(inputData.Position);
                        if (tileCast.isHit == false)
                        {
                            // Debug.LogFormat("TileNotHit pos={0}", inputData.Position);
                        }
                        else
                        {
                            // Debug.LogFormat("TileHit pos={0} idx={1}", inputData.Position, tileCast.idx);
                            startedHeight = tileCast.height;
                            beforeTileIdx = new Vector2Int(int.MinValue, int.MinValue);
                            // Debug.Log("DrawStarted!");
                            if (selectedCliffOperation == "Up")
                            {
                                cliffUpCommand = new CliffUpCommand(tileData, tileRenderer);
                            }
                            else if (selectedCliffOperation == "Down")
                            {
                                cliffDownCommand = new CliffDownCommand(tileData, tileRenderer);
                            }
                        }
                    }
                    break;
                case UIBelowInputState.Pressing:
                    {
                        if (startedHeight != -1)
                        {
                            //                            Debug.Log("DrawGoing!");
                            var tileCast = tileRaycaster.TileCastHeight(inputData.Position, startedHeight);
                            if (tileCast.isHit == false)
                            {
                                // Debug.LogFormat("TileNotHit pos={0}", inputData.Position);
                            }
                            else
                            {
                                if (tileCast.height != startedHeight)
                                    break;
                                // Debug.LogFormat("TileHit pos={0} idx={1}", inputData.Position, tileCast.idx);
                                if (beforeTileIdx != tileCast.idx)
                                {
                                    beforeTileIdx = tileCast.idx;
                                    if (selectedCliffOperation == "Up")
                                    {
                                        // tileData.TileHeightMap[tileCast.idx.x, tileCast.idx.y]++;
                                        cliffUpCommand.Do(tileCast.idx);
                                    }
                                    else if (selectedCliffOperation == "Down")
                                    {
                                        if (tileData.TileHeightMap[tileCast.idx.x, tileCast.idx.y] > 0)
                                        {
                                            //tileData.TileHeightMap[tileCast.idx.x, tileCast.idx.y]--;
                                            cliffDownCommand.Do(tileCast.idx);
                                        }
                                    }
                                    // tileRenderer.UpdateIdx(tileCast.idx);
                                }
                            }
                        }
                    }
                    break;
                case UIBelowInputState.Released:
                    {
                        startedHeight = -1;
                        if (cliffUpCommand != null)
                        {
                            commandSystem.PushCommand(cliffUpCommand);
                            cliffUpCommand = null;
                        }
                        if (cliffDownCommand != null)
                        {
                            commandSystem.PushCommand(cliffDownCommand);
                            cliffDownCommand = null;
                        }
                    }
                    break;
            }
        }

    }
}