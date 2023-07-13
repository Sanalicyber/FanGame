using System;
using UnityEngine;

namespace GameFolders.Scripts.GridSystem
{
    [Serializable]
    public class GridCell
    {
        private float _x;
        private float _y;
        private bool _isFilled;
        private bool _isLocked;
        private float _size;

        public Vector2 Position => new Vector2(_x, _y);
        public bool IsFilled => _isFilled;
        public bool IsLocked => _isLocked;
        public float Size => _size;

        public GridCell(float x, float y, float size, bool isFilled, bool isLocked)
        {
            _x = x;
            _y = y;
            _isFilled = isFilled;
            _isLocked = isLocked;
            _size = size;
        }

        /// <summary>
        /// Default constructor of GridCell. This constructor gives a GridCell which isFilled bool false.
        /// </summary>
        /// <param name="x">X position of GridCell</param>
        /// <param name="y">Y position of GridCell</param>
        /// <param name="size">Size of GridCell</param>
        public GridCell(float x, float y, float size)
        {
            _x = x;
            _y = y;
            _isFilled = false;
            _isLocked = false;
            _size = size;
        }

        /// <summary>
        /// Returns distance of between two GridCells
        /// </summary>
        /// <param name="on"></param>
        /// <param name="to"></param>
        /// <returns>Distance between two GridCells.</returns>
        public static float Distance(GridCell on, GridCell to)
        {
            return Vector2.Distance(on.Position, to.Position);
        }
    }
}