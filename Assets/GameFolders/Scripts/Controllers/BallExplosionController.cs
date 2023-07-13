using System;
using GameFolders.Scripts.Helpers;
using UnityEngine;

namespace GameFolders.Scripts.Controllers
{
    public class BallExplosionController : Singleton<BallExplosionController>
    {
        [SerializeField] private GameObject _explosionPrefab;
        public BallController _ballController;

        private void Start()
        {
            BallController.BallExplodeEvent += OnBallExplode;
        }

        private void OnBallExplode()
        {
            Instantiate(_explosionPrefab, _ballController.transform.position, Quaternion.identity).GetComponent<ExplodeObject>().BallController = _ballController;
        }

        private void OnDestroy()
        {
            BallController.BallExplodeEvent -= OnBallExplode;
        }
    }
}