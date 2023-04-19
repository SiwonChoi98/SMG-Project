using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 스킬 매니저는 오직 플레이어의 스킬 시스템을 위해 존재하는 매니저
// skills와 skillSpawnPos는 ESkill과 일치해야한다. 


public class SkillManager : MonoBehaviour
{   // 스킬 매니저의 역할에 대해서,
    public static SkillManager instance;

    public Player player;

    public GameObject[] skills;


    
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }


    }

    private void Update()
    {

    }

    public void SpawnParticle(ESkillType skillType, ESkillParticleType particleType) // 레벨은 나중에 넣든지 하자.
    {
        if (particleType == ESkillParticleType.Spawn) // 만약 타입이 스폰형 스킬이라면
        {
            // 자이언트 소드 소환 스킬
            if (skillType == ESkillType.GiantSword) // 이 아래에 Switch (level)에  따라 다른 파티클 시스템이 나갈 예정
            {
                GameObject GiantSwordSkill = Instantiate(skills[(int)ESkillType.GiantSword], 
                        player.skillSpawnPos[(int)ESkillType.GiantSword].position,
                        player.skillSpawnPos[(int)ESkillType.GiantSword].rotation); 

                ParticleSystem[] particleSystems = GiantSwordSkill.GetComponentsInChildren<ParticleSystem>();  

                foreach (ParticleSystem particle in particleSystems)
                {
                    particle.Play(); // 각 위치에 맞게 파티클 재생
                }
                Destroy(GiantSwordSkill, 4f);
            }
            
            // 땅으로 내려찍는 스킬
            else if (skillType == ESkillType.GroundBreak)
            {
                GameObject GroundBreakSkill = Instantiate(skills[(int)ESkillType.GroundBreak], 
                        player.skillSpawnPos[(int)ESkillType.GroundBreak].position,
                        player.skillSpawnPos[(int)ESkillType.GroundBreak].rotation); 

                ParticleSystem[] particleSystems = GroundBreakSkill.GetComponentsInChildren<ParticleSystem>(); 

                foreach (ParticleSystem particle in particleSystems)
                {
                    particle.Play(); // 각 위치에 맞게 
                }
                Destroy(GroundBreakSkill, 3f);

            }

            else if (skillType == ESkillType.UpperSlash)
            {
                GameObject UpperSlashSkill = Instantiate(skills[(int)ESkillType.UpperSlash],
                        player.skillSpawnPos[(int)ESkillType.UpperSlash].position,
                        player.skillSpawnPos[(int)ESkillType.UpperSlash].rotation);

                ParticleSystem[] particleSystems = UpperSlashSkill.GetComponentsInChildren<ParticleSystem>();

                foreach (ParticleSystem particle in particleSystems)
                {
                    particle.Play(); // 각 위치에 맞게 
                }

                Destroy(UpperSlashSkill, 3f);
            }
        }

    }

    public void AttachParticle(ESkillType skill, ESkillParticleType type)
    {
        if (type == ESkillParticleType.Attach)
        {
            // 찌르는 스킬
            if (skill == ESkillType.Thrust)
            {
                GameObject ThrustSkill = Instantiate(skills[(int)ESkillType.Thrust], 
                            player.skillSpawnPos[(int)ESkillType.Thrust].position,
                            player.skillSpawnPos[(int)ESkillType.Thrust].rotation, 
                            player.transform); 

                ParticleSystem[] particleSystems = ThrustSkill.GetComponentsInChildren<ParticleSystem>(); 

                foreach (ParticleSystem particle in particleSystems)
                {
                    particle.Play(); // 각 위치에 맞게 
                }

                Destroy(ThrustSkill, 1f);
            }

        }
    }

}
