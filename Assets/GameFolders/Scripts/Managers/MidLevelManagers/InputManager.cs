using Framework.Core;
using GameFolders.Scripts.Events;
using GameFolders.Scripts.Models;

namespace GameFolders.Scripts.Managers.MidLevelManagers
{
    public class InputManager : BaseManager
    {
        #region Private Variables

        private GameModel _gameModel;

        #endregion

        #region Begining

        #endregion

        #region Implemented Methods

        public override void Receive(BaseEventArgs baseEventArgs)
        {
            switch (baseEventArgs)
            {
                case ResetTheManagersEventArgs resetTheManagersEventArgs:
                    ResetTheInputManager();
                    break;
                case OnPlayerCreatedEventArgs createdEventArgs:
                    break;
            }
        }

        #endregion

        #region Public Methods

        public void InjectModel(GameModel gameModel)
        {
            _gameModel = gameModel;
        }

        #endregion

        #region Private Methods

        #endregion

        #region Incoming Events

        #endregion

        #region Incoming Receive Events

        private void ResetTheInputManager()
        {
        }

        #endregion
    }
}