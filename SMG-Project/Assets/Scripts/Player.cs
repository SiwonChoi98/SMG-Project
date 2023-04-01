using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Camera cameraMain;
    private Animator anim;
    private Rigidbody rigid;

    private float hAxis;
    private float vAxis;
    private bool dodge;
    private bool attackKey;

    private bool isDodging;
    private bool isAttacking;

    private bool isDodgeReady;
    private float dodgeCoolTime;
    private float dodgeCoolTimeMax;
    private Vector3 dodgeVec;
    public Vector3 moveVec;

    private float attackForce;
    private float speed;

    // 임시적으로 확인하기 위해서 public으로 해두었다.
    public ManualCollision normalAttackCollision;
    public LayerMask targetMask;



    private void Awake()
    {
        cameraMain = Camera.main;
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        dodgeCoolTimeMax = 3.0f;
    }

    void Start()
    {
        speed = 5.0f;
        attackForce = 6.0f;
        isDodgeReady = true;
        dodgeCoolTime = dodgeCoolTimeMax;
    }

    void Update()
    {
        GetInput();
        Move();
        Turn();
        Dodge();
        AttackKey();
    }

    // 플레이어의 입력을 받아온다.
    private void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal"); //ad
        vAxis = Input.GetAxisRaw("Vertical"); //ws
        dodge = Input.GetButtonDown("Jump"); // space
        attackKey = Input.GetButtonDown("Fire1"); //마우스 왼쪽
    }

    #region Move Methods
    private void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        
        if (isDodging)
            moveVec = dodgeVec;

        if (!isAttacking) // 공격 중이 아닌 경우 && 회피 상태가 아닌 경우
        {   
            transform.position += moveVec * speed * Time.deltaTime;

            anim.SetBool("IsRun", moveVec != Vector3.zero);
        }
    
    }

    private void Turn()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (moveVec != Vector3.zero && !isAttacking && !isDodging) //가만이 있을때는 회전 불가 && 공격 중이 아닐 경우  && 회피 상태가 아닌 경우
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveVec), 20 * Time.deltaTime);
        } 
            
    }

    private void Dodge()
    {
        dodgeCoolTime += Time.deltaTime;

        isDodgeReady = dodgeCoolTime >= dodgeCoolTimeMax;

        if (dodge && moveVec != Vector3.zero && !isAttacking && isDodgeReady) // 회피 키를 눌렀을 경우 && 움직임이 있는 경우 && 공격 상태가 아닌 경우 && 닷지 쿨타임이 충족하는 경우
        {
            dodgeCoolTime = 0f;
            dodgeVec = transform.forward;
            speed *= 2;
            anim.SetTrigger("DoDodge");
            isDodging = true;

            Invoke("DodgeOut", 0.6f);
        }
        else if (dodge && moveVec != Vector3.zero && isAttacking && isDodgeReady) // 회피 키를 눌렀을 경우 && 움직임이 있는 경우 && 공격 상태인 경우 && 닷지 쿨타임이 충족하는 경우
        {
            // 하던 공격을 캔슬해야 하므로 취소해준다.
            AttackEnd();

            dodgeCoolTime = 0f;
            dodgeVec = moveVec;
            transform.rotation = Quaternion.LookRotation(moveVec).normalized;
            speed *= 2;
            anim.SetTrigger("DoDodge");
            isDodging = true;

            Invoke("DodgeOut", 0.6f);
        }
    }

    private void DodgeOut()
    {
        speed *= 0.5f;
        isDodging = false;
    }

    private void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }

    #endregion Move Methods

    #region Attack Methods
    private void AttackKey()
    {
        
        if (attackKey) // 공격키를 눌렀을 경우 
        {
            if(!isAttacking &&!isDodging) // 공격 상태가 아닌 경우 && 회피 상태도 아닌 경우
            {
                isAttacking = true;
                anim.SetTrigger("DoAttack"); // 애니메이터에서 Attack 서브스테이트 머신으로 들어가게 만들고
                anim.SetBool("AttackEnd", false); // AttackEnd를 다시 False로 해준다.
            }

            if (isAttacking && !isDodging && anim.GetBool("AttackEnable"))  // 공격 상태인 경우 && 회피 중이지 않은 경우 && 그리고 다음 공격 애니메이션이 가능한 경우 
            {
                switch (anim.GetInteger("AttackCombo")) // 콤보가 0이면 1로, 1이면 2로, 2이면 다시 0으로 가게 해준다.
                {
                    case 0:
                        {
                            anim.SetInteger("AttackCombo", 1);
                            break;
                        }
                    case 1:
                        {
                            anim.SetInteger("AttackCombo", 2);
                            break;
                        }
                    case 2:
                        {
                            anim.SetInteger("AttackCombo", 0);
                            break;
                        }
                }

            }

        }

       
    }

    // 애니메이션 이벤트들을 public으로 해야 외부에서 호출 가능하다.

    // 공격 애니메이션의 시작부터 호출 / addforce는 Attack부분을 변경해야 자연스럽ㄴ다.
    public void AttackStart()
    {
        anim.SetBool("AttackEnable", false);
        
    }

    // 공격 애니메이션 중 타격 부분을 실행하면 호출 / 여기에 manual collision 함수 호출
    public void Attack()
    {
        Collider[] colliders = normalAttackCollision?.CheckOverlapBox(targetMask);

        foreach (Collider collider in colliders)
        {
            // 아래와 같이 충돌체의 정보를 가져옴
            //collider.gameObject.GetComponent<Monster>().TakeDamge()

            // 살짝 앞으로 가는 효과를 주기 위해 rigid를 앞으로 밀어줌.
            rigid.AddForce(transform.forward * attackForce, ForceMode.Impulse);
            // 나중에 적 타겟이 생기면 그 방향으로 회전하게 하자.
        }
    }


    // 공격 애니메이션 중 다음 공격이 가능한 시점부터 호출 
    public void AttackEnable()
    {
        anim.SetBool("AttackEnable", true);

        
    }

    // 공격 애니메이션 중 거의 끝나갈 때 쯤 호출
    public void AttackEnd()
    {
        isAttacking = false;
        anim.SetBool("AttackEnable", false); ;
        anim.SetInteger("AttackCombo", 0);
        anim.SetBool("AttackEnd", true);
       

    }

    #endregion Attack Methods
}