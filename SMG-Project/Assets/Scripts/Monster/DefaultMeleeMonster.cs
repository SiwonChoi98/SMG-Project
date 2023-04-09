using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultMeleeMonster : Monster
{
    protected override void InitStat()
    {
        _name = "DefaultMeleeMonster";
        MaxHealth = 70;
        CurHealth = 70;
        _speed = 5;
        _attackRange = 2f;
        _attackTime = 3f;
        _initialAttackTime = _attackTime;
        _attackSpeed = 10f;
    } //임시 능력치 셋팅

    public override void Shoot()
    {
        transform.LookAt(target.position); // 때릴 때마다 플레이어 방향으로 돌려준다.

        monsterSkills[0].ExcuteAttack(target.gameObject); // 여기서 몬스터의 첫번째 공격스킬이 나간다. ex) 고블린 평타
    } 
}