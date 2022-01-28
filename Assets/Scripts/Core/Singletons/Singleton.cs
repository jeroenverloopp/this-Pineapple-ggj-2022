using System;

namespace Core.Singletons
{
    public class Singleton<T>
    {
        
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Activator.CreateInstance<T>();
                }

                return _instance;
            }
        }

        private static T _instance;
    }
}