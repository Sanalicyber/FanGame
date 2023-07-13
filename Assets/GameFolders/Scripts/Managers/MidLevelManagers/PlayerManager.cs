using Framework.Core;
using GameFolders.Scripts.Controllers;
using GameFolders.Scripts.Events;
using GameFolders.Scripts.Helpers;
using GameFolders.Scripts.Models;
using UnityEngine;

namespace GameFolders.Scripts.Managers.MidLevelManagers
{
    public class PlayerManager : BaseManager
    {
        #region Private Variables
        public static PlayerManager Instance;

        private GameModel _gameModel;

        #endregion

        protected override void Awake()
        {
            base.Awake();
            Instance = this;
        }

        #region Implemented Methods

        public override void Receive(BaseEventArgs baseEventArgs)
        {

        }

        #endregion

        #region Public Methods

        public void InjectModel(GameModel gameModel)
        {
            _gameModel = gameModel;
        }

        public void AddRevertModel(RevertModel model)
        {
            BroadcastUpward(new AddRevertModelEventArgs(model));
        }

        #endregion

        public void CompleteLevel()
        {
            var completedEvent = new OnLevelCompletedEventArgs(MoveCounter.MoveCount);
            BroadcastUpward(completedEvent);
        }
    }
}