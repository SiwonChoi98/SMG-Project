using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 기본 공격 1 :  Duration : 0.5, StartDelay : 0.15, 위치는 직접 조정했음 , 24 프레임
// 기본 공격 2 : Duration : 0.56, StartDelay : 0.15, 위치는 직접 조정했음 , 26 프레임
// 기본 공격 3 : Duration : 0.6, StartDelay : 0.3, 위치는 직접 조정했음 , 38 프레임 

public class NormalAttack : Skill
{
    public ManualCollision normalAttackCollision;

  
    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null) 
    {
        Collider[] colliders = normalAttackCollision?.CheckOverlapBox(targetMask);

        // CheckOverlapBox을 통해 얻어온 충돌체마다 데미지 처리를 해준다.
        foreach (Collider collider in colliders)
        {
            
        }
        // 살짝 앞으로 가는 효과를 주기 위해 rigid를 앞으로 밀어줌.
        rigid_Player.AddForce(transform.forward * attackForce, ForceMode.Impulse);

    }


    public override void ExcuteParticleSystem()
    {
        foreach (ParticleSystem particles in particleSystems)
        {
            particles.Play();
        }
    }
}
