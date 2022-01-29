using System.Collections.Generic;
using Creatures.Behaviour;
using Level;
using UnityEngine;

[CreateAssetMenu(menuName = "GGJ - Creatures/Base Creature Data")]
public class BaseCreatureData : ScriptableObject
{
    
    [Header("Visuals")]
    public GameObject Prefab;

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
    
    [Header("Hunting")]
    public float StartHunger = 0; // Spawns with this amount of hunger
    public float HungerGained = 1; // Amount of hunger gained per second
    public float MaxHunger = 100; //Dies when reaches this
    public float HuntHunger = 50; //Amount of hunger before hunting starts Calling for activation
    public float HuntingPriorityIncrease = 0.5f; //Amount of priority that gets added with each added hunger above huntHunger.
    public float FindFoodRange = 15; //The range of vision to find food.
    public float EatRange = 4;
    public List<BaseCreatureData> EatsCreatures; //Creatures it can eat
    public List<GroundType> EatsGround; //Gets nutrition from ground (grass and stuff)
    
    
    [Header("FindingMate & Breeding")]
    public bool CanBreed; //Can this creature breed;
    public float BreedingTime; //How long it takes to make the magic happen.
    public float BreedingCooldown; //How long before it can search for a mate again.
}
