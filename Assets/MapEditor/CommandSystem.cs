using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MapUtil
{
    public interface ICommand
    {
        void Undo();
        void Redo();
    }
    public class CliffUpCommand : ICommand
    {
        private readonly TileData tileData;
        private readonly TileRenderer tileRenderer;
        private List<Vector2Int> recordList = new List<Vector2Int>();
        public CliffUpCommand(TileData tileData, TileRenderer tileRenderer)
        {
            this.tileData = tileData;
            this.tileRenderer = tileRenderer;
        }
        public void Do(Vector2Int pos, bool record = true)
        {
            tileData.TileHeightMap[pos.x, pos.y]++;
            tileRenderer.UpdateHeightIdx(pos);
            if (record)
                recordList.Add(pos);
        }
        public void Undo()
        {
            for (int i = recordList.Count - 1; i >= 0; i--)
            {
                var pos = recordList[i];
                tileData.TileHeightMap[pos.x, pos.y]--;
                tileRenderer.UpdateHeightIdx(pos);
            }
        }

        public void Redo()
        {
            for (int i = recordList.Count - 1; i >= 0; i--)
            {
                Do(recordList[i], false);
            }
        }
    }
    public class CliffDownCommand : ICommand
    {
        private readonly TileData tileData;
        private readonly TileRenderer tileRenderer;
        private List<Vector2Int> recordList = new List<Vector2Int>();
        public CliffDownCommand(TileData tileData, TileRenderer tileRenderer)
        {
            this.tileData = tileData;
            this.tileRenderer = tileRenderer;
        }
        public void Do(Vector2Int pos, bool record = true)
        {
            tileData.TileHeightMap[pos.x, pos.y]--;
            tileRenderer.UpdateHeightIdx(pos);
            if (record)
                recordList.Add(pos);
        }
        public void Undo()
        {
            for (int i = recordList.Count - 1; i >= 0; i--)
            {
                var pos = recordList[i];
                tileData.TileHeightMap[pos.x, pos.y]++;
                tileRenderer.UpdateHeightIdx(pos);
            }
        }

        public void Redo()
        {
            for (int i = recordList.Count - 1; i >= 0; i--)
            {
                Do(recordList[i], false);
            }
        }
    }
    public class CommandSystem
    {
        Stack<ICommand> undoStack = new Stack<ICommand>();
        Stack<ICommand> redoStack = new Stack<ICommand>();
        public void PushCommand(ICommand command)
        {
            undoStack.Push(command);
            redoStack.Clear();
        }
        public void Undo()
        {
            if (undoStack.Count <= 0)
            {
                Debug.LogFormat("Nothing to undo");
                return;
            }

            var item = undoStack.Pop();
            item.Undo();
            redoStack.Push(item);
        }
        public void Redo()
        {
            if (redoStack.Count <= 0)
            {
                Debug.LogFormat("Nothing to redo");
                return;
            }
            var item = redoStack.Pop();
            item.Redo();
            undoStack.Push(item);
        }
    }
}