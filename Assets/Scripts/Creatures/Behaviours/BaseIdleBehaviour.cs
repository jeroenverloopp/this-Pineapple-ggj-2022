using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseIdleBehaviour : BaseCreatureBehaviour
{
    public int BasePriority = 25;
    private void Awake()
    {
        Priority = BasePriority;
    }
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
    }

    public override void ActiveWhileInSight(Collider2D collider)
    {
    }
}
