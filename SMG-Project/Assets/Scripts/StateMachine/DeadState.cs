using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State<Monster>
{
    public Animator animator;

    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        context.StartCoroutine(DeadStateAnim());
    }

    public override void Update(float deltaTime)
    {
       
        
    }

    public override void OnExit()
    { 
    }

    IEnumerator DeadStateAnim()
    {
        animator?.SetTrigger("isDead");
        yield return new WaitForSeconds(1.6f);
        GameObject.Destroy(context.gameObject);
    }
}
