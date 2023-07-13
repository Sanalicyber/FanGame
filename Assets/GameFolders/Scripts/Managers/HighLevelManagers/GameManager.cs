using DG.Tweening;
using Framework.Core;
using GameFolders.Scripts.Enums;
using GameFolders.Scripts.Events;
using GameFolders.Scripts.Managers.MidLevelManagers;
using GameFolders.Scripts.Models;
using UnityEngine;

namespace GameFolders.Scripts.Managers.HighLevelManagers
{
    public class GameManager : BaseManager
    {
        #region Private Variables

        [SerializeField] private LevelManager levelManager;
        [SerializeField] private InputManager inputManager;
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private HapticManager hapticManager;
        [SerializeField] private RevertManager revertManager;

        public static GameManager Instance { get; private set; }
        public bool GameIsVeritcal;
        public Camera MainCamera, MouseCam;

        private GameModel _gameModel;

        #endregion

        #region Begining

        protected override void Start()
        {
            base.Start();
// #if UNITY_EDITOR || DEBUG
//          SRDebug.Init();
// #endif
        }

        protected override void Awake()
        {
            Instance = this;
            levelManager.InjectManager(this);
            inputManager.InjectManager(this);
            playerManager.InjectManager(this);
            audioManager.InjectManager(this);
            hapticManager.InjectManager(this);
            revertManager.InjectManager(this);

            var mediator = new BaseMediator();
            levelManager.InjectMediator(mediator);
            inputManager.InjectMediator(mediator);
            playerManager.InjectMediator(mediator);
            audioManager.InjectMediator(mediator);
            hapticManager.InjectMediator(mediator);
            revertManager.InjectMediator(mediator);

            Application.targetFrameRate = 60;
        }

        #endregion

        #region Implemented Methods

        public override void Receive(BaseEventArgs baseEventArgs)
        {
            switch (baseEventArgs)
            {
                case OnLevelCreatedEventArgs onLevelCreatedEventArgs:
                    Broadcast(onLevelCreatedEventArgs);
                    break;
                case ResetTheManagersEventArgs resetTheManagersEventArgs:
                    ResetTheGame(true);
                    BroadcastDownward(resetTheManagersEventArgs);
                    Broadcast(resetTheManagersEventArgs);
                    break;
                case NextLevelButtonClickedEventArgs nextLevelButtonClickedEventArgs:
                    levelManager.NextLevel();
                    break;
                case OnLevelCompletedEventArgs onLevelCompletedEventArgs:
                    Broadcast(onLevelCompletedEventArgs);
                    break;
                case RevertButtonClickedEventArgs revertButtonClickedEventArgs:
                    BroadcastDownward(revertButtonClickedEventArgs);
                    break;
                case AddRevertModelEventArgs addRevertModelEventArgs:
                    BroadcastDownward(addRevertModelEventArgs); 
                    break;
            }
        }

        public void InjectModel(GameModel gameModel)
        {
            _gameModel = gameModel;

            levelManager.InjectModel(gameModel);
            playerManager.InjectModel(gameModel);
            inputManager.InjectModel(gameModel);
            audioManager.InjectModel(gameModel);
            hapticManager.InjectModel(gameModel);

            LoadLevel(true);
        }

        #endregion

        #region Private Methods

        private void AnalyticsEventSender(AnalyticsEventTypesEnum eventType)
        {
            switch (eventType)
            {
                case AnalyticsEventTypesEnum.StartEvent:
                    break;
                case AnalyticsEventTypesEnum.ComplateEvent:
                    break;
                case AnalyticsEventTypesEnum.DesiginEvent:
                    break;
            }
        }

        #endregion

        #region Incoming Receive Events

        private void LoadLevel(bool isSendAnalyticsEvent)
        {
            if (isSendAnalyticsEvent)
            {
                AnalyticsEventSender(AnalyticsEventTypesEnum.StartEvent);
                AnalyticsEventSender(AnalyticsEventTypesEnum.DesiginEvent);
            }

            levelManager.LoadLevel();
        }

        private void NextLevel()
        {
            // _gameModel.LevelID++;
            ResetTheGame(true);
        }

        private void ResetTheGame(bool isLoadLevel)
        {
            BroadcastDownward(new ResetTheManagersEventArgs());
            levelManager.DisposeLevel();
            DOTween.KillAll();
            if (isLoadLevel) LoadLevel(false);
        }

        #endregion
    }
}