using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFolders.Scripts.Controllers
{
    public class BallMaterialController : MonoBehaviour
    {
        public List<Material> materials;
        public Renderer _renderer;

        private int _index;

        private void Start()
        {
            _index = 0;

            BallController.BallMaterialChangeEvent += ChangeMaterial;
        }

        private void ChangeMaterial(bool active)
        {
            _index = active ? 1 : 0;

            _renderer.material = materials[_index];
        }

        private void OnDestroy()
        {
            BallController.BallMaterialChangeEvent -= ChangeMaterial;
        }
    }
}