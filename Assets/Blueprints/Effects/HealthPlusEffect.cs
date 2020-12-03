using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Effects/HealthPlus")]
public class HealthPlusEffect : RewardEffect
{
    public int maxHealthIncrease = 5;
    public int currentHealthIncrease = 0;

    public override void Apply(PlayerValues values) {
        values.maxHealth += maxHealthIncrease;
        values.currentHealth += currentHealthIncrease;
    }

}
