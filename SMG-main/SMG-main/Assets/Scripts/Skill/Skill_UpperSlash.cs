using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_UpperSlash : BaseSkill
{

    [SerializeField]
    GameObject slashObject;

    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null)
    {
        Vector3 vec = transform.position;

        if (startPoint)
        {
            vec = startPoint.position;
        }

        GameObject go = Instantiate(slashObject, 
            SkillManager.instance.player.skillSpawnPos[(int)ESkillType.UpperSlash].position,
             SkillManager.instance.player.skillSpawnPos[(int)ESkillType.UpperSlash].rotation);

        go.GetComponent<Projectile>().SetDamage((int)(damage));
        go.GetComponent<Projectile>().SetTarget(targetMask);

    }

    public override void ExcuteParticleSystem()
    {

        SkillManager.instance.SpawnParticle(mSkillType, mParticleType);

    }

    public override void ExitParticleSystem()
    {


    }
}
