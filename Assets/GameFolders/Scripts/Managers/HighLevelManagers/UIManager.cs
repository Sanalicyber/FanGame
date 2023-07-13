using System.ComponentModel;
using Framework.Core;
using GameFolders.Scripts.Controllers;
using GameFolders.Scripts.Events;
using GameFolders.Scripts.Models;
using GameFolders.Scripts.Presenters;
using UnityEngine;

namespace GameFolders.Scripts.Managers.HighLevelManagers
{
    public class UIManager : BaseManager
    {
        #region Private Methods

        public static UIManager Instance { get; private set; }

        [SerializeField] private GamePlayPresenter gamePlayPresenter;
        [SerializeField] private SettingsPresenter settingsPresenter;
        [SerializeField] private SuccessPresenter successPresenter;

        private GameModel _gameModel;

        #endregion

        #region Begining

        protected override void Awake()
        {
            Instance = this;
            gamePlayPresenter.InjectManager(this);
            settingsPresenter.InjectManager(this);
            successPresenter.InjectManager(this);
        }

        protected override void Start()
        {
            gamePlayPresenter.ShowView();
            MoveCounter.OnMoveCountChanged += OnMoveCountChanged;
            OnMoveCountChanged(0);
        }

        private void OnMoveCountChanged(int count)
        {
            BroadcastDownward(new MoveCountChangedEventArgs(count));
        }

        #endregion

        #region Implemented Methods

        public override void Receive(BaseEventArgs baseEventArgs)
        {
            switch (baseEventArgs)
            {
                case NextLevelButtonClickedEventArgs nextLevelButtonClickedEventArgs:
                    Broadcast(nextLevelButtonClickedEventArgs);
                    BroadcastDownward(nextLevelButtonClickedEventArgs);
                    break;
                case RevertButtonClickedEventArgs revertButtonClickedEventArgs:
                    Broadcast(revertButtonClickedEventArgs);
                    break;
                case OnLevelCompletedEventArgs onLevelCompletedEventArgs:
                    BroadcastDownward(onLevelCompletedEventArgs);
                    MoveCounter.ResetMoveCount();
                    break;
                case OnLevelCreatedEventArgs onLevelCreatedEventArgs:
                    BroadcastDownward(onLevelCreatedEventArgs);
                    gamePlayPresenter.SetCurrentMoveCount(0, onLevelCreatedEventArgs.CorrectMoveCount);
                    break;
            }
        }

        private void OnGameModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_gameModel.Money))
            {
                gamePlayPresenter.SetCurrentMoney(_gameModel.Money);
            }
            else if (e.PropertyName == nameof(_gameModel.ShowingLevelNumber))
            {
                gamePlayPresenter.SetCurrentShowingLevelNumber(_gameModel.ShowingLevelNumber);
            }
        }

        #endregion

        #region Public Methods

        public void InjectModel(GameModel gameModel)
        {
            _gameModel = gameModel;

            gamePlayPresenter.SetCurrentMoney(gameModel.Money);
            gamePlayPresenter.SetCurrentShowingLevelNumber(gameModel.ShowingLevelNumber);

            _gameModel.PropertyChanged += OnGameModelPropertyChanged;
        }

        #endregion
    }
}