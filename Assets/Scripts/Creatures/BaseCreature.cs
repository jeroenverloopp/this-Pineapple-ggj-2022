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


    public MovementComponent Movement => _movement;
    public BehaviourState State;// { get; private set; }
    public float Hunger;// { get; private set; }
    public bool Alive;// { get; private set; };
    public float Age;// { get; private set; }
    public float Nutrition;
    
    [SerializeField] private MovementComponent _movement;
    [SerializeField] private SpriteAnimationComponent _animation;
    [SerializeField] private SpriteRenderer _body;
    private BehaviourManager _behaviourManager;

    public float Reproduce;

    protected virtual void Awake()
    {
        if (_movement == null)
        {
            Debug.LogError("Creature has no movement Component");
        }

        Hunger = creatureData.StartHunger;
        Reproduce = creatureData.StartReproduce;
        Age = Reproduce;
        Alive = true;
        Nutrition = creatureData.Nutrition;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        MoveSpeed = creatureData.MoveSpeed;
        
        _behaviourManager = gameObject.AddComponent<BehaviourManager>();
        _behaviourManager.Init(creatureData.Behaviours , creatureData, this);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(!Alive) return;
        
        Hunger = Mathf.Min(Hunger + Time.deltaTime*creatureData.HungerGained, creatureData.MaxHunger);
        float amount = Mathf.Min(1, Hunger / creatureData.MaxHunger);
        _body.color = new Color(1-amount, 1-amount, 1-amount);

        Reproduce = Mathf.Min(Reproduce + Time.deltaTime * creatureData.ReproduceGained, creatureData.MaxReproduce);
        Age += Time.deltaTime * creatureData.ReproduceGained;

        float minScale = .6f;
        float addedScale = (1 - minScale) * Mathf.Min(Age / creatureData.CanReproduceThreshold, 1);

        transform.localScale = Vector3.one * (minScale + addedScale);
        
        if(Hunger >= creatureData.MaxHunger && Alive)
        {
            Die();
        }
    }

    public virtual void SetState(BehaviourState state)
    {
        if (State != state)
        {
            State = state;
            _animation.SetAnimation(creatureData.SpriteAnimations[State]);
        }
    }

    public void Eat(BaseCreature creature)
    {
        Hunger = Mathf.Max(Hunger - creature.creatureData.Nutrition, 0);
        creature.Nutrition = 0;
    }

    public void Kill(BaseCreature creature)
    {
        creature.Die();
    }

    public virtual void PlayEatSound(){}
    public virtual void PlayDieSound(){}
    public virtual void PlayBreedSound(){}
    
    private void Die()
    {
        Alive = false;
    }
}
