using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkill_NormalAttack : BaseSkill
{
    public ManualCollision monsterAttackCollision;


    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null)
    {
        Collider[] colliders = monsterAttackCollision?.CheckOverlapBox(targetMask);


        // CheckOverlapBox을 통해 얻어온 충돌체마다 데미지 처리를 해준다. 
        foreach (Collider collider in colliders)
        {
            collider.gameObject.GetComponent<IDamageable>()?.TakeDamage((int)(damage), effectPrefab);
        }


    }


    public override void ExcuteParticleSystem()
    {
       
    }

    public override void ExitParticleSystem()
    {
       

    }
}
