using UnityEngine;

namespace Audio
{
    public class AudioComponent : MonoBehaviour
    {

        private AudioSource _source;

        private bool _played = false;
        
        public void Set(AudioData data)
        {
            _source = gameObject.AddComponent<AudioSource>();
            _source.clip = data.Clip;
            _source.loop = data.Loop;
            _source.pitch = data.Pitch;
            _source.volume = data.Volume;
        }

        public void Play()
        {
            if (_source != null)
            {
                _played = true;
                _source.Play();
            }
        }


        void Update()
        {
            if (_source.loop == false && _source.isPlaying == false && _played)
            {
                Destroy(_source);
                Destroy(this);
            }
        }
        
        
    }
}