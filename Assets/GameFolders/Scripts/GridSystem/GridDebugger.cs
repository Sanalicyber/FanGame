using System.Collections.Generic;
using GameFolders.Scripts.Helpers;
using UnityEngine;

namespace GameFolders.Scripts.GridSystem
{
    public class GridDebugger : Singleton<GridDebugger>
    {
        public int Width;
        public int Height;
        public float CellSize;
        public bool Centered;
        public Grid DebugGrid;

        private void OnDrawGizmos()
        {
            DebugGrid = new Grid(Width, Height, CellSize, transform, Centered);

            var array = DebugGrid.GetArray();
            Gizmos.color = Color.red;
            int width = DebugGrid.GetWidth();
            int height = DebugGrid.GetHeight();

            for (int x = 0; x < width - 1; x++)
            {
                for (int y = 0; y < height - 1; y++)
                {
                    Gizmos.DrawSphere(DebugGrid.GetGridPosition(x + 1, y), .1f);
                    Gizmos.DrawSphere(DebugGrid.GetGridPosition(x, y + 1), .1f);

                    Gizmos.DrawWireCube(DebugGrid.GetGridPosition(x + 1, y), new Vector3(CellSize, CellSize, 0));
                    Gizmos.DrawWireCube(DebugGrid.GetGridPosition(x, y + 1), new Vector3(CellSize, CellSize, 0));
                }
            }

            Gizmos.DrawSphere(DebugGrid.GetGridPosition(array.GetLength(0) - 1, array.GetLength(1) - 1), .1f);
            Gizmos.DrawSphere(DebugGrid.GetGridPosition(0, 0), .1f);
            
            Gizmos.DrawWireCube(DebugGrid.GetGridPosition(array.GetLength(0) - 1, array.GetLength(1) - 1), new Vector3(CellSize, CellSize, 0));
            Gizmos.DrawWireCube(DebugGrid.GetGridPosition(0, 0), new Vector3(CellSize, CellSize, 0));
        }
    }
}