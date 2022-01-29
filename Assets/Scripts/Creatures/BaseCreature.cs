using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseCreature : MonoBehaviour
{
    [HideInInspector]
    public float MoveSpeed;

    public BaseCreatureData creatureData;

    [SerializeField]
    protected List<BaseCreatureBehaviour> Behaviours;

    [SerializeField]
    protected BaseCreatureBehaviour ActiveBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        MoveSpeed = creatureData.MoveSpeed;
        Behaviours = GetComponentsInChildren<BaseCreatureBehaviour>().ToList();
        ActiveBehaviour = Behaviours.FirstOrDefault();
        ActiveBehaviour.ActiveBehaviourStart();
    }

    // Update is called once per frame
    void Update()
    {
        ActiveBehaviour.ActiveBehaviourUpdate();
        ManagePriority();
    }

    public virtual void WhenInReach(Collider2D collider)
    {
        foreach (var behaviour in Behaviours)
        {
            behaviour.InactiveWhenInReach(collider);
        }
        ActiveBehaviour.ActiveWhenInReach( collider);
    }

    public virtual void WhileInSight(Collider2D collider)
    {
        foreach (var behaviour in Behaviours)
        {
            behaviour.InactiveWhileInSight(collider);
        }
        ActiveBehaviour.ActiveWhileInSight(collider);
    }

    public virtual void WhenInSight(Collider2D collider)
    {
        foreach (var behaviour in Behaviours)
        {
            behaviour.InactiveWhenInSight(collider);
        }

        ActiveBehaviour.ActiveWhenInSight( collider);
    }

    protected virtual void ManagePriority()
    {
        // Get behaviour with highest priority
        var topPriobehaviour = Behaviours.OrderByDescending(x => x.Priority).FirstOrDefault();

        if (topPriobehaviour != ActiveBehaviour)
        {
            Debug.Log($"Priority change for {transform.name}: {topPriobehaviour.behaviourname}");
            ActiveBehaviour.ActiveBehaviourStop();
            ActiveBehaviour = topPriobehaviour;
        }
    }
}
