using System;
using GameFolders.Scripts.Managers.HighLevelManagers;
using UnityEngine;

namespace GameFolders.Scripts.Objects
{
    public class LevelItem : MonoBehaviour
    {
        public int ID;
        public Camera LevelCamera;
        public int CorrectMoveCountForFinish;

        protected virtual void Start()
        {
            GameManager.Instance.MainCamera = LevelCamera;
            GameManager.Instance.MouseCam.orthographicSize = 8.63f;
            GameManager.Instance.MouseCam.transform.position = new Vector3(0, 10, -.63f);
        }
    }
}