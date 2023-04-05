﻿using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;


// 플레이어의 SkillType들, 일단 일반 공격은 0, 1, 2 로 고정
public enum EPlayerAttackBehaviorType : int
{
    NormalAttack1,
    NormalAttack2, 
    NormalAttack3,
    Skill1,
    Skill2,
    Skill3,
    Skill4
}

public class Player : MonoBehaviour
{
    private Camera cameraMain;
    private Animator anim;
    private Rigidbody rigid;

    private float hAxis;
    private float vAxis;
    private bool dodge;
    private bool attackKey;
    private bool skillKey_1;
    
    private bool skillKey_2;
    
    private bool skillKey_3;
    
    private bool skillKey_4;

    private bool isDodging;
    private bool isAttacking;
    private bool isCasting;

    private bool isAttackingMove; // 공격할 때 이동시작
    private bool isDodgeReady;
    private float dodgeCoolTime;
    private float dodgeCoolTimeMax;
    private Vector3 dodgeVec;
    private Vector3 moveVec;

    
    private float speed;

    // 임시적으로 확인하기 위해서 public으로 해두었다.
    public List<AttackBehavior> attackBehaviors = new List<AttackBehavior>(); // 가능한 공격 및 스킬을 담은 리스트
    public ManualCollision normalAttackCollision;
    public LayerMask targetMask;

    private EPlayerAttackBehaviorType playerCurrentAttackBehavior; // 플레이어가 현재 수행하고 있는 타입

    #region Unity Methods

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
        isDodgeReady = true;
        dodgeCoolTime = dodgeCoolTimeMax;
    }
     
    void Update()
    {
        GetInput();
        Move();
        AttakingMove(); // 일반 공격하거나 스킬을 캐스팅할 때 살짝씩 이동
        Turn();
        Dodge();
        AttackKey();
        SkillKey();
    }

    // 플레이어의 입력을 받아온다.
    private void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal"); //ad
        vAxis = Input.GetAxisRaw("Vertical"); //ws
        dodge = Input.GetButtonDown("Jump"); // space
        attackKey = Input.GetButtonDown("Fire1"); //마우스 왼쪽
        skillKey_1 = Input.GetButtonDown("Skill1"); // 1번
    }

    #endregion Unity Methods

    #region Helper Methods

    // 플레이어가 현재 수행하고 있는 AttackBehavior가 뭔지 알거나 바꿔줘야 하므로, 다음과 같이 세팅
    public EPlayerAttackBehaviorType GetPlayerAttackBehaviorType() 
    {
        return playerCurrentAttackBehavior;
    }

    public void SetPlayerAttackBehaviorType(EPlayerAttackBehaviorType attackBehaviorType)
    {
        playerCurrentAttackBehavior = attackBehaviorType;
    }

    #endregion Helper Methods;

    #region Move Methods
    private void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        
        if (isDodging)
            moveVec = dodgeVec;

        if (!isAttacking && !isCasting) // 공격 중이 아닌 경우 && 스킬 캐스팅 중이 아닌 경우
        {   
            transform.position += moveVec * speed * Time.deltaTime;

            anim.SetBool("IsRun", moveVec != Vector3.zero);
        }
    
    }

    private void Turn()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (moveVec != Vector3.zero && !isAttacking && !isCasting && !isDodging) //가만이 있을때는 회전 불가 && 공격 중이 아닐 경우 && casting중이 아닐 경우  && 회피 상태가 아닌 경우
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveVec), 20 * Time.deltaTime);
        } 
            
    }

    private void Dodge()
    {
        dodgeCoolTime += Time.deltaTime;

        isDodgeReady = dodgeCoolTime >= dodgeCoolTimeMax;

        if (dodge && moveVec != Vector3.zero 
            && !isAttacking &&!isCasting && isDodgeReady) // 회피 키를 눌렀을 경우 && 움직임이 있는 경우 && 공격 상태가 아닌 경우 && 스킬 시전도 아닌 경우 && 닷지 쿨타임이 충족하는 경우
        {

            dodgeCoolTime = 0f;
            dodgeVec = transform.forward;
            speed *= 2;
            anim.SetTrigger("DoDodge");
            isDodging = true;

            Invoke("DodgeOut", 0.6f);
        }
        else if (dodge && moveVec != Vector3.zero 
            && isAttacking && !isCasting && isDodgeReady) // 회피 키를 눌렀을 경우 && 움직임이 있는 경우 && 공격 상태인 경우 && 스킬 시전도 아닌 경우&& 닷지 쿨타임이 충족하는 경우
        {
            // 하던 공격을 캔슬해야 하므로 취소해준다.
            PlayerAttackEnd();

            // 재생하고 있던 Particle System도 꺼줘야 한다.

            
            int skillsCount = attackBehaviors.Count;

            for (int i = 0; i < skillsCount; i++)
            {
                attackBehaviors[i].ExitParticleSystem();
            }

            isDodging = true;
            dodgeCoolTime = 0f;
            dodgeVec = moveVec;
            transform.rotation = Quaternion.LookRotation(-moveVec).normalized;
            speed *= 2;
            anim.SetTrigger("DoCombatDodge");


            Invoke("CombatDodgeOut", 0.4f);
        }
    }


    private void DodgeOut()
    {
        speed *= 0.5f;
        isDodging = false;

        // Dodge에서 PlayerAttackEnd()를 해줬지만, 애니메이션 Transition 탈출할 때 AttackEnable을 거쳐서 나오면 다시 AttackEnable이 True가 된다.
        // 그러므로 Transition Duration을 0으로 해줘야 한다.
        //PlayerAttackEnd();
    }

    private void CombatDodgeOut()
    {
        speed *= 0.5f;
        isDodging = false;
    }

    private void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }

    private void AttakingMove()
    {
        if (isAttacking || isCasting) // 공격하거나 캐스팅의 경우
        {
            if (isAttackingMove) // 만약 공격 행동을 실행하여 살짝 이동해야 한다면,
            {
                switch ((int)playerCurrentAttackBehavior)
                {
                    case 0:
                        {
                            transform.position += transform.forward * speed * Time.deltaTime * 
                                attackBehaviors[0].attackForce;
                            break;
                        }

                    case 1:
                        {
                            transform.position += transform.forward * speed * Time.deltaTime *
                               attackBehaviors[1].attackForce;
                            break;
                        }

                    case 2:
                        {
                            transform.position += transform.forward * speed * Time.deltaTime *
                               attackBehaviors[2].attackForce;
                            break;
                        }

                    case 3:
                        {
                            transform.position += transform.forward * speed * Time.deltaTime *
                               attackBehaviors[3].attackForce;
                            break;
                        }


                }

                
            }
        }   
    }
    

    #endregion Move Methods

    #region NormalAttack Methods
    private void AttackKey()
    {
        if (attackKey) // 공격키를 눌렀을 경우 
        {
            if(!isAttacking &&!isDodging && !isCasting) // 공격 상태가 아닌 경우 && 회피 상태도 아닌 경우 && 스킬 시전 상태도 아닌 경우
            {
                isAttacking = true;
                anim.SetTrigger("DoAttack"); // 애니메이터에서 Attack 서브스테이트 머신으로 들어가게 만들고
                anim.SetBool("AttackEnd", false); // AttackEnd를 다시 False로 해준다.

                playerCurrentAttackBehavior = 
                    EPlayerAttackBehaviorType.NormalAttack1; // 현재 공격 상태를 기본 공격 1로 해준다.

                attackBehaviors[(int)EPlayerAttackBehaviorType.NormalAttack1].ExcuteParticleSystem(); // 첫번째 공격의 파티클을 재생시켜준다.
            }

            // bool을 변수로 뺐더니 오류 발생

            if (isAttacking && !isDodging && !isCasting && anim.GetBool("AttackEnable"))  // 공격 상태인 경우 && 회피 중이지 않은 경우  && 스킬 시전 상태도 아닌 경우 그리고 다음 공격 애니메이션이 가능한 경우 
            {
                int attackComboNum = anim.GetInteger("AttackCombo");

                switch (attackComboNum) // 콤보가 1이면 2로, 2이면 3로, 3이면 다시 1으로 가게 해준다.
                {
                    case 1:
                        {
                            anim.SetInteger("AttackCombo", 2);

                            playerCurrentAttackBehavior = 
                                EPlayerAttackBehaviorType.NormalAttack2; // 현재 공격 상태를 기본 공격 2로 해준다.

                            attackBehaviors[(int)EPlayerAttackBehaviorType.NormalAttack2].ExcuteParticleSystem();
                            break;
                        }
                    case 2:
                        {
                            anim.SetInteger("AttackCombo", 3);

                            playerCurrentAttackBehavior = 
                                EPlayerAttackBehaviorType.NormalAttack3; // 현재 공격 상태를 기본 공격 3으로 해준다.

                            attackBehaviors[(int)EPlayerAttackBehaviorType.NormalAttack3].ExcuteParticleSystem();
                            break;
                        }
                    case 3:
                        {
                            anim.SetInteger("AttackCombo", 1);

                            playerCurrentAttackBehavior = 
                                EPlayerAttackBehaviorType.NormalAttack1; // 현재 공격 상태를 기본 공격 1로 해준다.

                            attackBehaviors[(int)EPlayerAttackBehaviorType.NormalAttack1].ExcuteParticleSystem();
                            break;
                        }
                }

            }

        }

       
    }

    // 애니메이션 이벤트들을 public으로 해야 외부에서 호출 가능하다.
    // 그리고 애니메이션 이벤트 함수와 똑같은 메소드 이름 쓰면 동시에 실행되므로 다르게 해야 한다.

    // 공격 애니메이션의 시작부터 호출
    public void PlayerAttackStart()
    {
        anim.SetBool("AttackEnable", false);

        isAttackingMove = true;
    }

    // 공격 애니메이션 중 타격 부분을 실행하면 호출
    public void PlayerAttack()
    {
        
        int attackComboNum = anim.GetInteger("AttackCombo");
        
        switch (attackComboNum)
        { 
            case 1:
                {
                    attackBehaviors[(int)EPlayerAttackBehaviorType.NormalAttack1].ExcuteAttack(); 
                    break; 
                }

            case 2:
                {
                    attackBehaviors[(int)EPlayerAttackBehaviorType.NormalAttack2].ExcuteAttack();
                    break;
                }

            case 3:
                {
                    attackBehaviors[(int)EPlayerAttackBehaviorType.NormalAttack3].ExcuteAttack();
                    break;
                }
        }

    }


    // 공격 애니메이션 중 다음 공격이 가능한 시점부터 호출 
    public void PlayerAttackEnable()
    {
        anim.SetBool("AttackEnable", true);

        isAttackingMove = false;
    }

    // 공격 애니메이션 중 거의 끝나갈 때 쯤 호출
    public void PlayerAttackEnd()
    {
        anim.SetBool("AttackEnable", false);
        anim.SetInteger("AttackCombo", 1);
        anim.SetBool("AttackEnd", true);
        isAttacking = false;

    }


    #endregion Attack Methods

    #region Skill Methods

    // 우선 스킬키를 누르면 정해진 대로 스킬이 나가지만, 이 순서는 나중에 변경 가능하도록 하는게 어떤지
    // 예를 들어 SkillKey_1이 찌르기를 넣으면 찌르기가, 다른 베기를 넣으면 베기가 나가도록
    private void SkillKey()
    {
        if(skillKey_1) // 스킬 1번을 눌렀을 때
        {
            if(!isAttacking && !isDodging && !isCasting) // 우선 아직까지는 일반 공격 상태에서 캔슬은 불가능하게
            {
                isCasting = true;
                anim.SetTrigger("DoSkill");
                anim.SetInteger("SkillNumber", 1);
                anim.SetBool("SkillEnd", false); // SkillEnd를 다시 False로 해준다.
                playerCurrentAttackBehavior = EPlayerAttackBehaviorType.Skill1;

                attackBehaviors[(int)EPlayerAttackBehaviorType.Skill1].ExcuteParticleSystem();
            }
            
        }

    }

    public void PlayerSkillStart()
    {
        isAttackingMove = true;
    }

    // 공격 애니메이션 중 타격 부분을 실행하면 호출
    public void PlayerSkill()
    {

        int SkillNum = anim.GetInteger("SkillNumber");

        switch (SkillNum)
        {
            case 0:
                {
                    break;
                }

            case 1:
                {
                    attackBehaviors[(int)EPlayerAttackBehaviorType.Skill1].ExcuteAttack();
                    break;
                }

            case 2:
                {
                    attackBehaviors[(int)EPlayerAttackBehaviorType.Skill2].ExcuteAttack();
                    break;
                }

            case 3:
                {
                    attackBehaviors[(int)EPlayerAttackBehaviorType.Skill3].ExcuteAttack();
                    break;
                }

            case 4:
                {
                    attackBehaviors[(int)EPlayerAttackBehaviorType.Skill4].ExcuteAttack();
                    break;
                }
        }

    }


    // 공격 애니메이션 중 다음 공격이 가능한 시점부터 호출 
    public void PlayerSkillEnable()
    {
        isAttackingMove = false;
    }

    // 공격 애니메이션 중 거의 끝나갈 때 쯤 호출
    public void PlayerSkillEnd()
    {

        anim.SetInteger("SkillNumber", 0);
        anim.SetBool("SkillEnd", true);
        isCasting = false;
    }

    #endregion Skill Methods
}

