using UnityEngine;

namespace Core.Singletons
{
    public abstract class MonoBehaviourLazySingleton<T> : MonoBehaviour where T: MonoBehaviourLazySingleton<T>
    {
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject($"{typeof(T).Name} [Singleton][Lazy]");
                    go.hideFlags = HideFlags.HideAndDontSave;
                    DontDestroyOnLoad(go);
                    _instance = go.AddComponent<T>();
                }

                return _instance;
            }
        }

        private static T _instance;

    }
}