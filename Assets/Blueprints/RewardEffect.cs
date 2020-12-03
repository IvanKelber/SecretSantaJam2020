using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RewardEffect : ScriptableObject
{
    public abstract void Apply(PlayerValues values);
}
