using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �⺻ ���� 1 :  Duration : 0.5, StartDelay : 0.15, ��ġ�� ���� �������� , 24 ������
// �⺻ ���� 2 : Duration : 0.56, StartDelay : 0.15, ��ġ�� ���� �������� , 26 ������
// �⺻ ���� 3 : Duration : 0.6, StartDelay : 0.3, ��ġ�� ���� �������� , 38 ������ 

public class NormalAttack : Skill
{
    public ManualCollision normalAttackCollision;

  
    public override void ExcuteAttack(GameObject target = null, Transform startPoint = null) 
    {
        Collider[] colliders = normalAttackCollision?.CheckOverlapBox(targetMask);

        // CheckOverlapBox�� ���� ���� �浹ü���� ������ ó���� ���ش�.
        foreach (Collider collider in colliders)
        {
            
        }
        // ��¦ ������ ���� ȿ���� �ֱ� ���� rigid�� ������ �о���.
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
