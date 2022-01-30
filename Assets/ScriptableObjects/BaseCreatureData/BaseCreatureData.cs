using System.Collections.Generic;
using Creatures.Behaviour;
using Food;
using Level;
using ScriptableObjects.BaseCreatureData;
using UnityEngine;

[CreateAssetMenu(menuName = "GGJ - Creatures/Base Creature Data")]
public class BaseCreatureData : ScriptableObject
{
    
    [Header("Visuals")]
    public BaseCreature Prefab;

    public Dictionary<BehaviourState, SpriteAnimation> SpriteAnimations
    {
        get
        {
            if (_spriteAnimations == null)
            {
                _spriteAnimations = new Dictionary<BehaviourState, SpriteAnimation>();
                foreach (var pair in _sprites)
                {
                    _spriteAnimations[pair.State] = pair.SpriteAnimation;
                }
            }

            return _spriteAnimations;
        }
    }
    private Dictionary<BehaviourState, SpriteAnimation> _spriteAnimations;

    [SerializeField] private List<BehaviourStateSpritePair> _sprites;

    [Header("Behaviour")] 
    public List<BaseBehaviour> Behaviours;

    [Header("Stats")]
    public int Nutrition = 50; //Amount of food gained when this creature is eaten.
    public float TimeToGetEaten = 5; //Amount of time before this creature is chomped to bits.
    public float MoveSpeed; //The speed when normally walking
    
    [Header("Idle")]
    public float MinIdleTime = 0;
    public float MaxIdleTime = 5f;
    
    [Header("Fleeing")]
    public List<BaseCreatureData> IsAfraidOfCreatures; //The creatures this creature will flee from.
    public float FleeSpeed = 2; //Run speed while fleeing
    public float MaxFleeDuration = 10; //Max duration of fleeing. (exhaustion)
    public float FleeCoolDown; //Time until the creature can flee again.
    public bool KeepFleeingAfterLosingPredator = false; //If true the creature keeps running even tho it lost its predator.
    
    [Header("Roaming")]
    public float RoamRange = 20f;
    public float MinRoamingTime = 5;
    public float MaxRoamingTime = 15;
    
    
    [Header("Hunger")]
    public float StartHunger = 0; // Spawns with this amount of hunger
    public float HungerGained = 1; // Amount of hunger gained per second
    public float MaxHunger = 100; //Dies when reaches this
    
    [Header("Hunting")]
    public float StartHuntingThreshold = 50; //Amount of hunger before hunting starts Calling for activation
    public float HuntingPriorityIncrease = 0.5f; //Amount of priority that gets added with each added hunger above huntHunger.
    public float FindPreyRange = 15; //The range of vision to find food.
    public float EatPreyRange = 0.3f;
    public List<BaseCreatureData> EatsCreatures; //Creatures it can eat
    
    
    [Header("Foraging")]
    public float StartForagingThreshold = 50; //Amount of hunger before hunting starts Calling for activation
    public float ForagingPriorityIncrease = 0.5f; //Amount of priority that gets added with each added hunger above huntHunger.
    public float FindFoodRange = 1.5f; //The range of vision to find food.
    public float EatFoodRange = 0.3f;
    public List<BaseFood> EatsFood; //Gets nutrition from ground (grass and stuff)
    
    
    [Header("FindingMate & Breeding")]
    public float BreedingTime; //How long it takes to make the magic happen.
    public float BreedingCooldown; //How long before it can search for a mate again.
    public float ReproduceGained = 0.1f; // Amount of Reproduce gained per second.
    public float CanReproduceThreshold = 40;
    public float StartReproduce = 0;
    public float MaxReproduce = 100;
    public float FindMateRange = 10;
    public float StartBreedRange = 1;

}
