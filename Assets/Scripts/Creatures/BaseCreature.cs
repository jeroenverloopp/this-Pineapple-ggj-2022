using Creatures;
using Creatures.Behaviour;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaseCreature : MonoBehaviour
{
    [HideInInspector]
    public float MoveSpeed;

    public ColliderProxy reachCollider, sightCollider;

    public BaseCreatureData creatureData;


    public MovementComponent Movement => _movement;

    [SerializeField] private MovementComponent _movement;
    private BehaviourManager _behaviourManager;

    public BehaviourState State { get; private set; }
    
    public float Hunger { get; private set; }

    public float Reproduce;

    private void Awake()
    {
        if (_movement == null)
        {
            Debug.LogError("Creature has no movement Component");
        }

        Hunger = creatureData.StartHunger;
        Reproduce = creatureData.StartReproduce;
    }

    // Start is called before the first frame update
    void Start()
    {
        MoveSpeed = creatureData.MoveSpeed;
        
        _behaviourManager = gameObject.AddComponent<BehaviourManager>();
        _behaviourManager.Init(creatureData.Behaviours , creatureData, this);
    }

    // Update is called once per frame
    void Update()
    {
        Hunger = Mathf.Min(Hunger + Time.deltaTime, creatureData.MaxHunger);
        if(Hunger >= creatureData.MaxHunger)
        {
            Destroy(gameObject);
        }

        Reproduce = Mathf.Min(Reproduce + Time.deltaTime * creatureData.ReproduceGained, creatureData.MaxReproduce);
    }

    public virtual void SetState(BehaviourState state)
    {
        Debug.Log($"{transform.name} sets state from {State} to {state}.");
        State = state;
    }
}
