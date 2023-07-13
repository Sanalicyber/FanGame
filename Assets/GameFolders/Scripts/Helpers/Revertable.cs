using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameFolders.Scripts.Managers.MidLevelManagers;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameFolders.Scripts.Helpers
{
    public class Revertable : MonoBehaviour
    {
        protected event Action<RevertModel> OnRevert;
        protected bool isReverting;

        public void Revert(RevertModel model)
        {
            if (transform.position == model.Position)
            {
                Debug.Log("Object already reverted.");
                return;
            }

            isReverting = true;

            if (model.RevertableObject.gameObject.activeSelf == false) model.RevertableObject.gameObject.SetActive(true);

            OnRevert?.Invoke(model);
        }

        protected void AddRevertModel()
        {
            if (isReverting) return;
            PlayerManager.Instance.AddRevertModel(new RevertModel(transform.position, transform.rotation, this));
        }
    }

    [Serializable]
    public class RevertModel
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Revertable RevertableObject;

        public RevertModel(Vector3 position, Quaternion rotation, Revertable revertableObject)
        {
            Position = position;
            Rotation = rotation;
            RevertableObject = revertableObject;
        }
    }
}