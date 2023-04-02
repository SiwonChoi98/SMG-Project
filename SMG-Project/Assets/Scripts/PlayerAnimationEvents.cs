using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    public void AttackStart()
    {
        GetComponent<Player>()?.AttackStart();
    }
    public void Attack()
    {
        GetComponent<Player>()?.Attack();
    }

    public void AttackEnable()
    {
        GetComponent<Player>()?.AttackEnable();
    }

    public void AttackEnd()
    {
        GetComponent<Player>()?.AttackEnd();

    }
}
