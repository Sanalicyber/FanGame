using System;

namespace GameFolders.Scripts.Controllers
{
    public static class MoveCounter
    {
        public static int MoveCount = 0;
        public static event Action<int> OnMoveCountChanged; 

        public static void SetMoveCount(int moveCount)
        {
            MoveCount = moveCount;
        }

        public static void UseMove()
        {
            MoveCount++;
            OnMoveCountChanged?.Invoke(MoveCount);
        }
        
        public static void ResetMoveCount()
        {
            MoveCount = 0;
            OnMoveCountChanged?.Invoke(MoveCount);
        }
    }
}