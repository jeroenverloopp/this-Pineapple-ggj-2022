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
                    GameObject go =new GameObject($"Lazy Singleton({typeof(T)})");
                    _instance = go.AddComponent<T>();
                }

                return _instance;
            }
        }

        private static T _instance;

    }
}