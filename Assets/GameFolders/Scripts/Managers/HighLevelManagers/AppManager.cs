using Framework.Core;
using GameFolders.Scripts.Models;

namespace GameFolders.Scripts.Managers.HighLevelManagers
{
    public class AppManager : BaseManager
    {
        private GameModel _gameModel;

        public override void Receive(BaseEventArgs baseEventArgs)
        {
        }

        public void InjectModel(GameModel gameModel)
        {
            this._gameModel = gameModel;
        }
    }
}