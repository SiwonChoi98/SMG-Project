using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultRangeMonster : Monster
{
    protected override void InitStat()
    {
        _name = "DefaultRangeMonster";
        MaxHealth = 30;
        CurHealth = 30;
        _speed = 5;
        _attackRange = 6f;
        _attackTime = 3f;
        _initialAttackTime = _attackTime;
        _attackSpeed = 10f;
    } //임시 능력치 셋팅

    public override void Shoot()
    {

        transform.LookAt(target.position);
        GameObject gameObject = Instantiate(attackPrefab);
        gameObject.transform.position = transform.position + new Vector3(0, 1, 0);
        gameObject.transform.rotation = gameObject.transform.rotation;
        Rigidbody rigid = gameObject.GetComponent<Rigidbody>();
        rigid.velocity = transform.forward * _attackSpeed;
        Destroy(gameObject, 3f);
    } //임시) 원거리공격
}
