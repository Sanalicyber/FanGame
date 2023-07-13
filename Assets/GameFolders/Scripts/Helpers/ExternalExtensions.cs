using System;
using System.Collections;
using GameFolders.Scripts.GridSystem.ExternalGrid;
using UnityEngine;
using UnityEngine.UI;

namespace GameFolders.Scripts.Helpers
{
    public static class ExternalExtensions
    {
        public static void SetListener(this Button.ButtonClickedEvent e, Action a)
        {
            e.RemoveAllListeners();
            e.AddListener(() => a());
        }

        public static void SetListener<T>(this Button.ButtonClickedEvent e, Action<T> a)
        {
            e.RemoveAllListeners();
            e.AddListener(() => a(default(T)));
        }

        public static Vector3Int ToVector3Int(this Vector3 v)
        {
            return new Vector3Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
        }

        public static void Scale(this Transform tr, float end, float time)
        {
            IEnumerator routine()
            {
                float t = 0;
                while (Math.Abs(t - time) < .1f)
                {
                    t += Time.deltaTime;
                    tr.localScale = Vector3.Lerp(tr.localScale, new Vector3(end, end, end), t / time);
                    yield return new WaitForEndOfFrame();
                }
            }

            MainThread.Instance.RunOnMainThread(routine());
        }

        public static int ConvertToInt(this float t)
        {
            string s = t.ToString();
            var i = s.Split(',');
            string ts = "";
            ts = i[0] == "-" ? i[1] : i[0];
            int result = Convert.ToInt32(ts);
            int tn = i[0] == "-" ? Convert.ToInt32(i[2][0]) : Convert.ToInt32(i[1][0]);
            if (tn > 5)
            {
                result += i[0] == "-" ? -1 : 1;
            }
            else
            {
                result -= i[0] == "-" ? 1 : -1;
            }

            return result;
        }
    }
}