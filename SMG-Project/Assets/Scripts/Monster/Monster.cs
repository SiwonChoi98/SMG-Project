using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private string name;  
    [SerializeField] private int maxHealth; 
    [SerializeField] private int curHealth; 
    [SerializeField] private int speed; //현재는 네비게이션안에 speed 써서 speed는 따로 정의가 필요없다. 추후에 바꿀예정
    [SerializeField] private int damage;

    protected Rigidbody rigid;
    protected Animator anim;
    [SerializeField] protected Transform target;

    private void Start()
    {
        rigid.GetComponent<Rigidbody>();
        anim.GetComponent<Animator>();
    }
    protected virtual void Update()
    {
     
    }
}
