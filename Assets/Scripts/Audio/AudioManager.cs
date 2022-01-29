using System.Collections.Generic;
using Core.Singletons;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviourSingleton<AudioManager>
    {


        [SerializeField] private List<AudioData> _audioDataList;


        private Dictionary<string, AudioData> _audioDict;

        void Awake()
        {
            _audioDict = CreateDictionary();

            Play("BackgroundMusic1");
            Play("BackgroundMusic2");
        }

        public AudioComponent Play(string name, bool play = true)
        {
            if (_audioDict.ContainsKey(name))
            {
                AudioComponent audioComponent = gameObject.AddComponent<AudioComponent>();
                audioComponent.Set(_audioDict[name]);
                if (play)
                {
                    audioComponent.Play();
                }
                return audioComponent;
            }
            else
            {
                Debug.Log($"AudioManager could not find AudioData with name {name}");
            }

            return null;
        }

        private Dictionary<string, AudioData> CreateDictionary()
        {
            Dictionary<string, AudioData> dict = new Dictionary<string, AudioData>();
            foreach (var audioData in _audioDataList)
            {
                if (dict.ContainsKey(audioData.Name) == false)
                {
                    dict[audioData.Name] = audioData;
                }
            }

            return dict;
        }

    }
}