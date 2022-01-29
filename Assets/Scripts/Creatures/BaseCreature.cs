using System;
using Creatures;
using Creatures.Behaviour;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaseCreature : MonoBehaviour
{
    [HideInInspector]
    public float MoveSpeed;

    public BaseCreatureData creatureData;

    [SerializeField]
    protected BreedState breedState = BreedState.Cooldown;
    protected float breedTimer;
    protected enum BreedState { Breeding, Cooldown, Idle };


    public MovementComponent Movement => _movement;

    [SerializeField] private MovementComponent _movement;
    private BehaviourManager _behaviourManager;

    private void Awake()
    {
        if (_movement == null)
        {
            Debug.LogError("Creature has no movement Component");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MoveSpeed = creatureData.MoveSpeed;
        breedState = BreedState.Cooldown;
        breedTimer = creatureData.BreedingCooldown;
        
        _behaviourManager = gameObject.AddComponent<BehaviourManager>();
        if (creatureData.Behaviours == null)
        {
            Debug.Log(GetType());
        }
        _behaviourManager.Init(creatureData.Behaviours , creatureData, this);
    }

    // Update is called once per frame
    void Update()
    {
        //BreedUpdate();
    }

    public virtual void WhenInReach(Collider2D collider)
    {
        if (!collider.isTrigger)
        {
            CheckForBreed(collider);
            CheckForEat(collider);
        }
    }

    public virtual void WhenInSight(Collider2D collider)
    {
        if (!collider.isTrigger)
        {
            //Debug.Log($"{transform.name} has {collider.transform.name} in sight!");
        }
    }

    protected void StartBreed()
    {
        //Debug.Log($"The breeding has started for {transform.name}");
        breedState = BreedState.Breeding;
        breedTimer = creatureData.BreedingTime;
    }
    protected void BreedUpdate()
    {
        if (breedState != BreedState.Idle)
        {
            breedTimer -= Time.deltaTime;

            if (breedTimer <= 0)
            {
                if (breedState == BreedState.Breeding)
                    Hatch();
                else if (breedState == BreedState.Cooldown)
                    breedState = BreedState.Idle;
            }
        }
    }

    protected void Hatch()
    {
        //Debug.Log("HATCH");
        // Instantiate new Fluff Creature somewhere close by
        var junior = Instantiate(creatureData.Prefab, (new Vector2(transform.position.x, transform.position.y) + new Vector2(Random.Range(-1, 1), Random.Range(-1, 1))), Quaternion.identity);
        breedState = BreedState.Cooldown;
        breedTimer = creatureData.BreedingCooldown;
    }

    private void CheckForBreed(Collider2D collider)
    {
        if (collider.gameObject.tag.Equals(gameObject.tag) && breedState == BreedState.Idle && creatureData.CanBreed)
        {
            if (gameObject.GetInstanceID() > collider.gameObject.GetInstanceID())
                StartBreed();
        }
    }

    private void CheckForEat(Collider2D collider)
    {
        var creature = collider.gameObject.GetComponent<BaseCreature>();
        if (creature == null)
        {
            return;
        }
        
        if (creatureData.EatsCreatures.Contains(creature.creatureData))
        {
            Debug.Log($"Eat the other!: {collider.transform.name}");
            Destroy(collider.gameObject);
        }
    }
}
