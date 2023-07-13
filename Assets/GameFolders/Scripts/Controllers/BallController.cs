using System;
using System.Collections;
using DG.Tweening;
using GameFolders.Scripts.GridSystem.ExternalGrid;
using GameFolders.Scripts.Helpers;
using GameFolders.Scripts.Managers.HighLevelManagers;
using GameFolders.Scripts.Managers.MidLevelManagers;
using Lofelt.NiceVibrations;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Grid = GameFolders.Scripts.GridSystem.Grid;

namespace GameFolders.Scripts.Controllers
{
    [Serializable]
    public class BallMoveEvent : UnityEvent
    {
    }

    public class BallController : Revertable
    {
        public static event Action BallExplodeEvent, OnBallMoveEnd, OnBallMoveStart;
        public static event Action<bool> BallMaterialChangeEvent;

        [FormerlySerializedAs("Face")] [SerializeField]
        private Transform face;

        [SerializeField] private Vector3 faceOffSet;
        [SerializeField] private Ease ease = Ease.InOutCirc;
        [SerializeField] private float reachTime = 1f;
        [SerializeField] private bool isMovingToInitialPosition;

        [FormerlySerializedAs("MinPositions")] [SerializeField]
        private Vector2 minPositions;

        [FormerlySerializedAs("MaxPositions")] [SerializeField]
        private Vector2 maxPositions;

        private BallMaterialController _ballMaterialController;

        [FormerlySerializedAs("Manager")] public PlayerManager manager;

        [SerializeField] public BallMoveEvent OnBallMove;

        private Vector3 _initialPosition;
        private bool _destroyCalled;

        private float _localTime, _maxLocalTime = 1.5f;

        private bool _isMoving;

        public bool IsMoving
        {
            get => _isMoving;
            set
            {
                _isMoving = value;
                _localTime = 0;

                if (value)
                {
                    AddRevertModel();
                    OnBallMoveStart?.Invoke();
                }
                else
                {
                    OnBallMoveEnd?.Invoke();
                }

                BallMaterialChangeEvent?.Invoke(value);
            }
        }

        protected void Start()
        {
            OnRevert += OnOnRevert;
            _ballMaterialController = GetComponent<BallMaterialController>();
            manager = FindObjectOfType<PlayerManager>();
            _initialPosition = transform.position;
            BallExplosionController.Instance._ballController = this;
            _destroyCalled = false;
        }

        private void OnOnRevert(RevertModel obj)
        {
            MainThread.Instance.StartCoroutine(DoMove(obj.Position));
        }

        private void Update()
        {
            if (IsMoving)
            {
                if (_localTime < _maxLocalTime)
                {
                    _localTime += Time.deltaTime;
                }
                else
                {
                    IsMoving = false;
                }
            }

            if (_destroyCalled) return;

            face.position = transform.position + faceOffSet;
            face.rotation = Quaternion.LookRotation(transform.position - face.position);
        }

        public void DoMove(Vector3 targetPosition, float t)
        {
            transform.DOKill();
            IsMoving = true;
            transform.DOMove(targetPosition, t).SetEase(Ease.InOutSine).OnComplete(() => { IsMoving = false; });
        }

        public void DoMove(Vector3 targetPosition, float t, Ease ease)
        {
            transform.DOKill();
            IsMoving = true;
            transform.DOMove(targetPosition, t).SetEase(ease).OnComplete(() => { IsMoving = false; });
        }

        public bool DoMove(Vector3 targetPosition, float t, Ease ease, FanController fanController)
        {
            transform.DOKill();
            IsMoving = true;
            transform.DOMove(targetPosition, t).SetEase(ease).OnComplete(() => { IsMoving = false; });
            return IsMoving;
        }

        public IEnumerator DoMove(Vector3 targetPosition, FanController fanController = null)
        {
            if (_destroyCalled) yield break;
            if (_isMoving)
            {
                transform.DOKill(false);
                IsMoving = false;
            }
            else if (transform.position == _initialPosition)
            {
                OnBallMove?.Invoke();
            }

            transform.DOKill();

            var currentPos = transform.position.ToVector3Int();

            Vibrator.Haptic(HapticPatterns.PresetType.Selection);

            if (Vector3.Distance(currentPos, transform.position) > .2f)
            {
                transform.DOMove(currentPos, .2f).OnComplete(OnComplete);

                yield return new WaitForSeconds(.25f);
            }

            if (targetPosition.x > maxPositions.x) targetPosition.x = maxPositions.x;
            else if (targetPosition.x < minPositions.x) targetPosition.x = minPositions.x;

            if (targetPosition.z > maxPositions.y) targetPosition.z = maxPositions.y;
            else if (targetPosition.z < minPositions.y) targetPosition.z = minPositions.y;

            var between = transform.InverseTransformPoint(targetPosition).normalized;
            between = between.ToVector3Int();

            if (GameManager.Instance.GameIsVeritcal)
                between.z = 0;
            else
                between.y = 0;

            IsMoving = true;
            if (between == Vector3.right || between == Vector3.left)
            {
                transform.DOMove(targetPosition, reachTime).SetEase(ease).OnComplete(OnComplete);
                // transform.DORotate(Mathf.Abs(currentPos.x - targetPosition.x) * 90 * Vector3.forward, reachTime,
                //     RotateMode.WorldAxisAdd);
            }
            else if (between == Vector3.up || between == Vector3.down)
            {
                transform.DOMove(targetPosition, reachTime).SetEase(ease).OnComplete(OnComplete);
                // transform.DORotate(Mathf.Abs(currentPos.y - targetPosition.y) * 90 * Vector3.forward, reachTime,
                //     RotateMode.WorldAxisAdd);
            }
            else if (between == Vector3.forward || between == Vector3.back)
            {
                transform.DOMove(targetPosition, reachTime).SetEase(ease).OnComplete(OnComplete);
                // transform.DORotate(Mathf.Abs(currentPos.z - targetPosition.z) * 90 * Vector3.forward, reachTime,
                //     RotateMode.WorldAxisAdd);
            }

            void OnComplete()
            {
                IsMoving = false;
                isReverting = false;
                Vibrator.Haptic(HapticPatterns.PresetType.Selection);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Obstacle"))
            {
                transform.DOKill();
                var snappedPos = transform.position.ToVector3Int();
                transform.DOMove(snappedPos, .5f)
                    .OnComplete(() =>
                    {
                        IsMoving = false;
                        isMovingToInitialPosition = false;
                    });
            }

            if (other.CompareTag("Finish"))
            {
                manager.CompleteLevel();
                Vibrator.Success();
            }

            if (other.CompareTag("Respawn"))
            {
                if (_destroyCalled) return;
                BallExplodeEvent?.Invoke();
                Vibrator.Failure();
                _destroyCalled = true;
                transform.DOScale(0, .5f);
                Observable.Timer(TimeSpan.FromSeconds(2f)).Subscribe(_ => LevelManager.Instance.RestartLevel());
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Obstacle") && !isMovingToInitialPosition)
            {
                transform.DOKill();
                isMovingToInitialPosition = true;
                var snappedPos = transform.position.ToVector3Int();
                transform.DOMove(snappedPos, .5f)
                    .OnComplete(() =>
                    {
                        IsMoving = false;
                        isMovingToInitialPosition = false;
                    });
            }
        }

        private void OnDestroy()
        {
            OnRevert -= OnOnRevert;
        }
    }
}