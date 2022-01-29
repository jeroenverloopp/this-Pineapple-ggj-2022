using System;
using UnityEngine;

namespace Audio
{
    [Serializable]
    public class AudioData
    {

        public string Name;
        public AudioClip Clip = null;
        public float Volume = 1;
        public float Pitch = 1;
        public bool Loop = false;

    }
}