using System.Collections.Generic;
using System.Linq;
using GameFolders.Scripts.Enums;
using GameFolders.Scripts.Helpers;
using UnityEngine;

namespace GameFolders.Scripts.GridSystem.ExternalGrid
{
    public class ExternalGridController : Singleton<ExternalGridController>
    {
        [SerializeField] private List<ExternalGridCell> externalGridCells = new List<ExternalGridCell>();

        public void AddExternalGridCell(ExternalGridCell externalGridCell)
        {
            externalGridCell.Id = externalGridCells.Count + 1;
            if (!externalGridCells.Contains(externalGridCell))
                externalGridCells.Add(externalGridCell);
        }

        public void RemoveExternalGridCell(ExternalGridCell externalGridCell)
        {
            if (!externalGridCells.Contains(externalGridCell)) return;
            externalGridCells.Remove(externalGridCell);
        }

        public void RemoveExternalGridCell(int index)
        {
            if (index < 0 || index >= externalGridCells.Count) return;
            externalGridCells.RemoveAt(index);
        }

        public int GetLastCellID()
        {
            if(externalGridCells.Count == 0) return 0;
            return externalGridCells.Count;
        }

        public void SetCellList(List<ExternalGridCell> newCells)
        {
            externalGridCells = newCells;
        }
        
        public ExternalGridCell GetOccupiedCell(Vector3 position)
        {
            return externalGridCells.FirstOrDefault(cell => cell.IsOccupied && cell.Position == position);
        }
        
        public ExternalGridCell GetOccupiedCell(int id)
        {
            return externalGridCells.FirstOrDefault(cell => cell.IsOccupied && cell.Id == id);
        }
        
        public ExternalGridCell GetOccupiedCell(Object item)
        {
            return externalGridCells.FirstOrDefault(cell => cell.IsOccupied && cell.OccupiedItem == item);
        }

        public List<ExternalGridCell> GetCells()
        {
            return externalGridCells;
        }

        public void RemoveAllExternalGridCells()
        {
            externalGridCells.Clear();
        }

        public ExternalGridCell GetNearestCell(Vector3 position)
        {
            return externalGridCells.OrderBy(x => Vector3.Distance(x.Position, position)).FirstOrDefault();
        }

        public void SetCellForwardForIndexes(int minCount, int maxCount, ForwardType forwardType)
        {
            for (int i = minCount; i < maxCount; i++)
            {
                externalGridCells[i].ForwardType = forwardType;
            }
        }

        public void SetCellPositionZAxisForIndexes(int minCount, int maxCount, Vector3 position)
        {
            for (int i = minCount; i < maxCount; i++)
            {
                var cell = externalGridCells[i];
                cell.Position = new Vector3(cell.Position.x, cell.Position.y, position.z);
            }
        }

        public float GetExternalCellPosition()
        {
            return externalGridCells.OrderByDescending(x => x.Position.z).Last().Position.z;
        }

        public void SetCellLockedForIndexes(int minCount, int maxCount, bool locked)
        {
            for (int i = minCount; i < maxCount; i++)
            {
                var cell = externalGridCells[i];
                cell.IsLockable = locked;
            }
        }
    }
}