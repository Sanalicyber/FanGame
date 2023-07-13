using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Core;
using GameFolders.Scripts.Events;
using GameFolders.Scripts.Helpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameFolders.Scripts.Managers.MidLevelManagers
{
    public class RevertManager : BaseManager
    {
        private List<RevertOrderModel> _revertActions = new List<RevertOrderModel>();

        public int currentOrder = 0;

        private float _localTime, _maxTime = 1f;
        private bool _revertChanging;

        public override void Receive(BaseEventArgs baseEventArgs)
        {
            switch (baseEventArgs)
            {
                case AddRevertModelEventArgs addRevertModelEventArgs:
                    AddRevertModel(addRevertModelEventArgs.Model);
                    break;
                case RevertButtonClickedEventArgs revertButtonClickedEventArgs:
                    RevertByOrder();
                    break;
                case OnLevelCreatedEventArgs onLevelCreatedEventArgs:
                    _revertActions.Clear();
                    currentOrder = 0;
                    break;
            }
        }

        private void AddRevertModel(RevertModel revertModel)
        {
            if (_revertActions == null)
            {
                _revertActions = new List<RevertOrderModel>();
            }

            _revertChanging = true;

            _revertActions.Add(new RevertOrderModel(currentOrder, revertModel));
        }

        private void RevertByOrder()
        {
            if (_revertActions == null)
            {
                return;
            }

            if (_revertActions.Count == 0)
            {
                return;
            }

            currentOrder--;

            var revertingModels = _revertActions.FindAll(x => x.Order == currentOrder);
            revertingModels.ForEach(x => x.RevertModel.RevertableObject.Revert(x.RevertModel));

            _revertActions.RemoveAll(x => x.Order >= currentOrder);
            if (_revertActions.All(x => x.Order != currentOrder - 1)) currentOrder--;
        }

        private void Update()
        {
            if (_revertChanging)
            {
                if (_localTime < _maxTime)
                {
                    _localTime += Time.deltaTime;
                }
                else
                {
                    _revertChanging = false;
                    _localTime = 0;
                    currentOrder++;
                }
            }
        }
    }

    [Serializable]
    public class RevertOrderModel
    {
        public int Order;
        public RevertModel RevertModel;

        public RevertOrderModel(int order, RevertModel revertModel)
        {
            Order = order;
            RevertModel = revertModel;
        }
    }
}