using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// 기본 48프레임, 6프레임까지 이동, 34까지 베기, 45까지 이동, 9정도에 생성 Duration : 4, StartDelay : 0.3
public class Skill_GiantSword : BaseSkill
{
    // Start is called before the first frame update

    public ManualCollision giantSwordAttackCollision; // 나중에 스킬이 늘어나면 배열로 바꿔줘서, ExcuteAttack에 강화정도의 인덱스를 넣어준다. 그 인덱스


    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null)
    {
        Collider[] colliders = giantSwordAttackCollision?.CheckOverlapBox(targetMask);

        
        // CheckOverlapBox을 통해 얻어온 충돌체마다 데미지 처리를 해준다. 
        foreach (Collider collider in colliders)
        {
            collider.gameObject.GetComponent<IDamageable>()?.TakeDamage((int)(damage), effectPrefab);
        }
    }

    public override void ExcuteParticleSystem()
    {

        SkillManager.instance.SpawnParticle(ESkill.GiantSword, ESkillParticleType.Spawn);

    }

    public override void ExitParticleSystem()
    {


    }
}