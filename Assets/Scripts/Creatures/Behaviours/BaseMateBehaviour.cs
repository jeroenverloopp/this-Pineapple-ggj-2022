using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMateBehaviour : BaseCreatureBehaviour
{
    private Transform mateTarget;

    public override void ActiveBehaviourStart()
    {
    }

    public override void ActiveBehaviourStop()
    {
    }

    public override void ActiveBehaviourUpdate()
    {

    }

    public override void ActiveWhenInReach(Collider2D collider)
    {
    }

    public override void ActiveWhenInSight(Collider2D collider)
    {
    }

    public override void ActiveWhileInSight(Collider2D collider)
    {
    }

    public override void InactiveUpdate()
    {
    }

    public override void InactiveWhenInReach(Collider2D collider)
    {
    }

    public override void InactiveWhenInSight(Collider2D collider)
    {
        
    }

    public override void InactiveWhileInSight(Collider2D collider)
    {
        if (collider.gameObject.tag.Equals(_creature.gameObject.tag) && _creature.creatureData.CanBreed)
        {
            Priority += Time.deltaTime * 4;
        }
    }
}
