using System;
using System.Collections.Generic;
using GameFolders.Scripts.Helpers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameFolders.Scripts.AppInitializer
{
    public class GridInitializer : Singleton<GridInitializer>
    {
        public GameObject gridPrefab;
        public int gridWidth = 10;
        public int gridHeight = 10;

        public List<GameObject> GridList;

        [Button]
        private void Start()
        {
            Init(gridWidth, gridHeight);
        }

        public void Init(int width, int height)
        {
            gridHeight = height;
            gridWidth = width;
            
            CreateGrid();
        }

        private void CreateGrid()
        {
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    var grid = ObjectPool.Spawn(gridPrefab, transform, new Vector3(x - gridWidth / 2, y - gridHeight / 2), Quaternion.Euler(270,0,0));
                    GridList.Add(grid);
                }
            }
        }
    }
}