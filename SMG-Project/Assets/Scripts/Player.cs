using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;


// �÷��̾��� SkillType��, �ϴ� �Ϲ� ������ 0, 1, 2 �� ����
public enum EPlayerSkillType : int
{
    NormalAttack1,
    NormalAttack2, 
    NormalAttack3
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

    private bool isDodging;
    public bool isAttacking;

    private bool isDodgeReady;
    private float dodgeCoolTime;
    private float dodgeCoolTimeMax;
    private Vector3 dodgeVec;
    private Vector3 moveVec;

    
    private float speed;

    // �ӽ������� Ȯ���ϱ� ���ؼ� public���� �صξ���.
    public List<Skill> skills = new List<Skill>(); // ������ ���� �� ��ų�� ���� ����Ʈ
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

    // �÷��̾��� �Է��� �޾ƿ´�.
    private void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal"); //ad
        vAxis = Input.GetAxisRaw("Vertical"); //ws
        dodge = Input.GetButtonDown("Jump"); // space
        attackKey = Input.GetButtonDown("Fire1"); //���콺 ����
    }

    #region Move Methods
    private void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        
        if (isDodging)
            moveVec = dodgeVec;

        if (!isAttacking) // ���� ���� �ƴ� ��� && ȸ�� ���°� �ƴ� ���
        {   
            transform.position += moveVec * speed * Time.deltaTime;

            anim.SetBool("IsRun", moveVec != Vector3.zero);
        }
    
    }

    private void Turn()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (moveVec != Vector3.zero && !isAttacking && !isDodging) //������ �������� ȸ�� �Ұ� && ���� ���� �ƴ� ���  && ȸ�� ���°� �ƴ� ���
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveVec), 20 * Time.deltaTime);
        } 
            
    }

    private void Dodge()
    {
        dodgeCoolTime += Time.deltaTime;

        isDodgeReady = dodgeCoolTime >= dodgeCoolTimeMax;

        if (dodge && moveVec != Vector3.zero && !isAttacking && isDodgeReady) // ȸ�� Ű�� ������ ��� && �������� �ִ� ��� && ���� ���°� �ƴ� ��� && ���� ��Ÿ���� �����ϴ� ���
        {

            dodgeCoolTime = 0f;
            dodgeVec = transform.forward;
            speed *= 2;
            anim.SetTrigger("DoDodge");
            isDodging = true;

            Invoke("DodgeOut", 0.6f);
        }
        else if (dodge && moveVec != Vector3.zero && isAttacking && isDodgeReady) // ȸ�� Ű�� ������ ��� && �������� �ִ� ��� && ���� ������ ��� && ���� ��Ÿ���� �����ϴ� ���
        {
            // �ϴ� ������ ĵ���ؾ� �ϹǷ� ������ش�.
            PlayerAttackEnd();

            // ����ϰ� �ִ� Particle System�� ����� �Ѵ�. ȸ�� �ϸ� Attack 1�� 2�� ���� ����Ǵ� ���׹߻�

            int skillsCount = skills.Count;

            for (int i = 0; i < skillsCount; i++)
            {

            }
            dodgeCoolTime = 0f;
            dodgeVec = moveVec;
            transform.rotation = Quaternion.LookRotation(moveVec).normalized;
            speed *= 2;
            anim.SetTrigger("DoDodge");
            isDodging = true;

            Invoke("DodgeOut", 0.5f);
        }
    }

    private void DodgeOut()
    {
        speed *= 0.5f;
        isDodging = false;

        // Dodge���� PlayerAttackEnd()�� ��������, �ִϸ��̼� Transition Ż���� �� AttackEnable�� ���ļ� ������ �ٽ� AttackEnable�� True�� �ȴ�.
        // �׷��Ƿ� Transition Duration�� 0���� ����� �Ѵ�.
        //PlayerAttackEnd();
    }

    private void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }

    #endregion Move Methods

    #region Attack Methods
    private void AttackKey()
    {
        
        if (attackKey) // ����Ű�� ������ ��� 
        {
            if(!isAttacking &&!isDodging) // ���� ���°� �ƴ� ��� && ȸ�� ���µ� �ƴ� ���
            {
                isAttacking = true;
                anim.SetTrigger("DoAttack"); // �ִϸ����Ϳ��� Attack ���꽺����Ʈ �ӽ����� ���� �����
                anim.SetBool("AttackEnd", false); // AttackEnd�� �ٽ� False�� ���ش�.

                skills[(int)EPlayerSkillType.NormalAttack1].ExcuteParticleSystem(); // ù��° ������ ��ƼŬ�� ��������ش�.
            }

            // bool�� ������ ������ ���� �߻�

            if (isAttacking && !isDodging && anim.GetBool("AttackEnable"))  // ���� ������ ��� && ȸ�� ������ ���� ��� && �׸��� ���� ���� �ִϸ��̼��� ������ ��� 
            {
                int attackComboNum = anim.GetInteger("AttackCombo");

                switch (attackComboNum) // �޺��� 1�̸� 2��, 2�̸� 3��, 3�̸� �ٽ� 1���� ���� ���ش�.
                {
                    case 1:
                        {
                            anim.SetInteger("AttackCombo", 2);
                            skills[(int)EPlayerSkillType.NormalAttack2].ExcuteParticleSystem();
                            break;
                        }
                    case 2:
                        {
                            anim.SetInteger("AttackCombo", 3);
                            skills[(int)EPlayerSkillType.NormalAttack3].ExcuteParticleSystem();
                            break;
                        }
                    case 3:
                        {
                            anim.SetInteger("AttackCombo", 1);
                            skills[(int)EPlayerSkillType.NormalAttack1].ExcuteParticleSystem();
                            break;
                        }
                }

            }

        }

       
    }

    // �ִϸ��̼� �̺�Ʈ���� public���� �ؾ� �ܺο��� ȣ�� �����ϴ�.
    // �׸��� �ִϸ��̼� �̺�Ʈ �Լ��� �Ȱ��� �޼ҵ� �̸� ���� ���ÿ� ����ǹǷ� �ٸ��� �ؾ� �Ѵ�..

    // ���� �ִϸ��̼��� ���ۺ��� ȣ��
    public void PlayerAttackStart()
    {
        anim.SetBool("AttackEnable", false);
        
    }

    // ���� �ִϸ��̼� �� Ÿ�� �κ��� �����ϸ� ȣ��
    public void PlayerAttack()
    {
        int attackComboNum = anim.GetInteger("AttackCombo");
        
        switch (attackComboNum)
        { 
            case 1:
                {
                    skills[(int)EPlayerSkillType.NormalAttack1].ExcuteAttack(); 
                    break; 
                }

            case 2:
                {
                    skills[(int)EPlayerSkillType.NormalAttack2].ExcuteAttack();
                    break;
                }

            case 3:
                {
                    skills[(int)EPlayerSkillType.NormalAttack3].ExcuteAttack();
                    break;
                }
        }

    }


    // ���� �ִϸ��̼� �� ���� ������ ������ �������� ȣ�� 
    public void PlayerAttackEnable()
    {
        anim.SetBool("AttackEnable", true);

    }

    // ���� �ִϸ��̼� �� ���� ������ �� �� ȣ��
    public void PlayerAttackEnd()
    {
        anim.SetBool("AttackEnable", false);
        anim.SetInteger("AttackCombo", 1);
        anim.SetBool("AttackEnd", true);
        isAttacking = false;

    }

    #endregion Attack Methods
}