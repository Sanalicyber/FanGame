using System.Collections;
using UnityEngine;

namespace GameFolders.Scripts.Helpers
{
    public class MainThread : MonoBehaviour
    {
        private static MainThread _instance;
        public static MainThread Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("MainThread").AddComponent<MainThread>();
                }
                return _instance;
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }

        public void RunOnMainThread(System.Action action)
        {
            StartCoroutine(RunOnMainThreadCoroutine(action, 0));
        }

        public IEnumerator RunOnMainThreadCoroutine(System.Action action, float t)
        {
            if (t == 0)
                yield return null;
            else
                yield return new WaitForSeconds(t);
            action();
        }

        public Coroutine RunOnMainThread(System.Action action, float delay)
        {
            return StartCoroutine(RunOnMainThreadCoroutine(action, delay));
        }

        public Coroutine RunOnMainThread(IEnumerator action)
        {
            return StartCoroutine(action);
        }
    }
}