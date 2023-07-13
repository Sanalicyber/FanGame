using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameFolders.Scripts.Controllers;
using GameFolders.Scripts.Managers.HighLevelManagers;
using UnityEngine;
using UnityEngine.Events;
using Vector3 = UnityEngine.Vector3;

namespace GameFolders.Scripts.Objects
{
    [Serializable]
    public class TutorialEndEvent : UnityEvent {}
    
    public class TutorialLevelItem : LevelItem
    {
        public float MouseCamSize = 5f;
        public Vector3 MouseCamPosition;

        public bool hasTutorial = true;
        public bool hasTwoTutorial = false;

        public GameObject Tutorial1, Tutorial2;

        public TutorialEndEvent OnFirstTutorialEnd, OnTutorialEnd;

        public Transform TargetObjectToMove;
        public FanController TemporaryFanController;
        private Vector3 _targetObjectStartPosition;
        public static bool tutorial3Completed;

        protected override void Start()
        {
            base.Start();

            if(TargetObjectToMove != null)
                _targetObjectStartPosition = TargetObjectToMove.position;

            GameManager.Instance.MouseCam.orthographicSize = MouseCamSize;
            GameManager.Instance.MouseCam.transform.position = MouseCamPosition;
            if(!hasTutorial)
            {
                if(Tutorial1 != null)
                    Tutorial1.SetActive(false);
                if(Tutorial2 != null)
                    Tutorial2.SetActive(false);
            }
            else
            {
                if(Tutorial1 != null)
                    Tutorial1.SetActive(true);
                if(Tutorial2 != null)
                    Tutorial2.SetActive(false);
            }

            // if (levelIndex > ID)
            // {
            //     if(Tutorial2 != null)
            //         Tutorial2.SetActive(true);
            //     OnFirstTutorialEnd?.Invoke();
            // }

            if (ID == 3 && tutorial3Completed)
            {
                OnFirstTutorialEnd?.Invoke();
            }
        }

        public void OnFirstTutorialComplete()
        {
            if(hasTwoTutorial)
            {
                if(Tutorial1 != null)
                    Tutorial1.SetActive(false);
                if(Tutorial2 != null)
                    Tutorial2.SetActive(true);
                OnFirstTutorialEnd?.Invoke();
            }
            else
            {
                OnTutorialComplete();
            }
        }

        public void OnFirstTutorialComplete(float delay)
        {
            IEnumerator delayed()
            {
                yield return new WaitForSeconds(delay);
                if(hasTwoTutorial)
                {
                    if(Tutorial1 != null)
                        Tutorial1.SetActive(false);
                    if(Tutorial2 != null)
                        Tutorial2.SetActive(true);
                    OnFirstTutorialEnd?.Invoke();
                }
                else
                {
                    OnTutorialComplete();
                }
            }

            if (ID == 3)
            {
                tutorial3Completed = true;
            }

            StartCoroutine(delayed());
        }

        public void OnTutorialComplete()
        {
            if(Tutorial1 != null)
                Tutorial1.SetActive(false);
            if (Tutorial2 != null)
                Tutorial2.SetActive(false);

            if(!hasTwoTutorial)
                OnFirstTutorialEnd?.Invoke();
            else
                OnTutorialEnd?.Invoke();
        }

        public void ReturnToInitialPos()
        {
            TemporaryFanController.Reset();
            TargetObjectToMove.DOMove(_targetObjectStartPosition, .5f);
        }
    }
}