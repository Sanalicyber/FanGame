using System;
using GameFolders.Scripts.Enums;
using UnityEngine;

namespace GameFolders.Scripts.GridSystem.ExternalGrid
{
    [Serializable]
    public class ExternalGridCell
    {
        public static event Action<ExternalGridCell> SnapToGridEvent;

        [SerializeField] private GameObject item;
        [SerializeField] private int id;
        [SerializeField] private bool isOccupied;
        [SerializeField] private bool isLockable;
        [SerializeField] private Vector3 position;
        [SerializeField] private ForwardType forwardType;

        public GameObject OccupiedItem
        {
            get => item;
            set => item = value;
        }

        public int Id
        {
            get => id;
            set => id = value;
        }

        public Vector3 Position
        {
            get => position;
            set => position = value;
        }

        public bool IsOccupied
        {
            get => isOccupied;
            set => isOccupied = value;
        }

        public bool IsLockable
        {
            get => isLockable;
            set => isLockable = value;
        }

        public ForwardType ForwardType
        {
            get => forwardType;
            set => forwardType = value;
        }

        public ExternalGridCell(GameObject item, int id, Vector3 position)
        {
            this.item = item;
            this.id = id;
            this.position = position;
        }

        public ExternalGridCell()
        {
            item = null;
            id = 0;
            position = Vector3.zero;
        }

        public ExternalGridCell(Vector3 position)
        {
            item = null;
            id = 0;
            this.position = position;
        }

        public ExternalGridCell(int id, Vector3 position)
        {
            item = null;
            this.id = id;
            this.position = position;
        }

        public void TryToOccupyItem(GameObject item)
        {
            OnSnapToGridEvent(this);
            OccupiedItem = item;
        }

        /// <summary>
        /// Sets occupied item and isOccupied boolean.
        /// Note: If you want to set occupied section null or false, you can set item null.
        /// </summary>
        /// <param name="item">Occupied item</param>
        public void OccupyItem(GameObject item)
        {
            if (item == null)
            {
                this.item = null;
                isOccupied = false;
                return;
            }
            this.item = item;
            isOccupied = true;
        }

        public void SetID(int id)
        {
            this.id = id;
        }

        public void SetPosition(Vector3 position)
        {
            this.position = position;
        }

        public void SetPosition(float x, float y, float z)
        {
            position = new Vector3(x, y, z);
        }

        public void SetPosition(float x, float y)
        {
            position = new Vector3(x, y, 0);
        }

        public void SetPositionX(float x)
        {
            position = new Vector3(x, position.y, position.z);
        }

        public void SetPositionY(float y)
        {
            position = new Vector3(position.x, y, position.z);
        }

        public void SetPositionZ(float z)
        {
            position = new Vector3(position.x, position.y, z);
        }

        private static void OnSnapToGridEvent(ExternalGridCell obj)
        {
            SnapToGridEvent?.Invoke(obj);
        }
    }
}