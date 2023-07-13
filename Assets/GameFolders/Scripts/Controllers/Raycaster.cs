using System;
using GameFolders.Scripts.Helpers;
using UnityEngine;

namespace GameFolders.Scripts.Controllers
{
    public class Raycaster : MonoBehaviour
    {
        [SerializeField] private FanController fanController;
        [SerializeField] private bool locked;

        private void Start()
        {
            fanController = transform.parent.GetComponent<FanController>();
        }

        private void FixedUpdate()
        {
            if (locked) return;
            if (!fanController.isActive) return;
            if (!Physics.Raycast(transform.position, transform.forward, out var hit)) return;
            if (!(hit.distance < fanController.Power)) return;

            BallController ballController = hit.transform.GetComponent<BallController>();
            if (ballController == null) return;
            var targetPos = CalculateTargetPosition(hit.transform.position, hit.distance);
            if (Vector3.Distance(targetPos, hit.transform.position) <= 0.1f) return;
            BallController.OnBallMoveEnd += OnBallMoveEnd;
            locked = true;

            StartCoroutine(ballController.DoMove(targetPos, fanController));
        }

        private void OnBallMoveEnd()
        {
            BallController.OnBallMoveEnd -= OnBallMoveEnd;
            locked = false;
        }

        private Vector3 CalculateTargetPosition(Vector3 hitPos, float distance)
        {
            int fanPower = Math.Min(4, fanController.Power);
            var forward = transform.forward.ToVector3Int();
            hitPos = hitPos.ToVector3Int();
            var result = hitPos + ((fanPower + 1) * forward) - (Convert.ToInt32(distance) * forward);
            return result;
        }
    }
}