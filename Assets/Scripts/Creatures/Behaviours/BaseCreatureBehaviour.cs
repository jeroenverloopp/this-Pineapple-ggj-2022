using UnityEngine;

public abstract class BaseCreatureBehaviour : MonoBehaviour
{
    public string behaviourname = "Behaviour";

    [SerializeField]
    private float _priority;
    public float Priority
    {
        get 
        {
            return _priority;
        }
        set 
        {
            _priority = Mathf.Max(Mathf.Min(100, value), 0);
        }
    }

    protected BaseCreature _creature;

    private void Start()
    {
        _creature = GetComponent<BaseCreature>();
    }

    private void Update()
    {
        InactiveUpdate();
    }

    // Start is called before the first frame update
    public virtual void ActiveBehaviourStart()
    {
        
    }

    // Update is called once per frame
    public virtual void ActiveBehaviourUpdate()
    {
        
    }

    public virtual void InactiveUpdate()
    {
        
    }

    public virtual void ActiveWhenInReach(Collider2D collider)
    {
        
    }

    public virtual void InactiveWhenInReach(Collider2D collider)
    {
        
    }

    public virtual void InactiveWhileInSight(Collider2D collider)
    {
        
    }

    public virtual void ActiveWhenInSight(Collider2D collider)
    {
        
    }

    public virtual void ActiveWhileInSight(Collider2D collider)
    {
        
    }

    public virtual void InactiveWhenInSight(Collider2D collider)
    {
        
    }

    public virtual void InactiveWhenOutOfSight(Collider2D collider) { }

    public virtual void WhenOutOfSight(Collider2D collider) { }

    public virtual void ActiveBehaviourStop() { }
}
