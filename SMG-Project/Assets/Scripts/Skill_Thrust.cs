﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 기본 28프레임, attackforce : 0.5, 3~14정도까지 찌르기동작, 12나 13정도에 파티클 생성 Duration 0.2 , StartDelay : 0.43, 
public class Skill_Thrust : AttackBehavior
{
    // Start is called before the first frame update

    public ManualCollision thrustAttackCollision;


    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null)
    {
        Collider[] colliders = thrustAttackCollision?.CheckOverlapBox(targetMask);

        // CheckOverlapBox을 통해 얻어온 충돌체마다 데미지 처리를 해준다. 
        foreach (Collider collider in colliders)
        {

        }


    }


    public override void ExcuteParticleSystem()
    {
        foreach (ParticleSystem particles in particleSystems)
        {
            particles.Play();
        }
    }

    public override void ExitParticleSystem()
    {
        foreach (ParticleSystem particles in particleSystems)
        {
            particles.Stop();
        }

    }
}