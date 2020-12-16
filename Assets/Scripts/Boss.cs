using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Imp
{

    public List<GunConfig> configs = new List<GunConfig>();
    public float attackChangeTimer = 10; //Every <attackChangeTimer> seconds change which gunconfig we are using to attack

    private float timeSinceLastAttackChange = 0;

    public float radius = 5;
    public float angleChangeRate = 15;
    private float angle = 0;
    private Vector3 startPosition;

    public override void Start() {
        base.Start();
        ChangeGun();
        startPosition = transform.position;
    }
    public void Circles() {
        Vector3 destination = startPosition + new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0) * radius;
        movement.Move((destination - transform.position) * Time.deltaTime);
        angle += angleChangeRate * Time.deltaTime;
    }

    public override void Update() {
        base.Update();
        Circles();
        if(timeSinceLastAttackChange >= attackChangeTimer) {
            ChangeGun();
            timeSinceLastAttackChange = 0;
        }
        timeSinceLastAttackChange += Time.deltaTime;
    }

    public override bool TakeDamage(float damage, Vector3 knockback) {
        return base.TakeDamage(damage);
    }

    public void ChangeGun() {
        gun.config = configs[Random.Range(0, configs.Count)];
    }
}
