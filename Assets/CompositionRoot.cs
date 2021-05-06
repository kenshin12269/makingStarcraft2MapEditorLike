using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class CompositionRoot : MonoBehaviour
{
    [SerializeField] Vector2Int tileMapSize = new Vector2Int(10, 10);
    [SerializeField] MapUtil.TileRenderer tileRenderer;
    [SerializeField] MapUtil.TileRaycaster tileRaycaster;
    [SerializeField] MapUtil.CliffDrawSystem cliffDrawSystem;
    [SerializeField] MapUtil.CameraController cameraController;
    [SerializeField] MapUtil.DebugTileGridSystem debugTileGridSystem;
    [SerializeField] MapUtil.MapPreviewSystem mapPreviewSystem;
    MapUtil.CommandSystem commandSystem;
    MapUtil.TileData tileData;
    void Awake()
    {
        const float TILE_SCALE = 1.0f;

        tileData = new MapUtil.TileData();
        tileData.MapSize = tileMapSize;

        commandSystem = new MapUtil.CommandSystem();

        var conversion = new MapUtil.TileAndWorldCoordConversion(tileData, TILE_SCALE);

        tileRenderer.Init(tileData, conversion);

        tileRaycaster.tileAndWorldConversion = conversion;
        tileRaycaster.tileData = tileData;

        cliffDrawSystem.Init(tileData, commandSystem);

        //Sync Camera Scale to TileDataMap Size..
        this.UpdateAsObservable().Subscribe(_ =>
        {
            cameraController.MovableBoundarySize = new Vector2(tileData.MapSize.x, tileData.MapSize.y);
        });

        debugTileGridSystem.TileData = tileData;
        debugTileGridSystem.conversion = conversion;

        //Preview보여질 영역은 Tile영역보다 10%더 많이 보여야 함.
        float bigSize = tileData.MapSize.x * TILE_SCALE;
        if (tileData.MapSize.y * TILE_SCALE > bigSize)
            bigSize = tileData.MapSize.y * TILE_SCALE;
        Debug.LogFormat("bigsize={0}", bigSize);
        mapPreviewSystem.SetPreviewSize(bigSize * 1.1f);
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyUp(KeyCode.Z))
        {
            commandSystem.Undo();
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyUp(KeyCode.Y))
        {
            commandSystem.Redo();
        }
    }
}