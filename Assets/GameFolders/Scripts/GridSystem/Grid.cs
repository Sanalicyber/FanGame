using System;
using UnityEngine;

namespace GameFolders.Scripts.GridSystem
{
    [Serializable]
    public class Grid
    {
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private float _cellSize;
        [SerializeField] private Transform _localWorld;
        [SerializeField] private bool _centered;
        
        public readonly GridCell[,] GridArray;
        public bool IsDirty;

        public int GetWidth() => _width;
        public int GetHeight() => _height;
        public float GetCellSize() => _cellSize;

        public Grid(int width, int height, float cellSize, Transform localWorld = null, bool centered = false)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;
            GridArray = new GridCell[width, height];
            _localWorld = localWorld;
            _centered = centered;

            Recalculate();
        }

        public void Recalculate()
        {
            if (_localWorld)
            {
                if (_centered)
                {
                    for (int x = 0; x < _width; x++)
                    {
                        for (int y = 0; y < _height; y++)
                        {
                            GridArray[x, y] = new GridCell(_localWorld.position.x + (x - _width / 2), _localWorld.position.y + (y - _height / 2), _cellSize);
                        }
                    }
                }
                else
                {
                    for (int x = 0; x < _width; x++)
                    {
                        for (int y = 0; y < _height; y++)
                        {
                            GridArray[x, y] = new GridCell(_localWorld.position.x + x, _localWorld.position.y + y, _cellSize);
                        }
                    }
                }
            }
            else
            {
                if (_centered)
                {
                    for (int x = 0; x < _width; x++)
                    {
                        for (int y = 0; y < _height; y++)
                        {
                            GridArray[x, y] = new GridCell((x - _width / 2), (y - _height / 2), _cellSize);
                        }
                    }
                }
                else
                {
                    for (int x = 0; x < _width; x++)
                    {
                        for (int y = 0; y < _height; y++)
                        {
                            GridArray[x, y] = new GridCell(x, y, _cellSize);
                        }
                    }
                }
            }
        }

        public GridCell[,] GetArray()
        {
            return GridArray;
        }

        public Vector3 GetWorldPosition(int x, int y)
        {
            if (_localWorld)
                return (new Vector3(x, y) + _localWorld.position) * _cellSize;
            return new Vector3(x, y) * _cellSize;
        }

        public Vector3 GetGridPosition(int x, int y)
        {
            return GridArray[x, y].Position;
        }

        public void SetCenter(bool isCentered)
        {
            _centered = isCentered;
            Recalculate();
        }

        public void SetLocalWorld(Transform localWorld)
        {
            _localWorld = localWorld;
            Recalculate();
        }

        public void Reset()
        {
            _width = 0;
            _height = 0;
            _cellSize = 0;
            _localWorld = null;
            _centered = false;
            Array.Clear(GridArray, 0, GridArray.Length);
        }

        public void SetData(int width, int height, float cellSize, Transform localWorld = null, bool centered = false)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;
            _localWorld = localWorld;
            _centered = centered;
            Recalculate();
        }

        public void SetValue(int x, int y, GridCell value, bool needCalculate = false)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height)
            {
                GridArray[x, y] = value;
                if (needCalculate) Recalculate();
            }
            else
                throw new ArgumentException($"X and Y values can not be greater or less than array limits.");
        }

        public void SetDirty()
        {
            IsDirty = true;
        }

        public static Vector3 GetNearestCell(Vector3 position)
        {
            var z = 0f;

            var x = Mathf.RoundToInt(position.x);
            var y = Mathf.RoundToInt(position.y);

            return new Vector3(x, y, z);
        }
    }
}