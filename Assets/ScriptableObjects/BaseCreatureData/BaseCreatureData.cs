using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GGJ - Creatures/Base Creature Data")]
public class BaseCreatureData : ScriptableObject
{
    public float MoveSpeed;

    public List<BaseCreatureData> EatsCreatures;

    public List<BaseCreatureData> IsAfraidOfCreatures;
}
