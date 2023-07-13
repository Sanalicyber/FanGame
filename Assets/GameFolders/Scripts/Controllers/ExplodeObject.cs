using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using GameFolders.Scripts.Helpers;
using UnityEngine;

namespace GameFolders.Scripts.Controllers
{
    public class ExplodeObject : MonoBehaviour
    {
        public BallController BallController;
        [SerializeField] private List<Rigidbody> _rigidbodies;
        private bool destroyCalled;

        private void Start()
        {
            MainThread.Instance.RunOnMainThread(StartProcess, .05f);
        }

        private void StartProcess()
        {
            destroyCalled = true;
            _rigidbodies = GetComponentsInChildren<Rigidbody>().ToList();

            foreach (var rigidbody in _rigidbodies)
            {
                rigidbody.AddExplosionForce(100, transform.position, 10);
            }

            destroyCalled = false;
        }

        private void Update()
        {
            if (BallController == null && !destroyCalled)
            {
                destroyCalled = true;
                Destroy(gameObject);
            }
        }
    }
}