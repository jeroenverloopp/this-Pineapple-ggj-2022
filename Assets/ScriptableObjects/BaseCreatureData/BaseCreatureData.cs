using System.Collections.Generic;
using Level;
using UnityEngine;

[CreateAssetMenu(menuName = "GGJ - Creatures/Base Creature Data")]
public class BaseCreatureData : ScriptableObject
{
    
    [Header("Visuals")]
    public GameObject Prefab;
    
    [Header("Stats")]
    public float Nutrition = 50; //Amount of food gained when this creature is eaten.
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
    public float FindMoveTargetRange = 10f;
    public float MinRoamingTime = 5;
    public float MaxRoamingTime = 15;
    
    [Header("LookingForFood & Eating")]
    public float StartHunger = 0; // Spawns with this amount of hunger
    public float HungerGained = 1; // Amount of hunger gained per second
    public float MaxHunger = 100; //Dies when reaches this
    public List<BaseCreatureData> EatsCreatures; //Creatures it can eat
    public List<GroundType> EatsGround; //Gets nutrition from ground (grass and stuff)
    public float FindFoodRange; //The range of vision to find food.
    
    [Header("FindingMate & Breeding")]
    public bool CanBreed; //Can this creature breed;
    public float BreedingTime; //How long it takes to make the magic happen.
    public float BreedingCooldown; //How long before it can search for a mate again.
}
