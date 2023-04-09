using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 스킬 매니저는 오직 플레이어의 스킬 시스템을 위해 존재하는 매니저
// skills와 skillSpawnPos는 ESkillOrder와 일치해야한다. 
public enum ESkillOrder 
{ 
    Thrust_1,
    GiantSword_1,
    GroundBreak_1
   
}
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

    public void SpawnParticle(ESkill skill, ESkillParticleType type) // 여기에 public void SpawnParticle(ESkill skill, ESkillParticleType type, ESkillLevel level) 와 같은 형식으로 바꿀 예정
    {
        if (type == ESkillParticleType.Spawn) // 만약 타입이 스폰형 스킬이라면
        {
            // 자이언트 소드 소환 스킬
            if (skill == ESkill.GiantSword) // 이 아래에 Switch (level)에  따라 다른 파티클 시스템이 나갈 예정
            {
                GameObject GiantSwordSkill = Instantiate(skills[(int)ESkillOrder.GiantSword_1], 
                        player.skillSpawnPos[(int)ESkillOrder.GiantSword_1].position,
                        player.skillSpawnPos[(int)ESkillOrder.GiantSword_1].rotation); 

                ParticleSystem[] particleSystems = GiantSwordSkill.GetComponentsInChildren<ParticleSystem>();  

                foreach (ParticleSystem particle in particleSystems)
                {
                    particle.Play(); // 각 위치에 맞게 파티클 재생
                }
                Destroy(GiantSwordSkill, 4f);
            }
            
            // 땅으로 내려찍는 스킬
            else if (skill == ESkill.GroundBreak)
            {
                GameObject GroundBreakSkill = Instantiate(skills[(int)ESkillOrder.GroundBreak_1], 
                        player.skillSpawnPos[(int)ESkillOrder.GroundBreak_1].position,
                        player.skillSpawnPos[(int)ESkillOrder.GroundBreak_1].rotation); 

                ParticleSystem[] particleSystems = GroundBreakSkill.GetComponentsInChildren<ParticleSystem>(); 

                foreach (ParticleSystem particle in particleSystems)
                {
                    particle.Play(); // 각 위치에 맞게 
                }
                Destroy(GroundBreakSkill, 3f);

            }
        }

    }

    public void AttachParticle(ESkill skill, ESkillParticleType type)
    {
        if (type == ESkillParticleType.Attach)
        {
            // 찌르는 스킬
            if (skill == ESkill.Thrust)
            {
                GameObject ThrustSkill = Instantiate(skills[(int)ESkillOrder.Thrust_1], 
                            player.skillSpawnPos[(int)ESkillOrder.Thrust_1].position,
                            player.skillSpawnPos[(int)ESkillOrder.Thrust_1].rotation, 
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
