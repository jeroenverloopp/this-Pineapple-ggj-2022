using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using PathFinding.AStar;

public class TeethCreature : BaseCreature
{
    
    public override void PlayDieSound()
    {
        AudioComponent ac = AudioManager.Instance.Play("TeethDie");
        ac.SetPitch(Random.Range(.9f, 1f));
    }
    
    public override void PlayEatSound()
    {
        AudioComponent ac = AudioManager.Instance.Play("TeethEat");
        ac.SetPitch(Random.Range(.9f, 1f));
    }
    
    public override void PlayBreedSound()
    {
        AudioComponent ac = AudioManager.Instance.Play("TeethBreed");
        ac.SetPitch(Random.Range(.9f, 1f));
    }
}
