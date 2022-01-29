using UnityEngine;
public class BaseBreedBehaviour : BaseCreatureBehaviour
{
    protected BreedState breedState = BreedState.Cooldown;

    protected float breedTimer;

    protected enum BreedState { Breeding, Cooldown, Idle };

    public override void ActiveBehaviourStart()
    {
        // Nothing...
    }

    public override void ActiveBehaviourUpdate()
    {
        if (breedState != BreedState.Idle)
        {
            breedTimer -= Time.deltaTime;

            if (breedTimer <= 0)
            {
                if (breedState == BreedState.Breeding)
                    Hatch();
                else if (breedState == BreedState.Cooldown)
                {
                    breedState = BreedState.Idle;
                    Priority = 0;
                }
            }
        }
        else if(Priority > 50)
        {
            StartBreed();
        }
    }

    protected virtual void Hatch()
    {
        Debug.Log("HATCH");
        //Instantiate creature somewhere nearby.
        var child = Object.Instantiate(_creature.creatureData.Prefab, (new Vector2(_creature.transform.position.x, _creature.transform.position.y) + new Vector2(Random.Range(-1, 1), Random.Range(-1, 1))), Quaternion.identity);
        SetCoolDown();
    }

    protected virtual void StartBreed()
    {
        Debug.Log($"The breeding has started for {_creature.transform.name}");
        breedState = BreedState.Breeding;
        breedTimer = _creature.creatureData.BreedingTime;
        Priority = 101;
    }

    public override void InactiveUpdate()
    {
        Priority -= (Time.deltaTime * 0.1f);

        if (breedState == BreedState.Cooldown)
        {
            Priority = 0;
        }
    }

    public override void InactiveWhenInReach(Collider2D collider)
    {
        if (!collider.isTrigger)
        {
            if (collider.gameObject.tag.Equals(_creature.gameObject.tag) && breedState == BreedState.Idle && _creature.creatureData.CanBreed)
            {
                if (_creature.gameObject.GetInstanceID() > collider.gameObject.GetInstanceID())
                    Priority = 100;
            }
        }
    }

    public override void InactiveWhenInSight(Collider2D collider)
    {
    }

    public override void ActiveBehaviourStop()
    {
        if (breedState == BreedState.Breeding)
        {
            SetCoolDown();
        }
    }

    private void SetCoolDown()
    {
        breedState = BreedState.Cooldown;
        breedTimer = _creature.creatureData.BreedingCooldown;
    }

    public override void InactiveWhileInSight(Collider2D collider)
    {
    }

    public override void ActiveWhileInSight(Collider2D collider)
    {
    }
}
