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
    public BehaviourState State { get; private set; }
    public float Hunger { get; private set; }
    public bool Alive { get; private set; } = true;
    
    [SerializeField] private MovementComponent _movement;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private BehaviourManager _behaviourManager;

    private void Awake()
    {
        if (_movement == null)
        {
            Debug.LogError("Creature has no movement Component");
        }

        Hunger = creatureData.StartHunger;
    }

    // Start is called before the first frame update
    void Start()
    {
        MoveSpeed = creatureData.MoveSpeed;
        breedState = BreedState.Cooldown;
        breedTimer = creatureData.BreedingCooldown;
        
        _behaviourManager = gameObject.AddComponent<BehaviourManager>();
        _behaviourManager.Init(creatureData.Behaviours , creatureData, this);
    }

    // Update is called once per frame
    void Update()
    {
        //BreedUpdate();

        Hunger = Mathf.Min(Hunger + Time.deltaTime, creatureData.MaxHunger);

        Color s = _spriteRenderer.color;
        _spriteRenderer.color = new Color(s.r, s.g, s.b, 1 - Hunger / creatureData.MaxHunger);
        
        if(Hunger >= creatureData.MaxHunger)
        {
            Die();
        }
        
    }

    public virtual void SetState(BehaviourState state)
    {
        //Debug.Log($"{State} -> {state}");
        State = state;

    }

    public void Eat(BaseCreature creature)
    {
        Hunger = Mathf.Max(Hunger - creature.creatureData.Nutrition, 0);
        Debug.Log(Hunger);
        creature.Die();
    }

    public void Die()
    {
        Alive = false;
        Destroy(gameObject);
    }

    public virtual void WhenInReach(Collider2D collider)
    {
        return;
        if (!collider.isTrigger)
        {
            CheckForBreed(collider);
            CheckForEat(collider);
        }
    }

    public virtual void WhenInSight(Collider2D collider)
    {
        return;
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
