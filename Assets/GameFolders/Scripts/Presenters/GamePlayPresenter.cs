using System;
using Framework.Core;
using Framework.UI;
using GameFolders.Scripts.Events;
using GameFolders.Scripts.Views;
using TMPro;
using UnityEngine;

namespace GameFolders.Scripts.Presenters
{
    public class GamePlayPresenter : BasePresenter
    {
        private int _correctMoveCount;

        public override void Receive(BaseEventArgs baseEventArgs)
        {
            switch (baseEventArgs)
            {
                case MoveCountChangedEventArgs moveCountChangedEventArgs:
                    SetCurrentMoveCount(moveCountChangedEventArgs.MoveCount, _correctMoveCount);
                    break;
            }
        }

        public void SetCurrentMoney(int currentMoney)
        {
            ((GamePlayView)view).currentMoneyText.text = $"{currentMoney}";
        }

        public void SetCurrentShowingLevelNumber(int levelNumber)
        {
            ((GamePlayView)view).currentShowingLevelText.text = $"LEVEL {levelNumber}";
        }

        public void SetCurrentMoveCount(int currentMoveCount, int correctMoveCount)
        {
            SetCorrectMoveCount(correctMoveCount);
        }

        public void SetCorrectMoveCount(int correctMoveCount)
        {
            _correctMoveCount = correctMoveCount;
        }

        public void OnSettingsButtonClickHandler()
        {
            BroadcastUpward(new SettingsButtonClickedEventArgs());
        }

        public void OnRevertButtonClickHandler()
        {
            BroadcastUpward(new RevertButtonClickedEventArgs());
        }
    }
}