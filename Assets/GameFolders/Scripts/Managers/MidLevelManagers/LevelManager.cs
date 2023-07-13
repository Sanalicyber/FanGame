using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Framework.Core;
using GameFolders.Scripts.Controllers;
using GameFolders.Scripts.Events;
using GameFolders.Scripts.Helpers;
using GameFolders.Scripts.Models;
using GameFolders.Scripts.Objects;
using UnityEngine;

namespace GameFolders.Scripts.Managers.MidLevelManagers
{
    public class LevelManager : BaseManager
    {
        #region Private Variables

        public static LevelManager Instance;
        public GameModel _gameModel;

        [SerializeField] private List<LevelItem> _levelItems;

        public LevelItem LoadedLevel;

        #endregion

        #region Implented Methods

        public override void Receive(BaseEventArgs baseEventArgs)
        {
            switch (baseEventArgs)
            {
                case ResetTheManagersEventArgs resetTheManagersEventArgs:
                    ResetTheLevelManager();
                    break;
            }
        }

        #endregion

        #region Public Methods

        protected override void Start()
        {
            base.Start();
            Instance = this;
        }

        public void LoadLevel()
        {
            LoadedLevel = Instantiate(_levelItems[_gameModel.LevelID]);
            BaseEventArgs tempEvent = new OnLevelCreatedEventArgs();
            Broadcast(tempEvent);
            BroadcastUpward(tempEvent);
        }

        public void NextLevel()
        {
            DisposeLevel();
            _gameModel.LevelID++;
            LoadLevel();
        }

        public void InjectModel(GameModel gameModel)
        {
            _gameModel = gameModel;
        }

        public void RestartLevel()
        {
            StartCoroutine(RestartLevelRoutine());
        }

        private IEnumerator RestartLevelRoutine()
        {
            DisposeLevel();

            yield return new WaitForSeconds(.01f);

            LoadLevel();
        }

        internal void DisposeLevel()
        {
            if (LoadedLevel != null)
            {
                Destroy(LoadedLevel.gameObject);
            }

            var canvasses = FindObjectsOfType<FanCanvasBehaviour>();
            foreach (var canvas in canvasses)
            {
                canvas.StartDestroy();
            }

            ObjectPool.RecycleAll();
        }

        #endregion

        #region Incoming Receive Events

        private void ResetTheLevelManager()
        {
        }

        #endregion
    }
}