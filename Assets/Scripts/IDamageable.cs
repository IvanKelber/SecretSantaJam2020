using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    bool TakeDamage(float damage);

    bool TakeDamage(float damage, Vector3 knockback);
}
