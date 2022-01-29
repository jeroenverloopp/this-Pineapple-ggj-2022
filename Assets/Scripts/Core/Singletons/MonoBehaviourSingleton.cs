﻿using UnityEngine;

namespace Core.Singletons
{
    public class MonoBehaviourSingleton<T> : MonoBehaviour where T: MonoBehaviourSingleton<T>
    {
        public static T Instance { get; protected set; }
 
        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                throw new System.Exception("An instance of this singleton already exists.");
            }
            else
            {
                Instance = (T)this;
            }
        }
    }
}