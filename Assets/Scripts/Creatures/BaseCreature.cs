using UnityEngine;

public class BaseCreature : MonoBehaviour
{
    [HideInInspector]
    public float MoveSpeed;

    public BaseCreatureData creatureData;

    // Start is called before the first frame update
    void Start()
    {
        MoveSpeed = creatureData.MoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void WhenInReach(Collider2D collider)
    {
        if(!collider.isTrigger)
            Debug.Log($"{transform.name} has {collider.transform.name} in reach!");
    }

    public virtual void WhenInSight(Collider2D collider)
    {
        if(!collider.isTrigger)
            Debug.Log($"{transform.name} has {collider.transform.name} in sight!");
    }
}
