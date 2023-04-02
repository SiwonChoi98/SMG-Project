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

    // �ӽ������� Ȯ���ϱ� ���ؼ� public���� �صξ���.
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
        
        if (attackKey) // ����Ű�� ������ ��� 
        {
            if(!isAttacking &&!isDodging) // ���� ���°� �ƴ� ��� && ȸ�� ���µ� �ƴ� ���
            {
                isAttacking = true;
                anim.SetTrigger("DoAttack"); // �ִϸ����Ϳ��� Attack ���꽺����Ʈ �ӽ����� ���� �����
                anim.SetBool("AttackEnd", false); // AttackEnd�� �ٽ� False�� ���ش�.
            }

            if (isAttacking && !isDodging && anim.GetBool("AttackEnable"))  // ���� ������ ��� && ȸ�� ������ ���� ��� && �׸��� ���� ���� �ִϸ��̼��� ������ ��� 
            {
                switch (anim.GetInteger("AttackCombo")) // �޺��� 0�̸� 1��, 1�̸� 2��, 2�̸� �ٽ� 0���� ���� ���ش�.
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

    // �ִϸ��̼� �̺�Ʈ���� public���� �ؾ� �ܺο��� ȣ�� �����ϴ�.

    // ���� �ִϸ��̼��� ���ۺ��� ȣ�� / addforce�� Attack�κ��� �����ؾ� �ڿ���������.
    public void AttackStart()
    {
        anim.SetBool("AttackEnable", false);
        
    }

    // ���� �ִϸ��̼� �� Ÿ�� �κ��� �����ϸ� ȣ�� / ���⿡ manual collision �Լ� ȣ��
    public void Attack()
    {
        Collider[] colliders = normalAttackCollision?.CheckOverlapBox(targetMask);

        foreach (Collider collider in colliders)
        {
            // �Ʒ��� ���� �浹ü�� ������ ������
            //collider.gameObject.GetComponent<Monster>().TakeDamge()

            // ��¦ ������ ���� ȿ���� �ֱ� ���� rigid�� ������ �о���.
            rigid.AddForce(transform.forward * attackForce, ForceMode.Impulse);
            // ���߿� �� Ÿ���� ����� �� �������� ȸ���ϰ� ����.
        }
    }


    // ���� �ִϸ��̼� �� ���� ������ ������ �������� ȣ�� 
    public void AttackEnable()
    {
        anim.SetBool("AttackEnable", true);

        
    }

    // ���� �ִϸ��̼� �� ���� ������ �� �� ȣ��
    public void AttackEnd()
    {
        isAttacking = false;
        anim.SetBool("AttackEnable", false); ;
        anim.SetInteger("AttackCombo", 0);
        anim.SetBool("AttackEnd", true);
       

    }

    #endregion Attack Methods
}