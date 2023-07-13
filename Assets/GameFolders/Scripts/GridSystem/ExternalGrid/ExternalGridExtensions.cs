using GameFolders.Scripts.Enums;
using GameFolders.Scripts.Managers.HighLevelManagers;
using UnityEngine;

namespace GameFolders.Scripts.GridSystem.ExternalGrid
{
    public static class ExternalGridExtensions
    {
        public static Quaternion GetRotation(this ExternalGridCell grid)
        {
            switch (grid.ForwardType, GameManager.Instance.GameIsVeritcal)
            {
                case (ForwardType.Right, true):
                    return Quaternion.Euler(0, 270, 90);
                case (ForwardType.Left, true):
                    return Quaternion.Euler(180, 270, 90);
                case (ForwardType.Up, true):
                    return Quaternion.Euler(90, 270, 90);
                case (ForwardType.Down, true):
                    return Quaternion.Euler(270, 270, 90);
                case (ForwardType.Forward, false):
                    return Quaternion.Euler(0, 0, 0);
                case (ForwardType.Backward, false):
                    return Quaternion.Euler(0, 180, 0);
                case (ForwardType.Right, false):
                    return Quaternion.Euler(0, 90, 0);
                case (ForwardType.Left, false):
                    return Quaternion.Euler(0, 270, 0);
                default:
                    return Quaternion.identity;
            }
        }
    }
}