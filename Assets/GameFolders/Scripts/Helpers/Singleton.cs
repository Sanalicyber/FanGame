using System.Linq;
using UnityEngine;

namespace GameFolders.Scripts.Helpers
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance => _instance ? _instance : _instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
    }
}
