using System;
using UnityEngine;

namespace GameFolders.Scripts.Objects
{
    public class RotateBehaviour : MonoBehaviour
    {
        [SerializeField] private float rotateSpeed;
        [SerializeField] private bool isRotating;

        public float Speed => rotateSpeed;
        public bool IsRotating => isRotating;

        private void Update()
        {
            if (isRotating)
                transform.Rotate(Vector3.up * rotateSpeed);
        }
    }
}