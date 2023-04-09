using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State<Monster>
{
    public Animator animator;

    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        context.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 0f; //임시
        Debug.Log("Attack 상태로 진입");
    }

    public override void Update(float deltaTime)
    {
        if (context.target)
        {
            if (context.isHit) // 플레이어에게 맞으면 무조건 Hit, 몬스터 종류에 따라 안넘어갈수도 있다.
            {
                stateMachine.ChangeState<HitState>();
                return;
            }
            if (!context.isAttackRange) // 공격 사거리 내에 들어와있지 않다면 Idle로 이동
            {
                stateMachine.ChangeState<IdleState>();
                return;
            }
            if (context.isAttack) //공격할수있으면 다시 공격
            {
                animator?.SetTrigger("isAttack");
                context.Shoot();
                Debug.Log("Attack 성공");
                context.isAttack = false;
                return;
            }
        }

    }

    public override void OnExit()
    {
        context.isAttack = false;
    }
}
