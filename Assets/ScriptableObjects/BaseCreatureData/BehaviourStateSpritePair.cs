using System;
using Creatures.Behaviour;
using UnityEngine;

namespace ScriptableObjects.BaseCreatureData
{
    [Serializable]
    public class BehaviourStateSpritePair
    {
        public BehaviourState State;
        public SpriteAnimation SpriteAnimation;
    }
}