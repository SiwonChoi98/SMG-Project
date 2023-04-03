using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    public void AttackStart()
    {
        GetComponent<Player>()?.PlayerAttackStart();
    }
    public void Attack()
    {
        GetComponent<Player>()?.PlayerAttack();
    }

    public void AttackEnable()
    {
        GetComponent<Player>()?.PlayerAttackEnable();
    }

    public void AttackEnd()
    {
        GetComponent<Player>()?.PlayerAttackEnd();

    }

}
