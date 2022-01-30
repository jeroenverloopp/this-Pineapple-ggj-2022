using Audio;
using UnityEngine;

public class FluffCreature : BaseCreature
{
    /*
    public override void WhenInReach(Collider2D collider)
    {
        base.WhenInReach(collider);

        if (!collider.isTrigger)
        {

        }
    }

    public override void WhenInSight(Collider2D collider)
    {
        base.WhenInSight(collider);

        if (!collider.isTrigger)
        {
            // Move away from current target
        }
    }
    */

    public override void PlayDieSound()
    {
        AudioComponent ac = AudioManager.Instance.Play("FluffDie");
        ac.SetPitch(Random.Range(.9f, 1f));
    }
    
    public override void PlayEatSound()
    {
        AudioComponent ac = AudioManager.Instance.Play("FluffEat");
        ac.SetPitch(Random.Range(.6f, .9f));
    }
    
    public override void PlayBreedSound()
    {
        AudioComponent ac = AudioManager.Instance.Play("FluffBreed");
        ac.SetPitch(Random.Range(.9f, 1.1f));
    }
}
