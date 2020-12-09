using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Imp
{

    public List<GunConfig> configs = new List<GunConfig>();
    public float attackChangeTimer = 10; //Every <attackChangeTimer> seconds change which gunconfig we are using to attack

    private float timeSinceLastAttackChange = 0;

    public override void Start() {
        base.Start();
        ChangeGun();
    }

    public override void Update() {
        base.Update();
        if(timeSinceLastAttackChange >= attackChangeTimer) {
            ChangeGun();
            timeSinceLastAttackChange = 0;
        }
        timeSinceLastAttackChange += Time.deltaTime;
    }

    public override void TakeDamage(float damage, Vector3 knockback) {
        base.TakeDamage(damage);
    }

    public void ChangeGun() {
        gun.config = configs[Random.Range(0, configs.Count)];
    }
}
