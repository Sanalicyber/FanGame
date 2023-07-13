using System;
using UnityEngine;

namespace GameFolders.Scripts.Controllers
{
    public class CurrentCameraReference : MonoBehaviour
    {
        private void Start()
        {
            FanMovementController.Instance.MainCam = GetComponent<Camera>();
        }
    }
}