using System.Collections.Generic;
using Core.Singletons;
using UnityEngine;

namespace Icons
{
    public class IconManager : MonoBehaviourSingleton<IconManager>
    {
        [SerializeField] private List<IconData> _iconDataList;


        private Dictionary<string, IconData> _iconDict;

        protected override void Awake()
        {
            base.Awake();
            _iconDict = CreateDictionary();
        }

        public void Create(string name, Vector2 position)
        {
            if (_iconDict.ContainsKey(name) == false)
            {
                return;
            }
            GameObject go = new GameObject();
            var iconComponent = go.AddComponent<IconComponent>();
            iconComponent.Set(_iconDict[name]);
            go.transform.parent = transform;
            go.transform.position = position;
        }
        
        private Dictionary<string, IconData> CreateDictionary()
        {
            Dictionary<string, IconData> dict = new Dictionary<string, IconData>();
            foreach (var iconData in _iconDataList)
            {
                if (dict.ContainsKey(iconData.Name) == false)
                {
                    dict[iconData.Name] = iconData;
                }
            }

            return dict;
        }
    }
}