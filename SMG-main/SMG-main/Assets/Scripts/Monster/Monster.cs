using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour, IDamageable
{
    [SerializeField] protected string _name;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _curHealth;
    [SerializeField] protected int _speed; //현재는 네비게이션안에 speed 써서 speed는 따로 정의가 필요없다. 추후에 바꿀예정

    [SerializeField] protected float _attackRange; //공격 거리
    [SerializeField] protected float _attackTime; //공격 쿨타임

    public List<BaseSkill> monsterSkills = new List<BaseSkill>(); // 새로추가한 부분, 가능한 공격 및 스킬을 담은 리스트

    public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public int CurHealth { get => _curHealth; set => _curHealth = value; }

    public Rigidbody rigid;
    protected Animator anim;

    [SerializeField] public Transform target;

    protected StateMachine<Monster> stateMachine;

    [Header("상태 체크 변수")]

    public bool isMove = false; //이동할수 있는지 체크 변수
    public bool isAttackRange = false; //공격할수 거리인지 체크 변수
    public bool isAttack = false; //공격 쿨타임 지났는지 체크 변수
    public bool isHit = false; //공격받았는지 체크 변수


    private float _distance; //플레이어(타겟)과의 거리
    protected float _initialAttackTime; //공격 쿨타임 초기화
    public GameObject attackPrefab; //임시) 원거리 공격 오브젝트
    protected float _attackSpeed; //나가는 속도

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        target = GameObject.Find("Player").transform;
        InitStat();
    }

    //임시 능력치 셋팅, 자식 몬스터들이 구현하도록 함
    protected virtual void InitStat()
    {
        //_name = "Monster";
        //MaxHealth = 70;
        //CurHealth = 70;
        //_speed = 5;
        //_damage = 10;
        //_attackRange = 6f;
        //_attackTime = 3f;
        //_initialAttackTime = _attackTime;
        //_attackSpeed = 10f;
    } 
    protected virtual void Start()
    {
        stateMachine = new StateMachine<Monster>(this, new IdleState()); //현재 상태입력
        AddState();//여러 상태 삽입
    }
    protected void AddState()
    {
        stateMachine.AddState(new AttackState()); //모든 상태들 삽입
        stateMachine.AddState(new MoveState());
        stateMachine.AddState(new DeadState());
        stateMachine.AddState(new HitState());
    }
    protected virtual void Update()
    {
        AttackDistanceCheak();
        AttackCoolTime();
        stateMachine.Update(Time.deltaTime); //상태 계속 체크
    }

    // 새로 추가한 부분
    #region IDamageable Methods 
    public bool IsAlive => _curHealth > 0; // 이 부분을 뺄 수도 있음

    public void TakeDamage(int damage, GameObject hittEffectPrefab)
    {
        if (!IsAlive)
        {
            return;
        }

        _curHealth -= damage;
        isHit = true; // 데미지 깎이면서 isHit을 true로

        Debug.Log("Damage : " + damage);

        if (IsAlive)
        {

        }
        else
        {
            //stateMachine.ChangeState<DeadState>(); // 죽으면 DeadState로 가준다.
        }

    }

    #endregion IDamageable Methods

    // 새로바뀐부분, 원래 있던 Hit 주석 처리 해주었다.
    //public virtual void Hit(int damage) 
    //{
    //    CurHealth -= damage;
    //    Debug.Log("Damage : " + damage);
    //    isHit = true;

    //} //임시) HIT

    // 거리 체크해서 공격할 수 있는 상태만들기
    public bool AttackDistanceCheak()
    {
        _distance = Vector3.Distance(this.transform.position, target.transform.position); // 나중에 플레이어와 몬스터의 키를 뺴준다.
        
        if (_distance < _attackRange) // 만약 플레이어가 공격 사거리 내로 들어왔다면,
        {
            return isAttackRange = true;
        }
        else
        {
            return isAttackRange = false;
        }

    }

    // 공격 쿨타임
    public void AttackCoolTime()
    {
        if (!isAttack)
        {
            _attackTime -= Time.deltaTime;
            if (_attackTime < 0)
            {
                _attackTime = _initialAttackTime;
                isAttack = true;
            }
        }
    } 

    public virtual void Shoot()
    {

    }

}
