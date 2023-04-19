using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Shield : BaseSkill
{


    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null)
    {
        // 이 부분에 DefenseCount를 3으로 해서, 맞을 때마다 까이는 식으로 하는 건 어떤지? 중첩 때문에 이런 방식이 나을 것 같다.
        SkillManager.instance.player.ShieldCount += 3; 
        
    }


    public override void ExcuteParticleSystem()
    {
        SkillManager.instance.AttachParticle(mSkillType, mParticleType);
    }

    public override void ExitParticleSystem()
    {

    }
}
