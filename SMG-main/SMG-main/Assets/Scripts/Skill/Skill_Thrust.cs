using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 기본 28프레임, attackforce : 0.5, 3~14정도까지 찌르기동작, 12나 13정도에 파티클 생성 Duration 0.3 , StartDelay : 0.43, 
public class Skill_Thrust : BaseSkill
{
    // Start is called before the first frame update

    public ManualCollision thrustAttackCollision;

    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null)
    {
        ManualCollision collision = GameObject.Find("Thrust_Area").GetComponent<ManualCollision>(); // 만약 할당이 안된 상태라면 저장해준다.

        thrustAttackCollision = collision;

        Collider[] colliders = thrustAttackCollision?.CheckOverlapBox(targetMask);

        // CheckOverlapBox을 통해 얻어온 충돌체마다 데미지 처리를 해준다. 
        foreach (Collider collider in colliders)
        {
            collider.gameObject.GetComponent<IDamageable>()?.TakeDamage((int)(damage), effectPrefab);
        }


    }


    public override void ExcuteParticleSystem()
    {
        SkillManager.instance.AttachParticle(mSkillType, mParticleType);
    }

    public override void ExitParticleSystem()
    {
        foreach (ParticleSystem particles in particleSystems)
        {
            particles.Stop();
        }

    }
}
