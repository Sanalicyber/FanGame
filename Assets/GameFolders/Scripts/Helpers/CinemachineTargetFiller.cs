using System;
using System.Collections.Generic;
using Cinemachine;
using GameFolders.Scripts.Controllers;
using UnityEngine;

namespace GameFolders.Scripts.Helpers
{
    public class CinemachineTargetFiller : MonoBehaviour
    {
        private List<FanController> _fanControllers;
        private BallController _ballController;
        private CinemachineTargetGroup _targetGroup;

        public void Fill()
        {
            _targetGroup = GetComponent<CinemachineTargetGroup>();
            _fanControllers = new List<FanController>(FindObjectsOfType<FanController>());
            _ballController = FindObjectOfType<BallController>();
            _fanControllers.ForEach(x => _targetGroup.AddMember(x.transform, 1, 10));
            _targetGroup.AddMember(_ballController.transform, 2, 10);
        }
    }
}