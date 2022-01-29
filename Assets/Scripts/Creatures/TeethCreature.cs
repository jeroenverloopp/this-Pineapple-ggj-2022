using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathFinding.AStar;

public class TeethCreature : BaseCreature
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void WhenInReach(Collider2D collider)
    {
        if (!collider.isTrigger)
        {
            Debug.Log($"Eat the other!: {collider.transform.name}");
            Destroy(collider.gameObject);
        }
    }

    public override void WhenInSight(Collider2D collider)
    {
        if (!collider.isTrigger)
        {
            Debug.Log($"Other spotted!: {collider.transform.name}");
            // Chase other entity...

        }
    }
}
